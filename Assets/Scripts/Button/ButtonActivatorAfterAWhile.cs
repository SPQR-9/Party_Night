using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Button))]
public class ButtonActivatorAfterAWhile : MonoBehaviour, IPointerClickHandler/* , IPointerEnterHandler, IPointerExitHandler*/
{
    [SerializeField] private bool _isEventAfterSecondExist;
    [SerializeField] private float _timeOfEventActivation = 1f;
    [SerializeField] private bool _isDisableAfterActivation = true;
    [SerializeField] private UnityEvent _onClickAfterAWhile;

    public event UnityAction OnButtonClick;
    public event UnityAction OnButtonActivationAfterAWhile;
    public event UnityAction OnButtonDisableAfterAWhile;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_button.IsInteractable())
        {
            if (_isEventAfterSecondExist)
                StartCoroutine(ActivateAfterAWhile());
            OnButtonClick?.Invoke();
        }
    }

    private IEnumerator ActivateAfterAWhile()
    {
        yield return new WaitForSeconds(_timeOfEventActivation);
        if(_isDisableAfterActivation)
            OnButtonDisableAfterAWhile?.Invoke();
        OnButtonActivationAfterAWhile?.Invoke();
        _onClickAfterAWhile?.Invoke();
    }
}
