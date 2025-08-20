using Verse;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts;

public class PendingCalloutEventLethalHediffProgression(Pawn initiator, Hediff hediff)
    : PendingCalloutEventSinglePawn(CalloutCategory.Undefined, initiator,
        CalloutDefOf.CM_Callouts_RulePack_Lethal_Hediff_Progression)
{
    protected override GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
    {
        var grammarRequest = base.PrepareGrammarRequest(rulePack);
        CalloutUtility.CollectHediffRules(hediff, "CULPRITHEDIFF", ref grammarRequest);
        return grammarRequest;
    }
}