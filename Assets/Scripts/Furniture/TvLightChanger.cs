using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvLightChanger : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _minLightIntensity = 0.5f;
    [SerializeField] private float _maxLightIntensity = 1.5f;
    [SerializeField] private float _minTimeChanged = 0.1f;
    [SerializeField] private float _maxTimeChanged = 0.5f;

    private float _timer = 0f;

    private void Update()
    {
        if (_timer > 0)
            _timer -= Time.deltaTime;
        else
        {
            _light.intensity = Random.Range(_minLightIntensity, _maxLightIntensity);
            _timer = Random.Range(_minTimeChanged, _maxTimeChanged);
        }
    }
}
