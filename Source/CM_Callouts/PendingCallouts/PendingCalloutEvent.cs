using System.Collections.Generic;
using Verse;

namespace CM_Callouts.PendingCallouts;

public abstract class PendingCalloutEvent(CalloutCategory category)
{
    protected readonly CalloutCategory category = category;
    protected BodyDef body;
    protected List<BodyPartRecord> bodyPartsDamaged = [];
    protected List<bool> bodyPartsDestroyed = [];

    public void FillBodyPartInfo(BodyDef _body, List<BodyPartRecord> _bodyPartsDamaged, List<bool> _bodyPartsDestroyed)
    {
        body = _body;
        bodyPartsDamaged = _bodyPartsDamaged;
        bodyPartsDestroyed = _bodyPartsDestroyed;
    }

    public virtual void AttemptCallout()
    {
        Logger.MessageFormat(this, "Attempting callout.");
    }
}