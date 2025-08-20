using System.Collections.Generic;
using CM_Callouts.PendingCallouts;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace CM_Callouts;

public static class CalloutUtility
{
    public static PendingCalloutEvent pendingCallout = null;

    public static bool CanCalloutNow(Thing thing)
    {
        var caller = thing as Pawn;

        var calloutTracker = Current.Game.World.GetComponent<CalloutTracker>();
        if (caller != null && calloutTracker != null)
        {
            return calloutTracker.CanCalloutNow(caller);
        }

        return false;
    }

    public static bool CanCalloutAtTarget(Thing target)
    {
        var targetPawn = target as Pawn;

        return targetPawn != null &&
               (CalloutMod.settings.allowCalloutsWhenTargetingAnimals || targetPawn.RaceProps.Humanlike);
    }

    public static void CollectPawnRules(Pawn pawn, string symbol, ref GrammarRequest grammarRequest)
    {
        grammarRequest.Rules.AddRange(GrammarUtility.RulesForPawn(symbol, pawn));
        grammarRequest.Constants.Add($"{symbol}_gender", pawn.gender.GetLabel());
        CollectWeaponRules(pawn, $"{symbol}_WEAPON", ref grammarRequest);
    }

    private static void CollectWeaponRules(Pawn pawn, string symbol, ref GrammarRequest grammarRequest)
    {
        if (pawn.equipment is { Primary: not null })
        {
            grammarRequest.Rules.AddRange(getRulesForWeapon(symbol, pawn.equipment.Primary));
        }
    }

    public static void CollectCoverRules(Pawn pawn, Pawn target, string symbol, Verb_LaunchProjectile verb,
        ref GrammarRequest grammarRequest)
    {
        var shotReport = ShotReport.HitReportFor(pawn, verb, target);
        var randomCoverToMissInto = shotReport.GetRandomCoverToMissInto();
        var targetCoverDef = randomCoverToMissInto?.def;

        if (targetCoverDef != null)
        {
            grammarRequest.Rules.AddRange(GrammarUtility.RulesForDef(symbol, targetCoverDef));
        }
    }

    public static void CollectHediffRules(Hediff hediff, string symbol, ref GrammarRequest grammarRequest)
    {
        grammarRequest.Rules.AddRange(GrammarUtility.RulesForHediffDef(symbol, hediff.def, hediff.Part));

        if (hediff.Part != null)
        {
            grammarRequest.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord($"{symbol}_target", hediff.Part));
        }

        if (hediff.CurStage != null)
        {
            grammarRequest.Constants[$"{symbol}_stage"] = hediff.CurStage.label;
        }
    }

    private static IEnumerable<Rule> getRulesForWeapon(string prefix, Thing thing)
    {
        ThingDef projectileDef = null;
        if (thing.def.Verbs is { Count: > 0 })
        {
            projectileDef = thing.def.Verbs[0].defaultProjectile;
        }

        foreach (var rule in PlayLogEntryUtility.RulesForOptionalWeapon(prefix, thing.def, projectileDef))
        {
            var defRule = (Rule_String)rule;
            yield return defRule;
        }

        //yield return new Rule_String(prefix + "_label", thing.def.label);
        //yield return new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(thing.def.label));
        //yield return new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(thing.def.label));
        if (thing.Stuff != null)
        {
            yield return new Rule_String($"{prefix}_stuffLabel", thing.Stuff.label);
        }

        var compArt = thing.TryGetComp<CompArt>();
        if (compArt is { Active: true })
        {
            yield return new Rule_String($"{prefix}_title", compArt.Title.ApplyTag(TagType.Name).Resolve());
        }

        var compQuality = thing.TryGetComp<CompQuality>();
        if (compQuality != null)
        {
            yield return new Rule_String($"{prefix}_quality", compQuality.Quality.GetLabel());
        }
    }

    public static void DrawText(Vector2 worldPos, string text, Color textColor)
    {
        var position = new Vector3(worldPos.x, 0f, worldPos.y);
        Vector2 vector = Find.Camera.WorldToScreenPoint(position) / Prefs.UIScale;
        vector.y = UI.screenHeight - vector.y;
        Text.Font = GameFont.Tiny;
        var x = Text.CalcSize(text).x;
        GUI.DrawTexture(new Rect(vector.x - (x / 2f) - 4f, vector.y, x + 8f, 12f), TexUI.GrayTextBG);
        GUI.color = textColor;
        Text.Anchor = TextAnchor.UpperCenter;
        Widgets.Label(new Rect(vector.x - (x / 2f), vector.y - 2f, x, 999f), text);
        GUI.color = Color.white;
        Text.Anchor = TextAnchor.UpperLeft;
    }
}