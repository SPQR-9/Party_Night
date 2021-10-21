using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(OutlinesEffect))]

public class UsableObject : MonoBehaviour
{
    public event UnityAction RecordedInteraction;

    [SerializeField] private Transform _nearPoint;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private float _inspectionTime = 2f;
    [SerializeField] private float _waitingTime = 0f;

    private OutlinesEffect _outlines;

    protected UsableObjectType _type = UsableObjectType.DesiredObject;
    protected bool _isUsable = true;

    public UsableObjectType Type => _type;

    protected void Start()
    {
        _outlines = GetComponent<OutlinesEffect>();
        OffOutlinesEffect();
    }

    private void OnMouseDown()
    {
        if (_playerMover.IsStop || _isUsable==false)
            return;
        if(_playerMover.TryToSetTargetPoint (_nearPoint, transform))
            _outlines.enabled = true;
    }

    private void OnDisable()
    {
        RecordedInteraction?.Invoke();
    }

    public virtual void Inspect()
    {
        OffOutlinesEffect();
        RecordedInteraction?.Invoke();
    }

    public float GetInspectionTime()
    {
        return _inspectionTime;
    }

    public float GetWaitingTime()
    {
        return _waitingTime;
    }

    public void OffOutlinesEffect()
    {
        _outlines.enabled = false;
    }
}

[Serializable]

public enum UsableObjectType
{
    DesiredObject,
    BrokenFurniture,
    Guest,
    Trash
}

