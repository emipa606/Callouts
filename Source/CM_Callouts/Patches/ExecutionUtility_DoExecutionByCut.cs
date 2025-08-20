using CM_Callouts.PendingCallouts.Interaction;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CM_Callouts.Patches;

[HarmonyPatch(typeof(ExecutionUtility), nameof(ExecutionUtility.DoExecutionByCut))]
public static class ExecutionUtility_DoExecutionByCut
{
    public static void Prefix(Pawn executioner, Pawn victim)
    {
        if (!victim.AnimalOrWildMan())
        {
            return;
        }

        new PendingCalloutEventAnimalInteraction(executioner, victim,
            CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Slaughter_Initiated,
            CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Slaughter_Received).AttemptCallout();
    }
}