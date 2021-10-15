using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(ButtonActivatorAfterAWhile))]
public class ButtonAnimationController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator _animator;
    private Button _button;
    private ButtonActivatorAfterAWhile _buttonActivatorAfterAWhile;

    private const string _onButtonPoint = "OnButtonPoint";
    private const string _onButtonClick = "OnButtonClick";
    private const string _deactivate = "Deactivate";
    private const string _disable = "Disable";

    private void Awake()
    {
        _buttonActivatorAfterAWhile = GetComponent<ButtonActivatorAfterAWhile>();
        _animator = GetComponent<Animator>();
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _buttonActivatorAfterAWhile.OnButtonClick += StartClickAnimation;
        _buttonActivatorAfterAWhile.OnButtonDisableAfterAWhile += StartDisableAnimation;
    }

    private void OnDisable()
    {
        _buttonActivatorAfterAWhile.OnButtonClick -= StartClickAnimation;
        _buttonActivatorAfterAWhile.OnButtonDisableAfterAWhile -= StartDisableAnimation;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_button.IsInteractable())
            _animator.SetBool(_onButtonPoint, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_button.IsInteractable())
            _animator.SetBool(_onButtonPoint, false);
    }

    public void Deactivate()
    {
        _animator.SetTrigger(_deactivate);
    }

    private void StartClickAnimation()
    {
        _animator.SetTrigger(_onButtonClick);
    }

    private void StartDisableAnimation()
    {
        _animator.SetBool(_disable, true);
    }
}
