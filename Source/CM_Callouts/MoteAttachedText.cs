using UnityEngine;
using Verse;

namespace CM_Callouts;

public class MoteAttachedText : MoteAttached
{
    public float overrideTimeBeforeStartFadeout = -1f;
    public string text;

    public Color textColor = Color.white;

    private float TimeBeforeStartFadeout =>
        !(overrideTimeBeforeStartFadeout >= 0f) ? SolidTime : overrideTimeBeforeStartFadeout;

    protected override bool EndOfLife => AgeSecs >= TimeBeforeStartFadeout + def.mote.fadeOutTime;

    protected override void DrawAt(Vector3 drawLoc, bool flip = false)
    {
    }

    public override void DrawGUIOverlay()
    {
        var a = 1f - ((AgeSecs - TimeBeforeStartFadeout) / def.mote.fadeOutTime);
        CalloutUtility.DrawText(new Vector2(exactPosition.x, exactPosition.z), text,
            new Color(textColor.r, textColor.g, textColor.b, a));
    }
}