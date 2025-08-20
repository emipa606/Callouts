using HarmonyLib;
using Verse;
using Verse.AI;

namespace CM_Callouts.Patches;

[HarmonyPatch(typeof(ThinkNode_ChancePerHour_Nuzzle), "MtbHours")]
public static class ThinkNode_ChancePerHour_Nuzzle_MtbHours
{
    public static void Postfix(ref float __result)
    {
        if (Prefs.DevMode && CalloutMod.settings.hyperNuzzling)
        {
            __result = 0.01f;
        }
    }
}