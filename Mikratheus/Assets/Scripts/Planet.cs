using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{

    private Animator _animator;
    public int totalPop;

    public int currentFollowers;

    // Zwischen 1 und 100
    public int influence;

    public float eventGenerationChance;

    public bool eventIsActive;

    public EventHandler EventGenerated;

    public Anliegen activeEvent;
    
    //Event generation
    [SerializeField] private float followerCountEventWeight;
    [SerializeField] private float timeEventWeight;
    [SerializeField] private float eventBaseProp = 0.3f;
    private float lastEventGeneratedTime;

    private void Awake()
    {
        influence = 50;
        _animator = GetComponent<Animator>();
    }
    
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
        activeEvent = PleaEventsLoader.Instance.GetRandomPlea();
        activeEvent.Init(this);
        eventIsActive = true;
        EventGenerated?.Invoke(this,EventArgs.Empty);
        lastEventGeneratedTime = Time.time;
    }

    public void SetActivePlanet(bool val)
    {
        _animator.SetBool("IsActivePlanet",val);
    }

    public void PlayEntryAnimation()
    {
        _animator.SetTrigger("EntryTrigger");
    }

    public void PlayExitAnimation()
    {
        _animator.SetTrigger("ExitTrigger");
    }
}