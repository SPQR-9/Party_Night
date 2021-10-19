using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class WayAnalizator : MonoBehaviour
{
    [SerializeField] private int _maxCompletedWaysCount = 10;

    private Transform[] _wayPoints;

    private int _closestTargetPointIndex;
    private int _closestCurrentPointIndex;
    private List<Way> _ways = new List<Way>();
    private List<Way> _completedWays = new List<Way>();

    public void Initialize(Transform[] wayPoints)
    {
        _wayPoints = wayPoints;
    }

    public bool TryFindAWay(Transform currentPoint, Transform targetPoint,ref List<Transform> completedWay)
    {
        completedWay = new List<Transform>();
        if (currentPoint == targetPoint || IsPuthBetweenPointAvailble(currentPoint, targetPoint))
        {
            completedWay = new List<Transform>();
            completedWay.Add(targetPoint);
            return true;
        }
        _closestCurrentPointIndex = FindNearestWaypointIndex(currentPoint);
        _closestTargetPointIndex = FindNearestWaypointIndex(targetPoint);
        if (_closestCurrentPointIndex == -1 || _closestTargetPointIndex == -1)
            return false;
        if(IsWayAvailable())
        {
            FindAllPossibleWays();
            completedWay.Add(currentPoint);
            completedWay.AddRange(FindShortestWay());
            completedWay.Add(targetPoint);
            return true;
        }
        completedWay = null;
        return false;
    }

    private int FindNearestWaypointIndex(Transform point)
    {
        int nearestWaypointIndex = -1;
        for (int i = 0; i < _wayPoints.Length; i++)
        {
            if (IsPuthBetweenPointAvailble(point,_wayPoints[i]))
            {
                if (nearestWaypointIndex == -1)
                {
                    nearestWaypointIndex = i;
                    continue;
                }
                if(Vector3.Distance(point.position, _wayPoints[nearestWaypointIndex].position) > Vector3.Distance(point.position, _wayPoints[i].position))
                    nearestWaypointIndex = i;
            }
        }
        return nearestWaypointIndex;
    }

    private bool IsWayAvailable()
    {
        List<Transform> availablePoints = new List<Transform>();
        availablePoints.Add(_wayPoints[_closestCurrentPointIndex]);
        bool isAddedNewAvailablePoints = true;
        while (isAddedNewAvailablePoints)
        {
            isAddedNewAvailablePoints = false;
            for (int i = 0; i < availablePoints.Count; i++)
            {
                for (int j = 0; j < _wayPoints.Length; j++)
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
            if (availablePoints.Contains(_wayPoints[_closestTargetPointIndex]))
                return true;
        }
        return false;
    }

    private void FindAllPossibleWays()
    {
        _ways = new List<Way>();
        _ways.Add(new Way(_closestCurrentPointIndex));
        _completedWays = new List<Way>();
        do
        {
            List<Way> newWays = new List<Way>();
            for (int i = 0; i < _ways.Count; i++)
            {
                List<int> wayPointNumbers = _ways[i].PointNumbers;
                for (int j = 0; j < _wayPoints.Length; j++)
                {
                    if (wayPointNumbers.Contains(j))
                        continue;
                    int theLastPointIndex = _ways[i].PointNumbers[_ways[i].PointNumbers.Count - 1];
                    if (IsPuthBetweenPointAvailble(_wayPoints[theLastPointIndex], _wayPoints[j]))
                    {
                        Way newWay = new Way();
                        newWay.PointNumbers.AddRange(wayPointNumbers);
                        newWay.PointNumbers.Add(j);
                        if (IsWayExistsInWays(newWay, newWays))
                            continue;
                        newWays.Add(newWay);
                        if (j == _closestTargetPointIndex)
                            _completedWays.Add(newWay);
                    }
                }
            }
            _ways = new List<Way>(newWays);
        }
        while (_completedWays.Count<_maxCompletedWaysCount && _ways.Count>0);
    }

    private List<Transform> FindShortestWay()
    {
        float minLenght = 0f;
        int shotersWayIndex = 0;
        for (int i = 0; i < _completedWays.Count; i++)
        {
            float lenght = 0f;
            if (_completedWays[i].PointNumbers.Count > 0)
            {
                for (int j = 1; j < _completedWays[i].PointNumbers.Count; j++)
                {
                    lenght += Vector3.Distance(_wayPoints[_completedWays[i].PointNumbers[j-1]].position, _wayPoints[_completedWays[i].PointNumbers[j]].position);
                }
                if (minLenght == 0 || minLenght > lenght)
                {
                    minLenght = lenght;
                    shotersWayIndex = i;
                }
            }
        }
        Debug.Log(ShowWayPointNumbers(_completedWays[shotersWayIndex].PointNumbers));
        Debug.Log("Lenght" + minLenght);
        return _completedWays[shotersWayIndex].GetWayPoints(_wayPoints);
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

    private bool IsWayExistsInWays(Way way, List<Way> ways)
    {
        foreach (var item in ways)
        {
            if (way.PointNumbers == item.PointNumbers)
                return true;
        }
        return false;
    }

    private string ShowWayPointNumbers(List<int> pointNumbers)
    {
        string numbersLine = "";
        foreach (var number in pointNumbers)
        {
            numbersLine += number;
            numbersLine += " ";
        }
        return numbersLine;
    }

    private void ShowAllWayes(List<Way> ways)
    {
        Debug.LogWarning("Show " + ways.Count);
        for (int i = 0; i < ways.Count; i++)
        {
            string numbersLine = "";
            foreach (var number in ways[i].PointNumbers)
            {
                numbersLine += number.ToString();
                numbersLine += " ";
            }
            Debug.Log(i  + ") " + numbersLine + " count = " + numbersLine.Length);
        }
    }
}

[System.Serializable]
public class Way
{
    public Way(int currentPointIndex)
    {
        PointNumbers.Add(currentPointIndex);
    }

    public Way() { }

    public List<int> PointNumbers = new List<int>();

    public List<Transform> GetWayPoints(Transform[] wayPoints)
    {
        List<Transform> way = new List<Transform>();
        foreach (var index in PointNumbers)
        {
            way.Add(wayPoints[index]);
        }
        return way;
    }
}


