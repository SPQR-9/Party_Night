using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : UsableObject
{
    [SerializeField] private bool _isOnGround;
    private void Awake()
    {
        _type = UsableObjectType.TrashOnWall;
        if(_isOnGround)
            _type = UsableObjectType.TrashOnGround;
    }

    public override void Inspect()
    {
        gameObject.SetActive(false);
        base.Inspect();
    }
}
