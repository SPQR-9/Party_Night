using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CameraController))]
public class StartPanelActivator : MonoBehaviour
{
    public UnityEvent ActivateOnStart;
    public UnityEvent ActivateWhenPhonePanelIsTurnedOn;
    public UnityEvent ActivateAfterPhonePanelDisable;

    [SerializeField] private float _timeToActivatePhonePanel = 5f;
    [SerializeField] private float _timePhonePanel = 3f;

    private bool _isPhonePanelActivate = false;
    private CameraController _cameraController;

    private void OnEnable()
    {
        ActivateOnStart?.Invoke();
        _cameraController = GetComponent<CameraController>();
    }

    private void Update()
    {
        if (!_isPhonePanelActivate)
        {
            _timeToActivatePhonePanel -= Time.deltaTime;
            if(_timeToActivatePhonePanel<=0)
            {
                _isPhonePanelActivate = true;
                _cameraController.SwitchOnPhoneCamera();
                ActivateWhenPhonePanelIsTurnedOn?.Invoke();
            }
        }
        else
        {
            _timePhonePanel -= Time.deltaTime;
            if (_timePhonePanel <= 0)
                DisableStartPanel();
        }
    }

    public void DisableStartPanel()
    {
        ActivateAfterPhonePanelDisable?.Invoke();
        _cameraController.SwitchOnMainCamera();
        gameObject.SetActive(false);
    }    
}
