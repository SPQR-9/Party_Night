using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class WayAnalizator : MonoBehaviour
{
    [SerializeField] private int _maxCompletedWaysCount = 10;

    private Transform[] _wayPoints;

    private int _targetPointIndex;
    private int _currentPointIndex;
    private List<Way> _ways = new List<Way>();
    private List<Way> _completedWays = new List<Way>();

    public void Initialize(Transform[] wayPoints)
    {
        _wayPoints = wayPoints;
    }

    public bool TryFindAWay(Transform currentPoint, Transform targetPoint,ref List<int> completedWay)
    {
        completedWay = new List<int>();
        _currentPointIndex = IndexDefinition(currentPoint);
        _targetPointIndex = IndexDefinition(targetPoint);
        if (_currentPointIndex == -1 || _targetPointIndex == -1)
            return false;
        if (_currentPointIndex == _targetPointIndex)
        {
            completedWay = new List<int>();
            completedWay.Add(_targetPointIndex);
            return true;
        }
        if(IsTargetPointAvailable())
        {
            FindAllPossibleWays();
            completedWay.AddRange(FindShortestWay());
            return true;
        }
        completedWay = null;
        return false;
    }

    private int IndexDefinition(Transform point)
    {
        for (int i = 0; i < _wayPoints.Length; i++)
        {
            if (point.position == _wayPoints[i].position)
                return i;
        }
        Debug.LogError("“очка не существует в пуле точек пути");
        return -1;
    }

    private bool IsTargetPointAvailable()
    {
        List<Transform> availablePoints = new List<Transform>();
        availablePoints.Add(_wayPoints[_currentPointIndex]);
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
            if (availablePoints.Contains(_wayPoints[_targetPointIndex]))
                return true;
        }
        return false;
    }

    private void FindAllPossibleWays()
    {
        _ways = new List<Way>();
        _ways.Add(new Way(_currentPointIndex));
        _completedWays = new List<Way>();
        int numberOfNewWays;
        do
        {
            numberOfNewWays = 0;
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
                        numberOfNewWays++;
                        if (j == _targetPointIndex)
                            _completedWays.Add(newWay);
                    }
                }
            }
            _ways = new List<Way>(newWays);
        }
        while (_completedWays.Count<_maxCompletedWaysCount && _ways.Count>0);
    }

    private List<int> FindShortestWay()
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
        return _completedWays[shotersWayIndex].PointNumbers;
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
}


