using UnityEngine;
using System.Collections;
using System;

public abstract class Weapon : MonoBehaviour, IWeapon {

    protected bool mainTriggerReleased = true;
    protected bool secondaryTriggerRealeased = true;
    public LayerMask layer;

    public virtual void MainTriggerHold()
    {
        mainTriggerReleased = false;
    }

    public virtual void MainTriggerRelease()
    {
        mainTriggerReleased = true;
    }

    public virtual void SecondaryTriggerHold()
    {
        secondaryTriggerRealeased = false;
    }

    public virtual void SecondaryTriggerRelease()
    {
        secondaryTriggerRealeased = true;
    }
}
