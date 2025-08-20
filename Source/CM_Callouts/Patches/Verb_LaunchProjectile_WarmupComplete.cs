using CM_Callouts.PendingCallouts.Combat;
using HarmonyLib;
using Verse;

namespace CM_Callouts;

[HarmonyPatch(typeof(Verb_LaunchProjectile), nameof(Verb_LaunchProjectile.WarmupComplete))]
public static class Verb_LaunchProjectile_WarmupComplete
{
    public static void Postfix(Verb_LaunchProjectile __instance)
    {
        if (__instance.CasterPawn == null)
        {
            return;
        }

        if (CalloutUtility.CanCalloutNow(__instance.CasterPawn) &&
            CalloutUtility.CanCalloutAtTarget(__instance.CurrentTarget.Thing))
        {
            new PendingCalloutEventRangedAttempt(__instance.CasterPawn, __instance.CurrentTarget.Pawn, __instance)
                .AttemptCallout();
        }
    }
}