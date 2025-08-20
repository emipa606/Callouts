using UnityEngine;
using Verse;

namespace CM_Callouts;

public class TextMoteQueueRealTime(IntVec3 aPosition, Map aMap) : TextMoteQueue(aPosition, aMap)
{
    private const float delayTime = 1.0f / 2.0f;
    private float lastReleaseTime;
    private float timeAccumulated;

    public override void AddMote(MoteText newMote)
    {
        if (queuedMotes.Count == 0 && timeAccumulated > lastReleaseTime + delayTime && Valid())
        {
            lastReleaseTime = timeAccumulated;
            GenSpawn.Spawn(newMote, position, map);
        }
        else
        {
            queuedMotes.Add(newMote);
        }
    }

    public override bool Update()
    {
        if (!Valid())
        {
            return false;
        }

        timeAccumulated += Time.deltaTime;

        if (!(timeAccumulated > lastReleaseTime + delayTime))
        {
            return true;
        }

        lastReleaseTime = timeAccumulated;
        if (queuedMotes.Count <= 0)
        {
            return false;
        }

        GenSpawn.Spawn(queuedMotes[0], position, map);
        queuedMotes.RemoveAt(0);
        return true;
    }
}