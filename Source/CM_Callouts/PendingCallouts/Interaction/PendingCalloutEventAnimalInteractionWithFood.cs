using Verse;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts.Interaction;

public class PendingCalloutEventAnimalInteractionWithFood(
    Pawn initiator,
    Pawn recipient,
    ThingDef food,
    RulePackDef initiatorRulePack,
    RulePackDef recipientRulePack)
    : PendingCalloutEventAnimalInteraction(initiator, recipient, initiatorRulePack, recipientRulePack)
{
    protected override GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
    {
        var grammarRequest = base.PrepareGrammarRequest(rulePack);
        grammarRequest.Rules.AddRange(GrammarUtility.RulesForDef("FOOD", food));
        return grammarRequest;
    }
}