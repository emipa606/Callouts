using System;
using CM_Callouts.PendingCallouts.Combat;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CM_Callouts;

[HarmonyPatch(typeof(Verb_MeleeAttack), nameof(Verb_MeleeAttack.CreateCombatLog))]
public static class Verb_MeleeAttack_CreateCombatLog
{
    public static void Postfix(Verb_MeleeAttack __instance, Func<ManeuverDef, RulePackDef> rulePackGetter)
    {
        CalloutUtility.pendingCallout = null;

        if (__instance.maneuver == null || __instance.tool == null)
        {
            return;
        }

        if (__instance.CurrentTarget.Thing is not Pawn pawn || !CalloutUtility.CanCalloutNow(__instance.CasterPawn) ||
            !CalloutUtility.CanCalloutAtTarget(pawn))
        {
            return;
        }

        // Ignore dodge for now since it already throws a text mote
        if (rulePackGetter(__instance.maneuver) == __instance.maneuver.combatLogRulesDodge)
        {
            return;
        }

        // If logging a hit, do the callout when we resolve the damage result
        if (rulePackGetter(__instance.maneuver) == __instance.maneuver.combatLogRulesHit)
        {
            // This will get resolved in a DamageWorker.DamageResult.AssociateWithLog patch so we will know the result and don't have to transpile
            CalloutUtility.pendingCallout = new PendingCalloutEventMeleeImpact(__instance.CasterPawn,
                pawn);
        }
        else if (rulePackGetter(__instance.maneuver) == __instance.maneuver.combatLogRulesMiss)
        {
            // TODO: Melee miss callout
        }
    }
}