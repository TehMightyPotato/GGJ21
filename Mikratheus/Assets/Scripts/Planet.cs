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

    public int followerGrowthIntervall;

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
        StartCoroutine(FollowerGrowth());
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
        activeEvent = Instantiate(PleaEventsLoader.Instance.GetRandomPlea());
        activeEvent.Init(this);
        eventIsActive = true;
        EventGenerated?.Invoke(this,EventArgs.Empty);
        lastEventGeneratedTime = Time.time;
    }

    public void updateFollowerInfluence(int followerMod, int influenceMod)
    {
        // Follower updaten
        if (currentFollowers + followerMod > totalPop)
        {
            currentFollowers = totalPop;
        }
        else if (currentFollowers + followerMod < 0)
        {
            currentFollowers = 0;
        }
        else
        {
            currentFollowers += followerMod;
        }
        // Influence updaten
        if (influence + influenceMod > 100)
        {
            influence = 100;
        }
        else if (influence + influenceMod < 0)
        {
            influence = 0;
        }
        else
        {
            influence += influenceMod;
        }
    }

    public IEnumerator FollowerGrowth()
    {
        while (true)
        {
            var maxIncrease = 0.05f * totalPop;
            float increase = 0;

            if (influence < 81 && influence >= 20)
            {
                increase = (maxIncrease / 900) * Mathf.Pow(influence, 2) - 100 * influence + 1600;
            }
            else if (influence < 20)
            {
                increase = (((20-influence)*5) / 100f) * -maxIncrease;
            }
            else if (influence > 80)
            {
                increase = (((influence - 80)*5) / 100f) * -maxIncrease;
            }

            currentFollowers += (int)increase;
            if (currentFollowers > totalPop)
            {
                currentFollowers = totalPop;
            }
            else if (currentFollowers < 0)
            {
                currentFollowers = 0;
            }

            yield return new WaitForSeconds(followerGrowthIntervall);
        }
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