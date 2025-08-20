using RimWorld;
using Verse;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts.Interaction;

public class PendingCalloutEventAnimalInteraction(
    Pawn initiator,
    Pawn recipient,
    RulePackDef initiatorRulePack,
    RulePackDef recipientRulePack)
    : PendingCalloutEventDoublePawn(CalloutCategory.Animal, initiator, recipient, initiatorRulePack, recipientRulePack)
{
    protected override GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
    {
        var grammarRequest = base.PrepareGrammarRequest(rulePack);

        if (recipient.relations != null && recipient.relations.DirectRelationExists(PawnRelationDefOf.Bond, initiator))
        {
            grammarRequest.Constants.Add("BONDED", "true");
        }

        return grammarRequest;
    }
}