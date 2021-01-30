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

    // Spielinfos
    public int totalFollower;
    public int godPower;
    public int godPowerBaseIncrease;
    public int godPowerLimit;


    private void Awake()
    {
        Instance = this;
        godPower = 0;
    }

    private void Start()
    {
        StartCoroutine(IncreaseGodPower());
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

    public IEnumerator IncreaseGodPower()
    {
        while (true)
        {
            godPower += godPowerBaseIncrease;
            if (godPower > godPowerLimit)
            {
                godPower = godPowerLimit;
            }
            yield return new WaitForSeconds(2);
        }
    }
}
