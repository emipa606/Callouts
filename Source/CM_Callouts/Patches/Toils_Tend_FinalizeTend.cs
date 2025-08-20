using CM_Callouts.PendingCallouts.Interaction;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CM_Callouts.Patches;

[HarmonyPatch(typeof(Toils_Tend), nameof(Toils_Tend.FinalizeTend))]
public static class Toils_Tend_FinalizeTend
{
    public static void Postfix(Pawn patient, Toil __result)
    {
        if (__result == null)
        {
            return;
        }

        if (patient == null || !patient.AnimalOrWildMan())
        {
            return;
        }

        __result.AddPreInitAction(delegate
        {
            var initiator = __result.GetActor();
            new PendingCalloutEventAnimalInteraction(initiator, patient,
                CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Tend_Initiated,
                CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Tend_Received).AttemptCallout();
        });
    }
}