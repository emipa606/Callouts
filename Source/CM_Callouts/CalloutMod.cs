using System.Reflection;
using HarmonyLib;
using Mlie;
using UnityEngine;
using Verse;

namespace CM_Callouts;

public class CalloutMod : Mod
{
    public static CalloutModSettings settings;
    public static string currentVersion;

    public CalloutMod(ModContentPack content) : base(content)
    {
        new Harmony("CM_Callouts").PatchAll(Assembly.GetExecutingAssembly());

        Instance = this;
        settings = GetSettings<CalloutModSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public static CalloutMod Instance { get; private set; }

    public override string SettingsCategory()
    {
        return "Callouts!";
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        settings.DoSettingsWindowContents(inRect);
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        CalloutModSettings.UpdateSettings();
    }
}