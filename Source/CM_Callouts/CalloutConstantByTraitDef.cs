using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CM_Callouts;

public class CalloutConstantByTraitDef : Def
{
    public readonly List<TraitDef> traits = [];
    public string name;
    public string value;
}