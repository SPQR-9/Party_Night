using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _losePoint;
    [SerializeField] private float _speed;
    private bool _isLose = false;

    private void Update()
    {
        if (_isLose == false)
            transform.position = new Vector3(transform.position.x, transform.position.y, _player.position.z - 18f);
        else
            transform.position = Vector3.MoveTowards(transform.position, _losePoint.position, _speed * Time.deltaTime);
    }

    public void AllowMovingUpToLosePoint()
    {
        _isLose = true;
    }
}
