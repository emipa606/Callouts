using CM_Callouts.PendingCallouts.Combat;
using HarmonyLib;
using Verse;

namespace CM_Callouts;

[HarmonyPatch(typeof(BattleLogEntry_RangedImpact), MethodType.Constructor, typeof(Thing), typeof(Thing), typeof(Thing),
    typeof(ThingDef), typeof(ThingDef), typeof(ThingDef))]
public static class BattleLogEntry_RangedImpact_Constructor
{
    public static void Postfix(Thing initiator, Thing recipient,
        Thing originalTarget, ThingDef weaponDef, ThingDef projectileDef, ThingDef coverDef)
    {
        CalloutUtility.pendingCallout = null;

        if (initiator is not Pawn pawn)
        {
            return;
        }


        if (recipient != originalTarget)
        {
            return;
        }

        if (recipient is Pawn target && CalloutUtility.CanCalloutNow(pawn) &&
            CalloutUtility.CanCalloutAtTarget(target))
        {
            CalloutUtility.pendingCallout = new PendingCalloutEventRangedImpact(pawn,
                target, originalTarget as Pawn, weaponDef, projectileDef, coverDef);
        }
    }
}