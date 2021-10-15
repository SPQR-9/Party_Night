using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenFurniture : UsableObject
{
    [SerializeField] private GameObject _renovatedFurniture;

    private void Awake()
    {
        _type = UsableObjectType.BrokenFurniture;
        _renovatedFurniture.SetActive(false);
    }

    public override void Inspect()
    {
        gameObject.SetActive(false);
        _renovatedFurniture.SetActive(true);
        base.Inspect();
    }
}
