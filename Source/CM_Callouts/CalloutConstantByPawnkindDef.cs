using System.Collections.Generic;
using Verse;

namespace CM_Callouts;

public class CalloutConstantByPawnkindDef : Def
{
    public readonly List<PawnKindDef> pawnKindDefs = [];
    public string name;
    public string value;
}