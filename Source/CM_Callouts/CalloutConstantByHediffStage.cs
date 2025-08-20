using System.Collections.Generic;
using Verse;

namespace CM_Callouts;

public class CalloutConstantByHediffStage : Def
{
    public readonly List<HediffAndStage> hediffsAndStages = [];
    public string name;
    public string value;
}