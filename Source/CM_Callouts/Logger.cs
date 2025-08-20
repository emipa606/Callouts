using System.Diagnostics;
using Verse;

namespace CM_Callouts;

public static class Logger
{
    public static void MessageFormat(object caller, string message, params object[] stuff)
    {
        if (!CalloutMod.settings.showDebugLogMessages)
        {
            return;
        }

        message =
            $"[Callouts]: {caller.GetType()}.{new StackTrace().GetFrame(1).GetMethod().Name} - {message}";
        Log.Message(string.Format(message, stuff));
    }

    public static void WarningFormat(object caller, string message, params object[] stuff)
    {
        message =
            $"[Callouts]: {caller.GetType()}.{new StackTrace().GetFrame(1).GetMethod().Name} - {message}";
        Log.Warning(string.Format(message, stuff));
    }
}