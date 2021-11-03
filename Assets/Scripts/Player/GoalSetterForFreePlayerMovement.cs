using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class GoalSetterForFreePlayerMovement : MonoBehaviour
{
    [SerializeField] private RaycastHit _raycastHit;
    [SerializeField] private Transform _targetPoint;

    private PlayerMover _playerMover;

    private void Awake()
    {
        _playerMover = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out _raycastHit))
            {
                if(_raycastHit.collider.TryGetComponent(out Floor _))
                {
                    Vector3 oldTargetPointPosition = _targetPoint.position;
                    _targetPoint.position = new Vector3(_raycastHit.point.x, _targetPoint.position.y, _raycastHit.point.z);
                    if(!_playerMover.TryToSetTargetPoint(_targetPoint))
                        _targetPoint.position = oldTargetPointPosition;
                }
            }
        }
    }
}
