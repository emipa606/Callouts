using CM_Callouts.PendingCallouts;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace CM_Callouts;

[HarmonyPatch(typeof(HealthUtility), nameof(HealthUtility.AdjustSeverity))]
public static class HealthUtility_AdjustSeverity
{
    private static Hediff initialFoundHediff;
    private static int startingStageIndex = -1;

    public static void Prefix(Pawn pawn, HediffDef hdDef, float sevOffset)
    {
        if (sevOffset != 0f && pawn != null && hdDef?.lethalSeverity > 0.0f)
        {
            initialFoundHediff = pawn.health.hediffSet.GetFirstHediffOfDef(hdDef);
            if (initialFoundHediff != null)
            {
                startingStageIndex = initialFoundHediff.CurStageIndex;
            }
        }
        else
        {
            initialFoundHediff = null;
            startingStageIndex = -1;
        }
    }

    public static void Postfix(Pawn pawn, HediffDef hdDef, float sevOffset)
    {
        if (sevOffset == 0f || pawn == null || hdDef?.lethalSeverity <= 0.0f)
        {
            // Resetting even though it seems unnecessary, in case another call somehow circumvented the prefix (probably not possible :P)
            initialFoundHediff = null;
            startingStageIndex = -1;
            return;
        }

        var foundHediff = pawn.health.hediffSet.GetFirstHediffOfDef(hdDef);

        if (foundHediff != null)
        {
            var currentStageIndex = foundHediff.CurStageIndex;

            if (initialFoundHediff == null || currentStageIndex > startingStageIndex)
            {
                var currentStage = foundHediff.CurStage;
                var showWoundLevel = CalloutMod.settings.showWoundLevel;

                // If the pawn is spawned and the current stage should be visible
                if (pawn.SpawnedOrAnyParentSpawned && currentStage != null && currentStage.becomeVisible)
                {
                    // Do a callout
                    new PendingCalloutEventLethalHediffProgression(pawn, foundHediff).AttemptCallout();

                    var stagesAwayFromDeath = foundHediff.def.stages.Count - currentStageIndex;
                    // Throw text if wound is serious enough according to our settings
                    if (showWoundLevel != ShowWoundLevel.None && (showWoundLevel == ShowWoundLevel.All ||
                                                                  stagesAwayFromDeath <= (int)showWoundLevel))
                    {
                        var thingVector3 = pawn.SpawnedParentOrMe.DrawPos;
                        var thingMap = pawn.SpawnedParentOrMe.Map;

                        var moteColor = HealthUtility.SlightlyImpairedColor;
                        switch (stagesAwayFromDeath)
                        {
                            case 1:
                                moteColor = Color.magenta;
                                break;
                            case 2:
                                moteColor = HealthUtility.RedColor;
                                break;
                            case 3:
                                moteColor = HealthUtility.ImpairedColor;
                                break;
                        }

                        CalloutTracker.CreateWoundTextMote(thingVector3, thingMap, foundHediff.Label, moteColor);
                    }
                }
            }
        }

        initialFoundHediff = null;
        startingStageIndex = -1;
    }
}