using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(GoalSetterForFreePlayerMovement))]
public class Player : MonoBehaviour
{
    private PlayerMover _mover;
    private Transform _targetObject;
    private HumanAnimationController _animation;

    private void Awake()
    {
        _mover = GetComponent<PlayerMover>();
        _animation = GetComponent<HumanAnimationController>();
    }

    private void OnEnable()
    {
        _mover.TargetObjectReached += InspectTargetObject;
    }

    private void OnDisable()
    {
        _mover.TargetObjectReached -= InspectTargetObject;
    }

    private void InspectTargetObject()
    {
        StopAllCoroutines();
        _targetObject = _mover.GetTargetObject();
        if(_targetObject.TryGetComponent(out UsableObject usableObject))
        {
            _animation.StartInspectAnimation(usableObject.Type);
            StartCoroutine(InspectObjectAfterAWhile(usableObject));
        }
        else
        {
            Debug.LogWarning("Не получен usableObject");
            _mover.AllowToMove();
        }
    }

    public void StopInspection()
    {
        StopAllCoroutines();
    }

    private IEnumerator InspectObjectAfterAWhile(UsableObject usableObject)
    {
        Debug.Log("Начало осмотра");
        _animation.StartInspectAnimation(usableObject.Type);
        yield return new WaitForSeconds(usableObject.GetInspectionTime());
        if (usableObject.GetWaitingTime() > 0)
            _mover.SetWaitingTime(usableObject.GetWaitingTime());
        _mover.AllowToMove();
        _animation.StartIdleAnimation();
        Debug.Log("Завершение осмотра");
        usableObject.Inspect();
    }


}
