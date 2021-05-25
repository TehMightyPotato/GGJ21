using Assets.Scripts.QGSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    [FormerlySerializedAs("name")] public string planetName;
    public AudioClip pleaAudioClip;
    private Animator _animator;
    public int totalPop;

    public int currentFollowers;

    // Zwischen 1 und 100
    public int influence;

    public int followerGrowthIntervall;

    public float eventGenerationChance;

    public bool eventIsActive;


    public EventHandler EventStatusChanged;
    public EventHandler PlanetValuesUpdate;

    public Plea activeEvent;

    [SerializeField] private float growthFactor;

    //Event generation
    [SerializeField] private float followerCountEventWeight;
    [SerializeField] private float timeEventWeight;
    [SerializeField] private float eventBaseProp = 0.3f;
    private float _lastEventGeneratedTime;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.planetUpdate.AddListener(UpdatePlanet);
        StartCoroutine(FollowerGrowth());
        GenerateEvent(PleaManager.Instance.GetStartPleaForPlanet(planetName));
    }

    private void UpdatePlanet()
    {
        eventGenerationChance = CalcEventGenerationChance();
        if (!eventIsActive)
        {
            var rng = Random.value;
            if (eventGenerationChance >= rng)
            {
                GenerateEvent(PleaManager.Instance.GetRandomPlea(planetName));
            }
        }
    }

    private float CalcEventGenerationChance()
    {
        var chanceLastEvent = 0f;
        if (Time.time - _lastEventGeneratedTime <= 600)
        {
            chanceLastEvent = (Time.time - _lastEventGeneratedTime) / 600;
        }
        else
        {
            chanceLastEvent = 1;
        }

        return eventBaseProp + (1 - eventBaseProp) * (followerCountEventWeight * (currentFollowers / totalPop) +
                                                      timeEventWeight * (chanceLastEvent));
    }

    private void GenerateEvent(Plea plea)
    {
        if (plea == null)
        {
            Debug.Log("quest ended");
            return;
        }

        activeEvent = plea;
        activeEvent.Init(this);

        AudioManager.Instance.PlayAudioClip(pleaAudioClip);
        eventIsActive = true;
        EventStatusChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateFollowerInfluence(int followerMod, int influenceMod)
    {
        // Follower updaten
        if (currentFollowers + followerMod > totalPop)
        {
            currentFollowers = totalPop;
        }
        else if (currentFollowers + followerMod < 1)
        {
            currentFollowers = 1;
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
            var maxIncrease = growthFactor * totalPop * (1 + 3 * (float) currentFollowers / (float) totalPop);
            float increase = 0;

            if (influence < 81 && influence >= 20)
            {
                increase = Mathf.Clamp(((-maxIncrease / 900) * (Mathf.Pow(influence, 2) - 100 * influence + 1600)), 0,
                    currentFollowers);
            }
            else if (influence < 20)
            {
                increase = (((20 - influence) * 5) / 100f) * -maxIncrease;
            }
            else //if (influence > 80)
            {
                increase = (((influence - 80) * 5) / 100f) * -maxIncrease;
            }

            if (totalPop == 564213)
            {
                Debug.Log(increase);
            }

            currentFollowers += (int) increase;
            if (currentFollowers > totalPop)
            {
                currentFollowers = totalPop;
            }
            else if (currentFollowers < 1)
            {
                currentFollowers = 1;
            }

            PlanetValuesUpdate?.Invoke(this, EventArgs.Empty);
            yield return new WaitForSeconds(followerGrowthIntervall);
        }
    }

    public void SetActivePlanet(bool isActive)
    {
        _animator.SetBool("IsActivePlanet", isActive);

        if (isActive)
        {
            QG_QuestUIHandler.Instance.DrawQuest(
                PleaManager.Instance.questDict[planetName]);
        }
    }

    public void PlayEntryAnimation(bool next)
    {
        _animator.SetTrigger(next ? "EntryTriggerNext" : "EntryTriggerPrev");
    }

    public void PlayExitAnimation(bool next)
    {
        _animator.SetTrigger(next ? "ExitTriggerNext" : "ExitTriggerPrev");
    }

    public void RemoveEvent()
    {
        activeEvent = null;
        eventIsActive = false;
        _lastEventGeneratedTime = Time.time;
        EventStatusChanged?.Invoke(this, EventArgs.Empty);
    }
}