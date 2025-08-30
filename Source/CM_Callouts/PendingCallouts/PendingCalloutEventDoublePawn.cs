using Verse;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts;

public class PendingCalloutEventDoublePawn(
    CalloutCategory category,
    Pawn initiator,
    Pawn recipient,
    RulePackDef initiatorRulePack,
    RulePackDef recipientRulePack)
    : PendingCalloutEvent(category)
{
    protected readonly Pawn initiator = initiator;

    protected readonly Pawn recipient = recipient;

    public override void AttemptCallout()
    {
        if (!CalloutMod.settings.CalloutCategoryEnabled(category))
        {
            return;
        }

        base.AttemptCallout();

        // Some of the functions leading here might be called when loading the game
        if (Scribe.mode != LoadSaveMode.Inactive)
        {
            return;
        }

        if (initiator == null)
        {
            Logger.WarningFormat(this, "Initiator null");
            return;
        }

        if (recipient == null)
        {
            Logger.WarningFormat(this, "Recipient null");
            return;
        }

        var calloutTracker = Current.Game.World.GetComponent<CalloutTracker>();
        if (calloutTracker == null)
        {
            return;
        }

        var hasInitiatorCallout = initiatorRulePack != null;
        var hasRecipientCallout = recipientRulePack != null;

        var initiatorCalloutForced =
            hasInitiatorCallout && Prefs.DevMode && CalloutMod.settings.forceInitiatorCallouts;
        var recipientCalloutForced =
            hasRecipientCallout && Prefs.DevMode && CalloutMod.settings.forceRecipientCallouts;

        var initiatorCallout = hasInitiatorCallout && (initiatorCalloutForced ||
                                                       (!hasRecipientCallout || Rand.Bool) &&
                                                       calloutTracker.CheckCalloutChance(category,
                                                           initiatorRulePack) &&
                                                       CalloutUtility.CanCalloutNow(initiator) &&
                                                       !recipientCalloutForced);
        var recipientCallout = hasRecipientCallout && (recipientCalloutForced ||
                                                       (!hasInitiatorCallout || Rand.Bool) &&
                                                       calloutTracker.CheckCalloutChance(category,
                                                           recipientRulePack) &&
                                                       CalloutUtility.CanCalloutNow(initiator));

        if (initiatorCallout)
        {
            doInitiatorCallout(calloutTracker);
        }
        else if (recipientCallout)
        {
            doRecipientCallout(calloutTracker);
        }
    }

    private void doInitiatorCallout(CalloutTracker calloutTracker)
    {
        var grammarRequest = PrepareGrammarRequest(initiatorRulePack);
        calloutTracker.RequestCallout(initiator, initiatorRulePack, grammarRequest);
    }

    private void doRecipientCallout(CalloutTracker calloutTracker)
    {
        var grammarRequest = PrepareGrammarRequest(recipientRulePack);
        calloutTracker.RequestCallout(recipient, recipientRulePack, grammarRequest);
    }

    protected virtual GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
    {
        var grammarRequest = new GrammarRequest { Includes = { rulePack } };
        if (CalloutMod.settings.noSwearing)
        {
            grammarRequest.Constants.Add("MILD", "true");
            grammarRequest.Constants.Add("SPICY", "false");
        }

        CalloutUtility.CollectPawnRules(initiator, "INITIATOR", ref grammarRequest);

        if (recipient != null)
        {
            CalloutUtility.CollectPawnRules(recipient, "RECIPIENT", ref grammarRequest);
        }

        return grammarRequest;
    }
}