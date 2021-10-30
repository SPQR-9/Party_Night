using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _phoneCamera;

    private void Start()
    {
        _phoneCamera.SetActive(false);
    }

    public void SwitchOnMainCamera()
    {
        _mainCamera.SetActive(true);
        _phoneCamera.SetActive(false);
    }

    public void SwitchOnPhoneCamera()
    {
        _phoneCamera.SetActive(true);
        _mainCamera.SetActive(false);
    }
}
