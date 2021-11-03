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
    private const string _isSitsOnToilet = "isSitsOnToilet";
    private const string _isDanceFem = "isDanceFem";
    private const string _isDanceMan = "isDanceMan";
    private const string _isRun = "isRun";
    private const string _isDrunkIdle = "isDrunkIdle";
    private const string _askToLeave = "askToLeave";
    private const string _shrugging = "shrugging";
    private const string _collectsFromGround = "collectsFromGround";
    private const string _reachingOut = "reachingOut";
    private const string _put = "put";
    private const string _kiss = "kiss";

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
            case StartAnimationType.DrunkIdle:
                _animator.SetBool(_isDrunkIdle, true);
                break;
            case StartAnimationType.SitsOnToilet:
                _animator.SetBool(_isSitsOnToilet, true);
                break;
            case StartAnimationType.DanceFem:
                _animator.SetBool(_isDanceFem, true);
                break;
            case StartAnimationType.DanceMan:
                _animator.SetBool(_isDanceMan, true);
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
        switch (usableObjectType)
        {
            case UsableObjectType.DesiredObject:
                _animator.SetTrigger(_shrugging);
                break;
            case UsableObjectType.Guest:
                _animator.SetTrigger(_askToLeave);
                break;
            case UsableObjectType.TrashOnGround:
                _animator.SetTrigger(_put);
                break;
            case UsableObjectType.BrokenFurnitureOnGroud:
                _animator.SetTrigger(_collectsFromGround);
                break;
            case UsableObjectType.ObjectOnWall:
                _animator.SetTrigger(_reachingOut);
                break;
            case UsableObjectType.GuestFem:
                _animator.SetTrigger(_kiss);
                break;
            default:
                break;
        }
    }
}

[Serializable]

public enum StartAnimationType
{
    Idle,
    Sits,
    SitsOnToilet,
    DrunkIdle,
    DanceFem,
    DanceMan
}
