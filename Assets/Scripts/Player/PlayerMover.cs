using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(WayAnalizator))]
[RequireComponent(typeof(HumanAnimationController))]
public class PlayerMover : MonoBehaviour
{
    public event UnityAction TargetObjectChanged;
    public event UnityAction TargetObjectReached;

    [SerializeField] private Transform _currentPoint;
    [SerializeField] private Transform _wayPointsContainer;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    /*    [SerializeField] private float _maximumInaccuracyValue = 0.01f;*/

    private Transform[] _wayPoints;
    private Transform _targetPoint;
    private Transform _localTargetPoint = null;
    private WayAnalizator _wayAnalizator;
    private HumanAnimationController _animationController;
    private List<Transform> _currentWay = null;
    private bool _isTargetChanges = false;
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
        transform.position = new Vector3(_currentPoint.position.x, _currentPoint.position.y, _currentPoint.position.z);
        _wayAnalizator.Initialize(_wayPoints);
        
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
        _animationController.StartWalkAnimation();
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
        _currentPoint = _localTargetPoint;
        _localTargetPoint = null;
        if (_isTargetChanges)
        {
            _isTargetChanges = false;
            TryFindAWay();
        }
        if (_targetPoint != null && _currentPoint.position == _targetPoint.position)
        {
            _currentWay = null;
            Debug.Log("Игрок дошел до конечной точки");
            _animationController.StartIdleAnimation();
            if(_targetObject!=null)
            {
                _movePermission = false;
                _isTurnOnTargetNeeded = true;
            }
            return;
        }
        if (_currentWay != null && _currentWay.Count != 0)
        {
            _currentWay.RemoveAt(0);
            _localTargetPoint = _currentWay[0];
        }
        else
            _animationController.StartIdleAnimation();
    }

    private void TurnOnTargetObject()
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
        /*return point1.position.x >= point2.position.x - _maximumInaccuracyValue && point1.position.x <= point2.position.x + _maximumInaccuracyValue
            && point1.position.y >= point2.position.y - _maximumInaccuracyValue && point1.position.y <= point2.position.y + _maximumInaccuracyValue
            && point1.position.z >= point2.position.z - _maximumInaccuracyValue && point1.position.z <= point2.position.z + _maximumInaccuracyValue*/;
    }

    public void SetWaitingTime(float value)
    {
        _waitingTime = value;
    }

    public bool TryToSetTargetPoint (Transform targetPoint,Transform targetObject = null)
    {
        if (_movePermission == false)
            return false;
        _targetPoint = targetPoint;
        _targetObject = targetObject;
        if(_localTargetPoint == null)
            return TryFindAWay();  
        else
        {
            TargetObjectChanged?.Invoke();
            _isTargetChanges = true;
        }
        return true;
    }

    private bool TryFindAWay()
    {
        if (_wayAnalizator.TryFindAWay(_currentPoint, _targetPoint, ref _currentWay))
        {
            _localTargetPoint = _currentWay[0];
            return true;
        }
        else
        {
            Debug.LogWarning("Не удалось построить путь до этого объекта");
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
