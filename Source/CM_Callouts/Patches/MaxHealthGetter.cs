using System.Linq;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace CM_Callouts;

[HarmonyPatch]
public class MaxHealthGetter
{
    public static bool Prepare()
    {
        // detect whether the EBF is loaded
        return LoadedModManager.RunningMods.Any(pack => pack.PackageId == "V1024.EBFramework");
    }

    public static MethodBase TargetMethod()
    {
        // the correct EBF endpoint method to get the updated statistics
        return AccessTools.Method("EBF.EBFEndpoints:GetMaxHealthWithEBF");
    }

    [HarmonyReversePatch]
    public static float GetMaxHealth(BodyPartRecord record, Pawn pawn)
    {
        // if EBF is loaded, then Harmony replaces the body with the EBF endpoint method
        // else, the reverse-patch fails and the body remains the vanilla GetMaxHealth().
        return record.def.GetMaxHealth(pawn);
    }
}