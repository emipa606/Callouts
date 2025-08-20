using HarmonyLib;
using UnityEngine;
using Verse;

namespace CM_Callouts;

[HarmonyPatch(typeof(MoteText), nameof(MoteText.DrawGUIOverlay))]
public static class MoteText_DrawGUIOverlay
{
    public static bool Prefix(MoteText __instance)
    {
        if (!CalloutMod.settings.drawLabelBackgroundForTextMotes)
        {
            return true;
        }

        var timeBeforeStartFadeout = !(__instance.overrideTimeBeforeStartFadeout >= 0f)
            ? __instance.def.mote.solidTime
            : __instance.overrideTimeBeforeStartFadeout;

        var a = 1f - ((__instance.AgeSecs - timeBeforeStartFadeout) / __instance.def.mote.fadeOutTime);
        CalloutUtility.DrawText(
            textColor: new Color(__instance.textColor.r, __instance.textColor.g, __instance.textColor.b, a),
            worldPos: new Vector2(__instance.exactPosition.x, __instance.exactPosition.z),
            text: __instance.text);
        return false;
    }
}