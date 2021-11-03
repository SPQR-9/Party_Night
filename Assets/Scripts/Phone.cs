using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Phone : MonoBehaviour
{
    public UnityEvent ActivateAfterClick;

    private void OnMouseDown()
    {
        ActivateAfterClick?.Invoke();
        gameObject.SetActive(false);
    }
}
