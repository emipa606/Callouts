using System.Collections.Generic;
using Verse;

namespace CM_Callouts;

public class CalloutConstantByHediffDef : Def
{
    public readonly List<HediffDef> hediffDefs = [];
    public string name;
    public string value;
}