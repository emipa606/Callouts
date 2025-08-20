using System.Collections.Generic;
using Verse;

namespace CM_Callouts;

public abstract class TextMoteQueue(IntVec3 aPosition, Map aMap)
{
    protected readonly Map map = aMap;
    protected readonly List<MoteText> queuedMotes = [];
    protected IntVec3 position = aPosition;


    public bool Valid()
    {
        return position != IntVec3.Invalid && map != null && !map.info.parent.Destroyed;
    }

    public abstract void AddMote(MoteText newMote);

    public abstract bool Update();
}