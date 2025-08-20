using Verse;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts.Combat;

public class PendingCalloutEventMeleeAttempt(Pawn initiator, Pawn recipient, Verb verb)
    : PendingCalloutEventDoublePawn(CalloutCategory.Combat, initiator, recipient,
        CalloutDefOf.CM_Callouts_RulePack_Melee_Attack, null)
{
    protected override GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
    {
        var grammarRequest = base.PrepareGrammarRequest(rulePack);

        if (verb is { maneuver.combatLogRulesHit: not null })
        {
            grammarRequest.Includes.Add(verb.maneuver.combatLogRulesHit);
        }

        return grammarRequest;
    }
}