using Verse;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts;

public class PendingCalloutEventSinglePawn(CalloutCategory category, Pawn initiator, RulePackDef initiatorRulePack)
    : PendingCalloutEvent(category)
{
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

        var calloutTracker = Current.Game.World.GetComponent<CalloutTracker>();
        if (calloutTracker == null)
        {
            return;
        }

        var initiatorCallout = Prefs.DevMode && CalloutMod.settings.forceInitiatorCallouts ||
                               calloutTracker.CheckCalloutChance(category, initiatorRulePack) &&
                               CalloutUtility.CanCalloutNow(initiator);

        if (initiatorCallout)
        {
            doInitiatorCallout(calloutTracker);
        }
    }

    private void doInitiatorCallout(CalloutTracker calloutTracker)
    {
        var grammarRequest = PrepareGrammarRequest(initiatorRulePack);
        calloutTracker.RequestCallout(initiator, initiatorRulePack, grammarRequest);
    }

    protected virtual GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
    {
        var grammarRequest = new GrammarRequest { Includes = { rulePack } };
        CalloutUtility.CollectPawnRules(initiator, "INITIATOR", ref grammarRequest);
        return grammarRequest;
    }
}