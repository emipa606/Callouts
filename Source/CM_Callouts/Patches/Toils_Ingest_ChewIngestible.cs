using CM_Callouts.PendingCallouts.Interaction;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CM_Callouts.Patches;

[HarmonyPatch(typeof(Toils_Ingest), nameof(Toils_Ingest.ChewIngestible))]
public static class Toils_Ingest_ChewIngestible
{
    public static void Postfix(Pawn chewer, TargetIndex ingestibleInd, Toil __result)
    {
        if (__result == null)
        {
            return;
        }

        if (chewer == null || !chewer.AnimalOrWildMan())
        {
            return;
        }

        __result.AddPreInitAction(delegate
        {
            var initiator = __result.GetActor();
            if (initiator == chewer)
            {
                return;
            }

            var food = initiator.CurJob.GetTarget(ingestibleInd).Thing;
            if (food is not { IngestibleNow: true })
            {
                return;
            }

            new PendingCalloutEventAnimalInteractionWithFood(initiator, chewer, food.def,
                CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Feed_Initiated,
                CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Feed_Received).AttemptCallout();
        });
    }
}