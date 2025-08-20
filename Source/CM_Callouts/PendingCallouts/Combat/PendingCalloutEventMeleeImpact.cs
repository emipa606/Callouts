using Verse;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts.Combat;

public class PendingCalloutEventMeleeImpact(Pawn initiator, Pawn recipient) : PendingCalloutEventDoublePawn(
    CalloutCategory.Combat, initiator, recipient, CalloutDefOf.CM_Callouts_RulePack_Melee_Attack_Landed,
    CalloutDefOf.CM_Callouts_RulePack_Melee_Attack_Received)
{
    protected override GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
    {
        var grammarRequest = base.PrepareGrammarRequest(rulePack);
        grammarRequest.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("PART", body, bodyPartsDamaged,
            bodyPartsDestroyed, grammarRequest.Constants));
        return grammarRequest;
    }
}