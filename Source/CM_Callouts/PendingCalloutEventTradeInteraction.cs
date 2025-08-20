using CM_Callouts.PendingCallouts;
using Verse;
using Verse.Grammar;

namespace CM_Callouts;

public class PendingCalloutEventTradeInteraction(
    Pawn initiator,
    Pawn recipient,
    RulePackDef initiatorRulePack,
    RulePackDef recipientRulePack)
    : PendingCalloutEventDoublePawn(CalloutCategory.Undefined, initiator, recipient, initiatorRulePack,
        recipientRulePack)
{
    protected override GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
    {
        var result = base.PrepareGrammarRequest(rulePack);
        result.Constants.Add("RECIPIENT_TraderKindDef", recipient.TraderKind.defName);
        return result;
    }
}