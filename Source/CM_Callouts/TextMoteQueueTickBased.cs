using Verse;

namespace CM_Callouts;

public class TextMoteQueueTickBased(IntVec3 aPosition, Map aMap) : TextMoteQueue(aPosition, aMap)
{
    private const int delayTicks = 30;
    private int lastReleaseTick = -1;

    public override void AddMote(MoteText newMote)
    {
        newMote.def = CalloutDefOf.CM_Callouts_Thing_Mote_Text_Ticked;
        if (map == null)
        {
            return;
        }

        var currentTick = Find.TickManager.TicksGame;

        if (queuedMotes.Count == 0 && currentTick > lastReleaseTick + delayTicks && Valid())
        {
            lastReleaseTick = currentTick;
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

        var currentTick = Find.TickManager.TicksGame;
        if (currentTick <= lastReleaseTick + delayTicks)
        {
            return true;
        }

        lastReleaseTick = currentTick;
        if (queuedMotes.Count <= 0)
        {
            return false;
        }

        GenSpawn.Spawn(queuedMotes[0], position, map);
        queuedMotes.RemoveAt(0);
        return true;
    }
}