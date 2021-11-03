using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpriteReaction : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2f;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _targetObject;

    private Animator _animator;

    private const string _lifeTimeOver = "LifeTimeOver";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 targetDirection = _camera.position - transform.position;
        transform.rotation = Quaternion.LookRotation(targetDirection.normalized);
        transform.position = new Vector3(_targetObject.position.x, transform.position.y, _targetObject.position.z);
        if(_lifeTime>0)
        {
            _lifeTime -= Time.deltaTime;
            if (_lifeTime <= 0)
                _animator.SetBool(_lifeTimeOver,true);
        }
    }
}
