using System.Collections;
using UnityEngine;

public class ButtonsDeactivatorOnPanel : MonoBehaviour
{
    private ButtonActivatorAfterAWhile[] _childrenButtons;
    private ButtonAnimationController[] _childrenButtonsAnimation;

    private void OnEnable()
    {
        _childrenButtons = GetComponentsInChildren<ButtonActivatorAfterAWhile>();
        _childrenButtonsAnimation = GetComponentsInChildren<ButtonAnimationController>();
        foreach (var button in _childrenButtons)
        {
            button.OnButtonClick += DeactivateAllButtons;
        }
    }

    private void OnDisable()
    {
        foreach (var button in _childrenButtons)
        {
            button.OnButtonClick -= DeactivateAllButtons;
        }
    }

    private void DeactivateAllButtons()
    {
        StartCoroutine(DisableAfterASecond());
        foreach (var buttonAnimation in _childrenButtonsAnimation)
        {
            buttonAnimation.Deactivate();
        }
    }

    private IEnumerator DisableAfterASecond()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
