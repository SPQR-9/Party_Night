using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WayAnalizator : MonoBehaviour
{
    [SerializeField] private int _maxCompletedWaysCount = 10;

    private List<Transform> _initialWayPoints = new List<Transform>();
    private List<Transform> _wayPoints;
    private Transform _startPoint;
    private Transform _targetPoint;
    private List<Way> _completedWays = new List<Way>();

    public void Initialize(Transform[] wayPoints)
    {
        _initialWayPoints.AddRange(wayPoints);
    }

    public bool TryFindAWay(Transform currentPoint, Transform targetPoint,ref List<Transform> completedWay)
    {
        _startPoint = currentPoint;
        _targetPoint = targetPoint;
        completedWay = new List<Transform>();
        if (_startPoint == _targetPoint || IsPuthBetweenPointAvailble(_startPoint, _targetPoint))
        {
            completedWay = new List<Transform>();
            completedWay.Add(_targetPoint);
            return true;
        }
        _wayPoints = new List<Transform>();
        if(!_initialWayPoints.Contains(_startPoint))
            _wayPoints.Add(_startPoint);
        _wayPoints.AddRange(_initialWayPoints);
        if(!_initialWayPoints.Contains(_targetPoint))
            _wayPoints.Add(_targetPoint);
        if (IsWayAvailable())
        {
            FindAllPossibleWays();
            completedWay.AddRange(FindShortestWay());
            return true;
        }
        completedWay = null;
        return false;
    }

    private bool IsWayAvailable()
    {
        List<Transform> availablePoints = new List<Transform>();
        availablePoints.Add(_startPoint);
        bool isAddedNewAvailablePoints = true;
        while (isAddedNewAvailablePoints)
        {
            isAddedNewAvailablePoints = false;
            for (int i = 0; i < availablePoints.Count; i++)
            {
                for (int j = 0; j < _wayPoints.Count; j++)
                {
                    if (availablePoints.Contains(_wayPoints[j]))
                        continue;
                    if (IsPuthBetweenPointAvailble(availablePoints[i], _wayPoints[j]))
                    {
                        availablePoints.Add(_wayPoints[j]);
                        isAddedNewAvailablePoints = true;
                    }
                }
            }
            if (availablePoints.Contains(_targetPoint))
                return true;
        }
        return false;
    }

    private void FindAllPossibleWays()
    {
        List<Way> ways = new List<Way>();
        ways.Add(new Way(_startPoint));
        _completedWays = new List<Way>();
        List<Way> newWays;
        do
        {
            newWays = new List<Way>();
            for (int i = 0; i < ways.Count; i++)
            {
                for (int j = 0; j < _wayPoints.Count; j++)
                {
                    if (ways[i].WayPoints.Contains(_wayPoints[j]))
                        continue;
                    if(IsPuthBetweenPointAvailble(ways[i].WayPoints[ways[i].WayPoints.Count-1],_wayPoints[j]))
                    {
                        newWays.Add(new Way());
                        newWays[newWays.Count - 1].WayPoints = new List<Transform>(ways[i].WayPoints); //
                        newWays[newWays.Count - 1].WayPoints.Add(_wayPoints[j]);
                        if (_wayPoints[j] == _targetPoint)
                            _completedWays.Add(newWays[newWays.Count - 1]);
                    }
                }
            }
            ways = new List<Way>(newWays);
        }
        while (_completedWays.Count<_maxCompletedWaysCount && newWays.Count>0);
    }

    private List<Transform> FindShortestWay()
    {
        float minLenght = 0f;
        int shotersWayIndex = 0;
        for (int i = 0; i < _completedWays.Count; i++)
        {
            float lenght = 0f;
            if(_completedWays[i].WayPoints.Count>1)
            {
                for (int j = 1; j < _completedWays[i].WayPoints.Count; j++)
                {
                    lenght += Vector3.Distance(_completedWays[i].WayPoints[j - 1].position, _completedWays[i].WayPoints[j].position);
                }
                if (minLenght == 0 || minLenght > lenght)
                {
                    minLenght = lenght;
                    shotersWayIndex = i;
                }
            }
        }
        Debug.Log(ShowWay(_completedWays[shotersWayIndex].WayPoints));
        Debug.Log("Lenght" + minLenght);
        return _completedWays[shotersWayIndex].WayPoints;
    }

    private bool IsPuthBetweenPointAvailble(Transform point1, Transform point2)
    {
        if (point1.position == point2.position)
            return false;
        RaycastHit rayResult;
        Ray ray = new Ray(point1.position, point2.position - point1.position);
        Physics.Raycast(ray, out rayResult, Vector3.Distance(point1.position, point2.position));
        return rayResult.collider == null;
    }

    private string ShowWay(List<Transform> wayPoints)
    {
        string numbersLine = "";
        foreach (var point in wayPoints)
        {
            numbersLine += point.name;
            numbersLine += " ";
        }
        return numbersLine;
    }
}

[System.Serializable]
public class Way
{
    public Way(Transform startPoint)
    {
        WayPoints.Add(startPoint);
    }

    public Way() { }

    public List<Transform> WayPoints = new List<Transform>();
}


