using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace CM_Callouts;

[HarmonyPatch(typeof(MoteMaker), nameof(MoteMaker.ThrowText), typeof(Vector3), typeof(Map), typeof(string),
    typeof(Color), typeof(float))]
public static class MoteMaker_ThrowText
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var moteSpawner = AccessTools.Method(typeof(GenSpawn), nameof(GenSpawn.Spawn),
            [typeof(Thing), typeof(IntVec3), typeof(Map), typeof(WipeMode)]);
        var newMoteSpawner = AccessTools.Method(typeof(CalloutTracker), nameof(CalloutTracker.ThrowText),
            [typeof(Thing), typeof(IntVec3), typeof(Map), typeof(WipeMode)]);

        var replaceCall = true;
        foreach (var instruction in instructions)
        {
            if (replaceCall && instruction.Calls(moteSpawner))
            {
                replaceCall = false;
                instruction.operand = newMoteSpawner;
            }

            yield return instruction;
        }
    }
}