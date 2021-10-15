using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : UsableObject
{
    private void Awake()
    {
        _type = UsableObjectType.Trash;
    }

    public override void Inspect()
    {
        gameObject.SetActive(false);
        base.Inspect();
    }
}
