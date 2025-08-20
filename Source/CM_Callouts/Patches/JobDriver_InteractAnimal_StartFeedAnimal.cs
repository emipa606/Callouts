using System;
using System.Reflection;
using CM_Callouts.PendingCallouts.Interaction;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CM_Callouts.Patches;

[HarmonyPatch(typeof(JobDriver_InteractAnimal), "StartFeedAnimal")]
public static class JobDriver_InteractAnimal_StartFeedAnimal
{
    public static void Postfix(JobDriver_InteractAnimal __instance, TargetIndex tameeInd, Toil __result)
    {
        __result?.AddPreInitAction(delegate
        {
            try
            {
                //FoodUtility.BestFoodInInventory();
                var initiator = __instance.GetActor();
                var recipient = initiator.CurJob.GetTarget(tameeInd).Pawn;
                var methodInfo = typeof(FoodUtility).GetMethod("BestFoodInInventory",
                    BindingFlags.Public | BindingFlags.Static);
                Thing thing;
                if (methodInfo != null && methodInfo.GetParameters().Length == 6)
                {
                    thing = (Thing)methodInfo.Invoke(null,
                    [
                        initiator, recipient, FoodPreferability.NeverForNutrition, FoodPreferability.RawTasty,
                        0f, false
                    ]);
                }
                else //ugly harmony hack for unstable/normal branch compat for 1.4, 16.11.2022
                {
                    thing = (Thing)methodInfo?.Invoke(null,
                    [
                        initiator, recipient, FoodPreferability.NeverForNutrition, FoodPreferability.RawTasty,
                        0f, false, false
                    ]);
                }

                if (thing != null)
                {
                    new PendingCalloutEventAnimalInteractionWithFood(initiator, recipient, thing.def,
                        CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Feed_Initiated,
                        CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Feed_Received).AttemptCallout();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception in Callouts: {e}");
            }
        });
    }
}