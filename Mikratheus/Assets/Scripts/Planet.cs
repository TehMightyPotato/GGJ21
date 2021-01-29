using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    public int totalPop;

    public int currentFollowers;

    public float eventGenerationChance;

    public bool eventIsActive;
    [SerializeField] private float lastEventGeneratedTime;

    private void Start()
    {
        GameManager.Instance.planetUpdate.AddListener(UpdatePlanet);
    }

    private void UpdatePlanet()
    {
        return;
        eventGenerationChance = CalcEventGenerationChance();
        if (!eventIsActive)
        {
            var rng = Random.value;
            if (eventGenerationChance >= rng)
            {
                GenerateEvent();
            }
        }
    }

    private float CalcEventGenerationChance()
    {
        return 0;
    }

    private void GenerateEvent()
    {
        lastEventGeneratedTime = Time.time;
    }
    
}
