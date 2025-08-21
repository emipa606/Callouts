using System.Collections.Generic;
using System.Linq;
using CM_Callouts.PendingCallouts;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace CM_Callouts;

public sealed class CalloutTracker : WorldComponent
{
    private static Dictionary<int, TextMoteQueueTickBased> textMoteQueuesTickBased = new();
    private static Dictionary<int, TextMoteQueueRealTime> textMoteQueuesRealTime = new();

    private static int maxRulesSeen = 1;
    private readonly int checkTicks = 60;
    private readonly int hashCache;
    private readonly int ticksBetweenCallouts = 240;

    private Dictionary<Pawn, int> pawnCalloutExpireTick = new();

    public CalloutTracker(World world) : base(world)
    {
        hashCache = GetHashCode();

        // New world, new queues
        textMoteQueuesTickBased = new Dictionary<int, TextMoteQueueTickBased>();
        textMoteQueuesRealTime = new Dictionary<int, TextMoteQueueRealTime>();
    }

    public override void WorldComponentTick()
    {
        base.WorldComponentTick();

        var currentTickPlusHash = Find.TickManager.TicksGame + hashCache;

        // Replace dictionary with new one without expired values
        if (currentTickPlusHash % checkTicks == 0)
        {
            pawnCalloutExpireTick = pawnCalloutExpireTick.Where(kvp => kvp.Value > currentTickPlusHash)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        UpdateTextMoteQueuesTickBased();
    }

    private static void UpdateTextMoteQueuesTickBased()
    {
        var cleanup = false;
        foreach (var textMoteQueue in textMoteQueuesTickBased.Values)
        {
            cleanup = cleanup || !textMoteQueue.Update();
        }

        if (cleanup)
        {
            textMoteQueuesTickBased = textMoteQueuesTickBased.Where(kvp => kvp.Value.Valid())
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }

    public static void UpdateTextMoteQueuesRealTime()
    {
        var cleanup = false;
        foreach (var textMoteQueue in textMoteQueuesRealTime.Values)
        {
            cleanup = cleanup || !textMoteQueue.Update();
        }

        if (cleanup)
        {
            textMoteQueuesRealTime = textMoteQueuesRealTime.Where(kvp => kvp.Value.Valid())
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }

    // ReSharper disable once UnusedParameter.Global Needed for MoteMaker patch
    public static Thing ThrowText(Thing thing, IntVec3 location, Map map, WipeMode wipeMode = WipeMode.Vanish)
    {
        if (map == null || thing is not MoteText moteText)
        {
            return thing;
        }

        var hash = (map.Index * 1000000) + (location.x * 1000) + location.z;

        if (Find.TickManager.Paused)
        {
            // Meh, the situations I can think of where this is happening (changing temperature on a heater for example) queueing it makes it worse
            GenSpawn.Spawn(thing, location, map);
        }
        else if (!CalloutMod.settings.queueTextMotes)
        {
            thing.def = CalloutDefOf.CM_Callouts_Thing_Mote_Text_Ticked;
            GenSpawn.Spawn(thing, location, map);
        }
        else
        {
            if (!textMoteQueuesTickBased.ContainsKey(hash))
            {
                textMoteQueuesTickBased[hash] = new TextMoteQueueTickBased(location, map);
            }

            textMoteQueuesTickBased[hash].AddMote(moteText);
        }

        return thing;
    }

    public bool CanCalloutNow(Pawn pawn)
    {
        return pawn is { Dead: false, Spawned: true } &&
               (CalloutMod.settings.allowCalloutsForAnimals || pawn.def.race.Humanlike) &&
               !pawnCalloutExpireTick.ContainsKey(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking);
    }

    public bool CheckCalloutChance(CalloutCategory category, RulePackDef rulePackDef, string keyword = "rule")
    {
        var calloutChance = CalloutMod.settings.baseCalloutChance * scaledCalloutFrequency(category, rulePackDef);
        var randChance = Rand.Value;
        Logger.MessageFormat(this, "calloutChance of {0} = {1}/{2}", rulePackDef, randChance, calloutChance);
        return randChance <= calloutChance;
    }

    private static float scaledCalloutFrequency(CalloutCategory category, RulePackDef rulePackDef,
        string keyword = "rule")
    {
        if (category != CalloutCategory.Combat)
        {
            return 1.0f;
        }

        var numberOfRules = rulePackDef.RulesPlusIncludes.Count(rule => rule.keyword == keyword);

        if (numberOfRules > maxRulesSeen)
        {
            maxRulesSeen = numberOfRules;
        }

        var scaledChance = (float)numberOfRules / maxRulesSeen;

        return scaledChance;
    }

    private static bool hasHediffOfStage(Pawn pawn, HediffAndStage hdas)
    {
        var hs = pawn.health.hediffSet;
        foreach (var hediff in hs.hediffs)
        {
            if (hediff.def == hdas.hediffDef && hediff.CurStageIndex == hdas.stage)
            {
                return true;
            }
        }

        return false;
    }

    public void RequestCallout(Pawn pawn, RulePackDef rulePack, GrammarRequest grammarRequest)
    {
        if (!CanCalloutNow(pawn))
        {
            return;
        }

        if (pawn.InMentalState)
        {
            grammarRequest.Constants["SPICY"] = "true";
        }

        if (pawn.story is { traits: not null })
        {
            foreach (var constantByTraitDef in DefDatabase<CalloutConstantByTraitDef>.AllDefs)
            {
                foreach (var traitDef in constantByTraitDef.traits)
                {
                    if (!pawn.story.traits.allTraits.Any(trait => trait.def == traitDef))
                    {
                        continue;
                    }

                    grammarRequest.Constants[constantByTraitDef.name] = constantByTraitDef.value;
                    break;
                }
            }
        }

        foreach (var calloutConstantByPawnkindDef in DefDatabase<CalloutConstantByPawnkindDef>.AllDefs)
        {
            if (calloutConstantByPawnkindDef.pawnKindDefs.Any(pkd => pkd.defName == pawn.kindDef.defName))
            {
                grammarRequest.Constants[calloutConstantByPawnkindDef.name] = calloutConstantByPawnkindDef.value;
            }
        }

        foreach (var calloutConstantByThingDef in DefDatabase<CalloutConstantByThingDef>.AllDefs)
        {
            if (calloutConstantByThingDef.thingDefs.Any(td => td.defName == pawn.def.defName))
            {
                grammarRequest.Constants[calloutConstantByThingDef.name] = calloutConstantByThingDef.value;
            }
        }

        if (pawn.health is { hediffSet: not null })
        {
            foreach (var calloutConstantByHediffDef in DefDatabase<CalloutConstantByHediffDef>.AllDefs)
            {
                if (calloutConstantByHediffDef.hediffDefs.Any(hd => pawn.health.hediffSet.HasHediff(hd)))
                {
                    grammarRequest.Constants[calloutConstantByHediffDef.name] = calloutConstantByHediffDef.value;
                }
            }

            foreach (var calloutConstantByHediffStage in DefDatabase<CalloutConstantByHediffStage>.AllDefs)
            {
                if (calloutConstantByHediffStage.hediffsAndStages.Any(hdas => hasHediffOfStage(pawn, hdas)))
                {
                    grammarRequest.Constants[calloutConstantByHediffStage.name] = calloutConstantByHediffStage.value;
                }
            }
        }

        if (pawn.needs != null)
        {
            foreach (var calloutConstantByNeedDef in DefDatabase<CalloutConstantByNeedDef>.AllDefs)
            {
                var need = pawn.needs.TryGetNeed(calloutConstantByNeedDef.needDef);
                if (need == null)
                {
                    continue;
                }

                var above = need.CurLevel > calloutConstantByNeedDef.needLevel;
                if (calloutConstantByNeedDef.aboveLevel ? above : !above)
                {
                    grammarRequest.Constants[calloutConstantByNeedDef.name] = calloutConstantByNeedDef.value;
                }
            }
        }

        var text = GrammarResolver.Resolve("rule", grammarRequest);
        if (!text.NullOrEmpty())
        {
            Logger.MessageFormat(this, "Callout resolved.");

            if (CalloutMod.settings.attachCalloutText)
            {
                createAttachedCalloutText(pawn, text, Color.white);
            }
            else
            {
                MoteMaker.ThrowText(pawn.DrawPos, pawn.MapHeld, text);
            }
        }
        else
        {
            Logger.WarningFormat(this, " Could not find text for requested {1} by {0}", pawn, rulePack);
        }

        // Mark tick when pawn can call out again
        var expireTick = Find.TickManager.TicksGame + ticksBetweenCallouts + hashCache;

        pawnCalloutExpireTick.Add(pawn, expireTick);
    }

    private static void createAttachedCalloutText(Thing caller, string text, Color color,
        float timeBeforeStartFadeout = -1f)
    {
        var location = caller.PositionHeld;
        var map = caller.MapHeld;

        if (!location.InBounds(map))
        {
            return;
        }

        var moteText = (MoteAttachedText)ThingMaker.MakeThing(CalloutDefOf.CM_Callouts_Thing_Mote_Attached_Text);

        moteText.text = text;
        moteText.textColor = color;
        if (timeBeforeStartFadeout >= 0f)
        {
            moteText.overrideTimeBeforeStartFadeout = timeBeforeStartFadeout;
        }

        GenSpawn.Spawn(moteText, location, map);
        moteText.Attach(caller);
    }

    public static void CreateWoundTextMote(Vector3 loc, Map map, string text, Color color,
        float timeBeforeStartFadeout = -1f)
    {
        var intVec = loc.ToIntVec3();
        if (!intVec.InBounds(map))
        {
            return;
        }

        var moteText = (MoteText)ThingMaker.MakeThing(CalloutDefOf.CM_Callouts_Thing_Mote_Text_Wound);
        moteText.exactPosition = loc;
        moteText.SetVelocity(Rand.Range(5, 35), Rand.Range(0.42f, 0.45f));
        moteText.text = text;
        moteText.textColor = color;
        if (timeBeforeStartFadeout >= 0f)
        {
            moteText.overrideTimeBeforeStartFadeout = timeBeforeStartFadeout;
        }

        ThrowText(moteText, intVec, map);
    }
}