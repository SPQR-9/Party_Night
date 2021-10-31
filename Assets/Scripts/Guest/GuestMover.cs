using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HumanAnimationController))]
public class GuestMover : MonoBehaviour
{
    [SerializeField] private List<Transform> _way;
    [SerializeField] private float _idleTime = 0.5f;
    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _rotationSpeed = 5f;

    private bool _movePermission = false;
    private Transform _localTargetPoint;
    private HumanAnimationController _animationController;

    private void Awake()
    {
        _animationController = GetComponent<HumanAnimationController>();
    }

    private void Update()
    {
        if (!_movePermission)
            return;
        if (_idleTime > 0)
        {
            _animationController.StartIdleAnimation();
            _idleTime -= Time.deltaTime;
        }
        else
            Move();
    }

    private void Move()
    {
        if (_way.Count > 0)
        {
            _animationController.StartRunAnimation();
            _localTargetPoint = _way[0];
            if (IsReachedPoint(transform, _localTargetPoint))
                _way.RemoveAt(0);
            Vector3 targetDirection = _localTargetPoint.position - transform.position;
            Quaternion rotationOnTarget = Quaternion.LookRotation(targetDirection.normalized);
            transform.position = Vector3.MoveTowards(transform.position, _localTargetPoint.position, _speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationOnTarget, _rotationSpeed * Time.deltaTime);
        }
        else
            gameObject.SetActive(false);
    }

    private bool IsReachedPoint(Transform point1, Transform point2)
    {
        return (int)point1.position.x == (int)point2.position.x
            && (int)point1.position.x == (int)point2.position.x
            && (int)point1.position.z == (int)point2.position.z;
    }

    public void AllowToMove()
    {
        _movePermission = true;
    }
}
