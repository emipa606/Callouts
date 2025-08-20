using Verse;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts;

public class PendingCalloutEventWounded(Pawn initiator) : PendingCalloutEventSinglePawn(CalloutCategory.Combat,
    initiator, CalloutDefOf.CM_Callouts_RulePack_Wounded)
{
    protected override GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
    {
        var grammarRequest = base.PrepareGrammarRequest(rulePack);
        grammarRequest.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("PART", body, bodyPartsDamaged,
            bodyPartsDestroyed, grammarRequest.Constants));
        return grammarRequest;
    }
}