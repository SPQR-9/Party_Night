using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartPanel : MonoBehaviour
{
    public UnityEvent ActivateOnStart;

    private void OnEnable()
    {
        ActivateOnStart?.Invoke();
    }
}
