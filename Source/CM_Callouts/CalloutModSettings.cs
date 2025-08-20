using CM_Callouts.PendingCallouts;
using UnityEngine;
using Verse;

namespace CM_Callouts;

public class CalloutModSettings : ModSettings
{
    public bool allowCalloutsForAnimals;

    public bool allowCalloutsWhenTargetingAnimals;
    public bool attachCalloutText = true;
    public float baseCalloutChance = 0.15f;
    public bool drawLabelBackgroundForTextMotes = true;
    private bool enableCalloutsAnimal = true;
    private bool enableCalloutsCombat = true;
    public bool forceInitiatorCallouts;
    public bool forceRecipientCallouts;
    public bool hyperNuzzling;

    public bool queueTextMotes = true;


    // Debugging
    public bool showDebugLogMessages;
    public ShowWoundLevel showWoundLevel = ShowWoundLevel.Major;

    public override void ExposeData()
    {
        base.ExposeData();

        Scribe_Values.Look(ref enableCalloutsCombat, "enableCalloutsCombat", true);
        Scribe_Values.Look(ref enableCalloutsAnimal, "enableCalloutsAnimal", true);

        Scribe_Values.Look(ref queueTextMotes, "queueTextMotes", true);
        Scribe_Values.Look(ref attachCalloutText, "attachCalloutText", true);
        Scribe_Values.Look(ref drawLabelBackgroundForTextMotes, "drawLabelBackgroundForTextMotes", true);
        Scribe_Values.Look(ref baseCalloutChance, "baseCalloutChance", 0.15f);
        Scribe_Values.Look(ref showWoundLevel, "showWoundLevel", ShowWoundLevel.All);

        Scribe_Values.Look(ref allowCalloutsWhenTargetingAnimals, "allowCalloutsWhenTargetingAnimals");
        Scribe_Values.Look(ref allowCalloutsForAnimals, "allowCalloutsForAnimals");

        Scribe_Values.Look(ref showDebugLogMessages, "showDebugLogMessages");
        Scribe_Values.Look(ref hyperNuzzling, "hyperNuzzling");
        Scribe_Values.Look(ref forceInitiatorCallouts, "forceInitiatorCallouts");
        Scribe_Values.Look(ref forceRecipientCallouts, "forceRecipientCallouts");
    }

    public void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();

        listingStandard.Begin(inRect);

        listingStandard.CheckboxLabeled("CM_Callouts_Settings_Do_Callouts_Combat_Label".Translate(),
            ref enableCalloutsCombat, "CM_Callouts_Settings_Do_Callouts_Combat_Description".Translate());
        listingStandard.CheckboxLabeled("CM_Callouts_Settings_Do_Callouts_Animal_Label".Translate(),
            ref enableCalloutsAnimal, "CM_Callouts_Settings_Do_Callouts_Animal_Description".Translate());

        listingStandard.GapLine();

        listingStandard.CheckboxLabeled("CM_Callouts_Settings_Queue_Text_Motes_Label".Translate(), ref queueTextMotes,
            "CM_Callouts_Settings_Queue_Text_Motes_Description".Translate());
        listingStandard.CheckboxLabeled("CM_Callouts_Settings_Attach_Callout_Text_Label".Translate(),
            ref attachCalloutText, "CM_Callouts_Settings_Attach_Callout_Text_Description".Translate());
        listingStandard.CheckboxLabeled("CM_Callouts_Settings_Draw_Label_Background_For_Text_Motes_Label".Translate(),
            ref drawLabelBackgroundForTextMotes,
            "CM_Callouts_Settings_Draw_Label_Background_For_Text_Motes_Description".Translate());

        listingStandard.GapLine();

        listingStandard.Label("CM_Callouts_Settings_Show_Wound_Level_Label".Translate(), -1,
            "CM_Callouts_Settings_Show_Wound_Level_Description".Translate());
        if (listingStandard.RadioButton("CM_Callouts_Settings_Show_Wounds_None_Label".Translate(),
                showWoundLevel == ShowWoundLevel.None, 8f,
                "CM_Callouts_Settings_Show_Wounds_None_Description".Translate()))
        {
            showWoundLevel = ShowWoundLevel.None;
        }

        if (listingStandard.RadioButton("CM_Callouts_Settings_Show_Wounds_Destroyed_Label".Translate(),
                showWoundLevel == ShowWoundLevel.Destroyed, 8f,
                "CM_Callouts_Settings_Show_Wounds_Destroyed_Description".Translate()))
        {
            showWoundLevel = ShowWoundLevel.Destroyed;
        }

        if (listingStandard.RadioButton("CM_Callouts_Settings_Show_Wounds_Major_Label".Translate(),
                showWoundLevel == ShowWoundLevel.Major, 8f,
                "CM_Callouts_Settings_Show_Wounds_Major_Description".Translate()))
        {
            showWoundLevel = ShowWoundLevel.Major;
        }

        if (listingStandard.RadioButton("CM_Callouts_Settings_Show_Wounds_Serious_Label".Translate(),
                showWoundLevel == ShowWoundLevel.Serious, 8f,
                "CM_Callouts_Settings_Show_Wounds_Serious_Description".Translate()))
        {
            showWoundLevel = ShowWoundLevel.Serious;
        }

        if (listingStandard.RadioButton("CM_Callouts_Settings_Show_Wounds_All_Label".Translate(),
                showWoundLevel == ShowWoundLevel.All, 8f,
                "CM_Callouts_Settings_Show_Wounds_All_Description".Translate()))
        {
            showWoundLevel = ShowWoundLevel.All;
        }

        listingStandard.GapLine();

        listingStandard.Label("CM_Callouts_Settings_Base_Callout_Chance_Label".Translate(), -1,
            "CM_Callouts_Settings_Base_Callout_Chance_Description".Translate());
        listingStandard.Label(baseCalloutChance.ToString("P0"));
        baseCalloutChance = listingStandard.Slider(baseCalloutChance, 0.0f, 1.0f);

        listingStandard.CheckboxLabeled("CM_Callouts_Settings_Callout_When_Targeting_Animals_Label".Translate(),
            ref allowCalloutsWhenTargetingAnimals,
            "CM_Callouts_Settings_Callout_When_Targeting_Animals_Description".Translate());
        listingStandard.CheckboxLabeled("CM_Callouts_Settings_Animal_Callouts_Label".Translate(),
            ref allowCalloutsForAnimals, "CM_Callouts_Settings_Animal_Callouts_Description".Translate());

        if (Prefs.DevMode)
        {
            listingStandard.Label("Debug settings");
            listingStandard.CheckboxLabeled("showDebugLogMessages", ref showDebugLogMessages);
            listingStandard.CheckboxLabeled("hyperNuzzling", ref hyperNuzzling);
            listingStandard.CheckboxLabeled("forceInitiatorCallouts", ref forceInitiatorCallouts);
            listingStandard.CheckboxLabeled("forceRecipientCallouts", ref forceRecipientCallouts);
        }

        if (CalloutMod.currentVersion != null)
        {
            GUI.contentColor = Color.gray;
            listingStandard.Label("CM_Callouts_Settings_ModVersion".Translate(CalloutMod.currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }

    public static void UpdateSettings()
    {
    }

    public bool CalloutCategoryEnabled(CalloutCategory category)
    {
        switch (category)
        {
            case CalloutCategory.Combat:
                return enableCalloutsCombat;
            case CalloutCategory.Animal:
                return enableCalloutsAnimal;
            default:
                return true;
        }
    }
}