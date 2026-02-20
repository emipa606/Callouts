using System.Reflection;
using HarmonyLib;
using Verse;

namespace CM_Callouts;

[HarmonyPatch]
public class EBFEndpoints_GetMaxHealthWithEBF
{
    public static bool Prepare()
    {
        // detect whether the EBF is loaded
        return ModLister.GetActiveModWithIdentifier("v1024.ebframework", true) != null;
    }

    public static MethodBase TargetMethod()
    {
        // the correct EBF endpoint method to get the updated statistics
        return AccessTools.Method("EBF.EBFEndpoints:GetMaxHealthWithEBF");
    }

    [HarmonyReversePatch]
    public static float GetMaxHealth(BodyPartRecord record, Pawn pawn, bool _ = true)
    {
        // if EBF is loaded, then Harmony replaces the body with the EBF endpoint method
        // else, the reverse-patch fails and the body remains the vanilla GetMaxHealth().
        // note: the discard parameter is for EBF having an optional "useCache" parameter.
        return record.def.GetMaxHealth(pawn);
    }
}