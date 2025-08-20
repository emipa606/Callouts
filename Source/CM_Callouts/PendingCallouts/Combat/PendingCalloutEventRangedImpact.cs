using Verse;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts.Combat;

public class PendingCalloutEventRangedImpact(
    Pawn initiator,
    Pawn recipient,
    Pawn originalTarget,
    ThingDef weaponDef,
    ThingDef projectileDef,
    ThingDef coverDef)
    : PendingCalloutEventDoublePawn(CalloutCategory.Combat, initiator, recipient,
        CalloutDefOf.CM_Callouts_RulePack_Ranged_Attack_Landed_OriginalTarget,
        CalloutDefOf.CM_Callouts_RulePack_Ranged_Attack_Received_OriginalTarget)
{
    public Pawn originalTarget = originalTarget;
    public ThingDef projectileDef = projectileDef;
    public ThingDef weaponDef = weaponDef;

    protected override GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
    {
        var grammarRequest = base.PrepareGrammarRequest(rulePack);

        if (recipient == null)
        {
            return grammarRequest;
        }

        grammarRequest.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("PART", body, bodyPartsDamaged,
            bodyPartsDestroyed, grammarRequest.Constants));

        if (coverDef != null)
        {
            grammarRequest.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT_COVER", coverDef));
        }

        return grammarRequest;
    }
}