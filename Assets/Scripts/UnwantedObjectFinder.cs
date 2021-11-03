using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnwantedObjectFinder : MonoBehaviour
{
    public event UnityAction NonUwantedObjectsFound;

    private UsableObject[] _unwantedUsableObjects;

    private void OnEnable()
    {
        _unwantedUsableObjects = GetComponentsInChildren<UsableObject>();
        foreach (var item in _unwantedUsableObjects)
        {
            item.RecordedInteraction += FindUnwantedObjects;
        }
    }

    private void OnDisable()
    {
        foreach (var item in _unwantedUsableObjects)
        {
            item.RecordedInteraction -= FindUnwantedObjects;
        }
    }

    private void FindUnwantedObjects()
    {
        foreach (var item in _unwantedUsableObjects)
        {
            if (item.Type != UsableObjectType.DesiredObject && item.gameObject.activeSelf)
                return;
        }
        NonUwantedObjectsFound?.Invoke();
    }
}
