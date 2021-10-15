using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GuestMover))]
public class Guest : UsableObject
{
    private GuestMover _mover;

    private void Awake()
    {
        _type = UsableObjectType.Guest;
        _mover = GetComponent<GuestMover>();
    }

    public override void Inspect()
    {
        _type = UsableObjectType.DesiredObject;
        base.Inspect();
        _mover.AllowToMove();
        this.enabled = false;
    }
}
