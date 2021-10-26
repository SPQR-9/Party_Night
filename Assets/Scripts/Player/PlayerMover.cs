using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(WayAnalizator))]
[RequireComponent(typeof(HumanAnimationController))]
public class PlayerMover : MonoBehaviour
{
    public event UnityAction TargetObjectReached;

    [SerializeField] private Transform _wayPointsContainer;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private Transform[] _wayPoints;
    private Transform _targetPoint;
    private Transform _localTargetPoint = null;
    private WayAnalizator _wayAnalizator;
    private HumanAnimationController _animationController;
    private List<Transform> _currentWay = null;
    private bool _movePermission = true;
    private bool _isTurnOnTargetNeeded = false;
    private bool _isStop = false;
    private Transform _targetObject = null;
    private float _waitingTime = 0f;

    public bool IsStop => _isStop;

    private void Start()
    {
        _wayPoints = _wayPointsContainer.GetComponentsInChildren<Transform>();
        _animationController = GetComponent<HumanAnimationController>();
        _wayAnalizator = GetComponent<WayAnalizator>();
        _wayAnalizator.Initialize(_wayPoints);
        StopMoving();
    }

    private void Update()
    {
        if(_waitingTime>0)
        {
            _waitingTime -= Time.deltaTime;
            return;
        }
        if (_isStop)
            return;
        if (_isTurnOnTargetNeeded)
            TurnOnTargetObject();
        else if (_localTargetPoint != null && _movePermission)
            Move();
    }

    public void StopMoving()
    {
        _isStop = true;
        _animationController.StartIdleAnimation();
        _currentWay = null;
        OffOutlinesOnTargetObject();
        _targetObject = null;
    }

    private void Move()
    {
        _animationController.StartRunAnimation();
        /*_animationController.StartWalkAnimation();*/
        if (IsPlayerReachedPoint(transform, _localTargetPoint))
        {
            DetermineTheDirectionToThePoint();
            return;
        }
        Vector3 targetDirection = _localTargetPoint.position - transform.position;
        Quaternion rotationOnTarget = Quaternion.LookRotation(targetDirection.normalized);
        transform.position = Vector3.MoveTowards(transform.position, _localTargetPoint.position, _speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationOnTarget, _rotationSpeed * Time.deltaTime);
    }

    private void DetermineTheDirectionToThePoint()
    {
        _localTargetPoint = null;
        if (_targetPoint != null && IsPlayerReachedPoint(transform,_targetPoint))
        {
            _currentWay = null;
            Debug.Log("Игрок дошел до конечной точки");
            _animationController.StartIdleAnimation();
            if(_targetObject!=null)
            {
                _movePermission = false;
                _isTurnOnTargetNeeded = true;
            }
        }
        else if (_currentWay != null && _currentWay.Count != 0)
        {
            _currentWay.RemoveAt(0);
            if(_currentWay.Count>0)
                _localTargetPoint = _currentWay[0];
        }
        else
            _animationController.StartIdleAnimation();
    }

    private void TurnOnTargetObject()
    {
        if(_targetObject!=null)
        {
            Vector3 targetDirection = new Vector3(_targetObject.position.x, transform.position.y, _targetObject.position.z) - transform.position;
            Quaternion rotationOnTarget = Quaternion.LookRotation(targetDirection);
            if ((int)transform.rotation.eulerAngles.y == (int)rotationOnTarget.eulerAngles.y)
            {
                _isTurnOnTargetNeeded = false;
                TargetObjectReached?.Invoke();
                return;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationOnTarget, _rotationSpeed * Time.deltaTime);
        }
    }

    public void AllowToMove()
    {
        _movePermission = true;
        _currentWay = null;
        _isStop = false;
    }

    private bool IsPlayerReachedPoint(Transform point1,Transform point2)
    {
        return (int)point1.position.x == (int)point2.position.x
            && (int)point1.position.x == (int)point2.position.x
            && (int)point1.position.z == (int)point2.position.z;
    }

    public void SetWaitingTime(float value)
    {
        _waitingTime = value;
    }

    public bool TryToSetTargetPoint (Transform targetPoint,Transform targetObject = null)
    {
        OffOutlinesOnTargetObject();
        if (_movePermission == false)
            return false;
        _targetPoint = targetPoint;
        _targetObject = targetObject;
        return TryFindAWay();  
    }

    private bool TryFindAWay()
    {
        if (_wayAnalizator.TryFindAWay(transform, _targetPoint, ref _currentWay))
        {
            _localTargetPoint = _currentWay[0];
            return true;
        }
        else
        {
            Debug.LogWarning("Не удалось построить путь до этой точки");
            OffOutlinesOnTargetObject();
            _targetObject = null;
            _targetPoint = null;
            return false;
        }
    }

    public Transform GetTargetObject()
    {
        return _targetObject;
    }

    private void OffOutlinesOnTargetObject()
    {
        if (_targetObject != null)
        {
            if (_targetObject.TryGetComponent(out UsableObject usableObject))
                usableObject.OffOutlinesEffect();
        }
    }
}
