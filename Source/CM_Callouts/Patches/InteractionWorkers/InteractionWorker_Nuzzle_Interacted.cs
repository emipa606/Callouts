using CM_Callouts.PendingCallouts.Interaction;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CM_Callouts.Patches.InteractionWorkers;

[HarmonyPatch(typeof(InteractionWorker_Nuzzle), nameof(InteractionWorker_Nuzzle.Interacted))]
public static class InteractionWorker_Nuzzle_Interacted
{
    public static void Postfix(Pawn initiator, Pawn recipient)
    {
        if (recipient.needs.mood == null)
        {
            return;
        }

        // Recipient and initiator are deliberately reversed here so that the nuzzle callout can share logic with other animal interaction
        new PendingCalloutEventAnimalInteraction(recipient, initiator,
            CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Nuzzle_Initiated,
            CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Nuzzle_Received).AttemptCallout();
    }
}