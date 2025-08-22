using System.Collections.Generic;
using System.Linq;
using CM_Callouts.PendingCallouts;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace CM_Callouts;

[HarmonyPatch(typeof(DamageWorker.DamageResult), nameof(DamageWorker.DamageResult.AssociateWithLog))]
public static class DamageWorker_DamageResult_AssociateWithLog
{
    public static void Postfix(DamageWorker.DamageResult __instance)
    {
        if (__instance.deflected)
        {
            return;
        }

        if (__instance.hitThing is not Pawn hitPawn || __instance.parts.NullOrEmpty())
        {
            return;
        }

        var recipientPartsDamaged = __instance.parts.Distinct().ToList();
        var recipientPartsDestroyed = recipientPartsDamaged
            .Select(part => hitPawn.health.hediffSet.GetPartHealth(part) <= 0f).ToList();

        if (CalloutUtility.pendingCallout != null)
        {
            CalloutUtility.pendingCallout.FillBodyPartInfo(hitPawn.RaceProps.body, recipientPartsDamaged,
                recipientPartsDestroyed);
            CalloutUtility.pendingCallout.AttemptCallout();

            CalloutUtility.pendingCallout = null;
        }
        else if (CalloutUtility.CanCalloutNow(hitPawn))
        {
            new PendingCalloutEventWounded(hitPawn).AttemptCallout();
        }

        throwDestroyedPartMotes(hitPawn, recipientPartsDamaged, recipientPartsDestroyed);
    }

    private static void throwDestroyedPartMotes(Pawn pawn, List<BodyPartRecord> recipientPartsDamaged,
        List<bool> recipientPartsDestroyed)
    {
        var showWoundLevel = CalloutMod.settings.showWoundLevel;
        if (showWoundLevel == ShowWoundLevel.None || !pawn.SpawnedOrAnyParentSpawned)
        {
            return;
        }

        var thingVector3 = pawn.SpawnedParentOrMe.DrawPos;
        var thingMap = pawn.SpawnedParentOrMe.Map;

        for (var i = 0; i < recipientPartsDestroyed.Count; ++i)
        {
            if (recipientPartsDestroyed[i])
            {
                CalloutTracker.CreateWoundTextMote(thingVector3, thingMap, recipientPartsDamaged[i].def.label,
                    Color.magenta);
            }
            else
            {
                var partHealth = pawn.health.hediffSet.GetPartHealth(recipientPartsDamaged[i]);
                var partMaxHealth = EBFEndpoints_GetMaxHealthWithEBF.GetMaxHealth(recipientPartsDamaged[i], pawn);
                var partPercentHealth = partHealth / partMaxHealth;
                bool showWound;

                switch (partPercentHealth)
                {
                    case < 0.4f:
                        showWound = showWoundLevel >= ShowWoundLevel.Major;
                        break;
                    case < 0.7f:
                        showWound = showWoundLevel >= ShowWoundLevel.Serious;
                        break;
                    default:
                        showWound = showWoundLevel >= ShowWoundLevel.All;
                        break;
                }

                if (!showWound)
                {
                    continue;
                }

                var moteColor = HealthUtility.GetPartConditionLabel(pawn, recipientPartsDamaged[i]).Second;
                CalloutTracker.CreateWoundTextMote(thingVector3, thingMap, recipientPartsDamaged[i].def.label,
                    moteColor);
            }
        }
    }
}