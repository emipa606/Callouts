using System.Collections.Generic;
using Verse;

namespace CM_Callouts;

public class CalloutConstantByThingDef : Def
{
    public readonly List<ThingDef> thingDefs = [];
    public string name;
    public string value;
}