using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CM_Callouts;

[HarmonyPatch(typeof(JobDriver_TradeWithPawn), "MakeNewToils")]
public static class TradeWithPawn_CalloutsExpanded_Patch
{
    public static IEnumerable<Toil> Postfix(IEnumerable<Toil> values, JobDriver_TradeWithPawn __instance)
    {
        var toils = values.ToList();
        int i;
        for (i = 0; i < toils.Count; i++)
        {
            if (i == toils.Count - 1)
            {
                var toil = toils[i];
                var action = toil.initAction;
                toil.initAction = delegate
                {
                    action();
                    var pawn = __instance.GetType()
                        .GetProperty("Trader", BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.GetValue(__instance) as Pawn;
                    if (toil.actor == null || pawn == null)
                    {
                        return;
                    }

                    new PendingCalloutEventTradeInteraction(toil.actor, pawn,
                        CalloutsExpandedDefOf.Callouts_RulePack_Trade_Initiated,
                        CalloutsExpandedDefOf.Callouts_RulePack_Trade_Received).AttemptCallout();
                };
            }

            yield return toils[i];
        }
    }
}