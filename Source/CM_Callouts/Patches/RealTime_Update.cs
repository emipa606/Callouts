using HarmonyLib;
using Verse;

namespace CM_Callouts;

[HarmonyPatch(typeof(RealTime), nameof(RealTime.Update))]
public static class RealTime_Update
{
    public static void Postfix()
    {
        CalloutTracker.UpdateTextMoteQueuesRealTime();
    }
}