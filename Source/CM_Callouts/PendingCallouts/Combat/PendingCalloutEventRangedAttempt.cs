using RimWorld;
using Verse;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts.Combat;

public class PendingCalloutEventRangedAttempt(Pawn initiator, Pawn recipient, Verb_LaunchProjectile verb)
    : PendingCalloutEventDoublePawn(CalloutCategory.Combat, initiator, recipient,
        CalloutDefOf.CM_Callouts_RulePack_Ranged_Attack, null)
{
    protected override GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
    {
        var grammarRequest = base.PrepareGrammarRequest(rulePack);

        if (recipient == null)
        {
            return grammarRequest;
        }

        if (!StatDefOf.ShootingAccuracyPawn.Worker.IsDisabledFor(recipient))
        {
            CalloutUtility.CollectCoverRules(recipient, initiator, "INITIATOR_COVER", verb, ref grammarRequest);
        }

        CalloutUtility.CollectCoverRules(initiator, recipient, "RECIPIENT_COVER", verb, ref grammarRequest);

        return grammarRequest;
    }
}