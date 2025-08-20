using CM_Callouts.PendingCallouts;
using HarmonyLib;
using RimWorld;

namespace CM_Callouts;

[HarmonyPatch(typeof(Pawn_DraftController), nameof(Pawn_DraftController.Drafted), MethodType.Setter)]
public static class Pawn_DraftController_Drafted
{
    public static void Prefix(bool value, bool ___draftedInt, out bool __state)
    {
        __state = value && !___draftedInt;
    }

    public static void Postfix(Pawn_DraftController __instance, bool ___draftedInt, bool __state)
    {
        if (___draftedInt && __state)
        {
            new PendingCalloutEventSinglePawn(CalloutCategory.Undefined, __instance.pawn,
                CalloutDefOf.CM_Callouts_RulePack_Drafted).AttemptCallout();
        }
    }
}