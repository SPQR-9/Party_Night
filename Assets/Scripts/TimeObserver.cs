using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeObserver : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private UnwantedObjectFinder _unwantedObjectFinder;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private UnityEvent _acivateAfterWin;
    [SerializeField] private UnityEvent _activateAtEndTimer;

    private bool _isStop = false;

    public event UnityAction TimesUp;

    private void OnEnable()
    {
        _unwantedObjectFinder.NonUwantedObjectsFound += Win;
    }

    private void OnDisable()
    {
        _unwantedObjectFinder.NonUwantedObjectsFound -= Win;
    }

    private void Start()
    {
        StopTimer();
    }

    private void Update()
    {
        if (_isStop)
            return;
        if((int)_time==0)
        {
            TimesUp?.Invoke();
            _activateAtEndTimer?.Invoke();
            StopTimer();
        }
        _time -= Time.deltaTime;
        _timerText.text = ((int)_time).ToString();
    }

    private void Win()
    {
        _acivateAfterWin?.Invoke();
        _isStop = true;
    }

    public void StopTimer()
    {
        _isStop = true;
    }

    public void RunTimer()
    {
        _isStop = false;
    }
}
