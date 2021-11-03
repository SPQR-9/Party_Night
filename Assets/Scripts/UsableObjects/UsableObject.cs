using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(OutlinesEffect))]

public class UsableObject : MonoBehaviour
{
    public event UnityAction RecordedInteraction;
    public UnityEvent ActivateAfterInspect;

    [SerializeField] private Transform _nearPoint;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private float _inspectionTime = 1f;
    [SerializeField] private float _waitingTime = 0f;
    [SerializeField] protected UsableObjectType _type = UsableObjectType.DesiredObject;
    [SerializeField] protected bool _isUsable = true;

    private OutlinesEffect _outlines;


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
        ReturnTypeToDefaultType();
        RecordedInteraction?.Invoke();
    }

    public virtual void Inspect()
    {
        ActivateAfterInspect?.Invoke();
        RecordedInteraction?.Invoke();
        OffOutlinesEffect();
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

    public void DisallowUseOf()
    {
        _isUsable = false;
    }

    public void AllowUse()
    {
        _isUsable = true;
    }

    public void ReturnTypeToDefaultType()
    {
        _type = UsableObjectType.DesiredObject;
    }
}

[Serializable]

public enum UsableObjectType
{
    DesiredObject,
    BrokenFurnitureOnGroud,
    Guest,
    ObjectOnWall,
    TrashOnGround,
    GuestFem
}

