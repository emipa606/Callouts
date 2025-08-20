using CM_Callouts.PendingCallouts.Interaction;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CM_Callouts.Patches;

[HarmonyPatch(typeof(Pawn_TrainingTracker), nameof(Pawn_TrainingTracker.Train))]
public static class Pawn_TrainingTracker_Train
{
    public static void Postfix(TrainableDef td, Pawn trainer, bool complete, Pawn ___pawn)
    {
        if (td == TrainableDefOf.Tameness && complete)
        {
            return;
        }

        new PendingCalloutEventAnimalInteraction(trainer, ___pawn,
            CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Train_Success_Initiated,
            CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Train_Success_Received).AttemptCallout();
    }
}