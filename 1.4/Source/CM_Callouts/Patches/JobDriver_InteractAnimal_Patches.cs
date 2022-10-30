using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

using CM_Callouts.PendingCallouts.Interaction;

namespace CM_Callouts.Patches
{
    [StaticConstructorOnStartup]
    public static class JobDriver_InteractAnimal_Patches
    {
        [HarmonyPatch(typeof(JobDriver_InteractAnimal))]
        [HarmonyPatch("StartFeedAnimal", MethodType.Normal)]
        public static class JobDriver_InteractAnimal_StartFeedAnimal
        {
            [HarmonyPostfix]
            public static void Postfix(JobDriver_InteractAnimal __instance, TargetIndex tameeInd, Toil __result)
            {
                if (__result == null)
                    return;

                __result.AddPreInitAction(delegate
                {
                    Pawn initiator = __instance.GetActor();
                    Pawn recipient = initiator.CurJob.GetTarget(tameeInd).Pawn;
                    Thing thing = FoodUtility.BestFoodInInventory(initiator, recipient, FoodPreferability.NeverForNutrition, FoodPreferability.RawTasty);

                    if (thing != null)
                    {
                        new PendingCalloutEventAnimalInteractionWithFood(initiator, recipient, thing.def, CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Feed_Initiated, CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Feed_Received).AttemptCallout();
                    }
                });
            }
        }
    }
}