using CM_Callouts.PendingCallouts.Interaction;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CM_Callouts.Patches.InteractionWorkers;

[HarmonyPatch(typeof(InteractionWorker_RecruitAttempt), nameof(InteractionWorker_RecruitAttempt.DoRecruit),
    [typeof(Pawn), typeof(Pawn), typeof(string), typeof(string), typeof(bool), typeof(bool)],
    [
        ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Out, ArgumentType.Normal,
        ArgumentType.Normal
    ])]
public static class InteractionWorker_RecruitAttempt_DoRecruit
{
    public static void Postfix(Pawn recruiter, Pawn recruitee)
    {
        if (!recruitee.AnimalOrWildMan())
        {
            return;
        }

        new PendingCalloutEventAnimalInteraction(recruiter, recruitee,
            CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Tame_Success_Initiated,
            CalloutDefOf.CM_Callouts_RulePack_Interaction_Animal_Tame_Success_Received).AttemptCallout();
    }
}