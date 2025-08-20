using CM_Callouts.PendingCallouts.Combat;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CM_Callouts;

[HarmonyPatch(typeof(Verb), nameof(Verb.TryStartCastOn), typeof(LocalTargetInfo), typeof(LocalTargetInfo), typeof(bool),
    typeof(bool), typeof(bool), typeof(bool))]
public static class Verb_TryStartCastOn
{
    public static void Postfix(Verb __instance)
    {
        if (__instance is not Verb_MeleeAttack || __instance.CasterPawn == null)
        {
            return;
        }

        if (CalloutUtility.CanCalloutNow(__instance.CasterPawn) &&
            CalloutUtility.CanCalloutAtTarget(__instance.CurrentTarget.Thing))
        {
            new PendingCalloutEventMeleeAttempt(__instance.CasterPawn, __instance.CurrentTarget.Thing as Pawn,
                __instance).AttemptCallout();
        }
    }
}