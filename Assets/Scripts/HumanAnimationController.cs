using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HumanAnimationController : MonoBehaviour
{
    [SerializeField] private StartAnimationType _startAnimationType = StartAnimationType.Idle;

    private Animator _animator;

    private const string _isWalk = "isWalk";
    private const string _isIdle = "isIdle";
    private const string _isSits = "isSits";
    private const string _isRun = "isRun";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        switch (_startAnimationType)
        {
            case StartAnimationType.Idle:
                _animator.SetBool(_isIdle, true);
                break;
            case StartAnimationType.Sits:
                _animator.SetBool(_isSits, true);
                break;
        }   
    }

    public void StartIdleAnimation()
    {
        _animator.SetBool(_isWalk, false);
        _animator.SetBool(_isRun, false);
        _animator.SetBool(_isIdle, true);
    }

    public void StartWalkAnimation()
    {
        _animator.SetBool(_isWalk, true);
    }

    public void StartRunAnimation()
    {
        _animator.SetBool(_isRun, true);
    }

    public void StartInspectAnimation(UsableObjectType usableObjectType)
    {
        Debug.LogWarning(usableObjectType);
    }
}

[Serializable]

public enum StartAnimationType
{
    Idle,
    Sits,
}
