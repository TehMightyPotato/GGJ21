using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    public int totalPop;

    public int currentFollowers;

    public float eventGenerationChance;

    public bool eventIsActive;

    public EventHandler EventGenerated;

    public Anliegen activeEvent;
    
    //Event generation
    [SerializeField] private float followerCountEventWeight;
    [SerializeField] private float timeEventWeight;
    [SerializeField] private float eventBaseProp = 0.3f;
    private float lastEventGeneratedTime;

    private void Start()
    {
        GameManager.Instance.planetUpdate.AddListener(UpdatePlanet);
    }

    private void UpdatePlanet()
    {
        eventGenerationChance = CalcEventGenerationChance();
        if (!eventIsActive)
        {
            var rng = Random.value;
            if (eventGenerationChance >= rng)
            {
                Debug.Log("Generated");
                GenerateEvent();
            }
        }
    }

    private float CalcEventGenerationChance()
    {
        var chanceLastEvent = 0f;
        if (Time.time - lastEventGeneratedTime <= 600)
        {
            chanceLastEvent = (Time.time - lastEventGeneratedTime) / 600;
        }
        else
        {
            chanceLastEvent = 1;
        }

        return eventBaseProp + (1 - eventBaseProp) * (followerCountEventWeight * (currentFollowers / totalPop) +
                                                      timeEventWeight * (chanceLastEvent));
    }

    private void GenerateEvent()
    {
        eventIsActive = true;
        EventGenerated?.Invoke(this,EventArgs.Empty);
        lastEventGeneratedTime = Time.time;
    }
}