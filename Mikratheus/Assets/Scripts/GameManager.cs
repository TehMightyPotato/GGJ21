using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int updateRate;
    private int _currentUpdateCount;
    
    //Event Handler
    public UnityEvent planetUpdate;

    // Total followers
    public int totalFollower;

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        if (_currentUpdateCount == 0)
        {
            planetUpdate.Invoke();
            _currentUpdateCount = updateRate;
            return;
        }
        _currentUpdateCount -= 1;

        // Total Follower berechnen
        var planetList = PlanetManager.Instance.planets;
        int totalFollowerCount = 0;
        for (int i = 0; i < planetList.Count; i++)
        {
            totalFollowerCount += planetList[i].GetComponent<Planet>().currentFollowers;
        }
        totalFollower = totalFollowerCount;
    }
}
