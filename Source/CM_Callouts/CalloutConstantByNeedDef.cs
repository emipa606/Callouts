using RimWorld;
using Verse;

namespace CM_Callouts;

public class CalloutConstantByNeedDef : Def
{
    public bool aboveLevel;
    public string name;
    public NeedDef needDef;
    public float needLevel;
    public string value;
}