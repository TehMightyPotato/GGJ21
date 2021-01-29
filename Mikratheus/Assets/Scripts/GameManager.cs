using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;
    public int updateRate;
    public EventHandler PlanetUpdate;
    private int _currentUpdateCount;

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        if (_currentUpdateCount == 0)
        {
            PlanetUpdate?.Invoke(this,EventArgs.Empty);
            _currentUpdateCount = updateRate;
        }

        _currentUpdateCount -= 1;
    }
}
