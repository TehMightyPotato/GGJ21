﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager Instance;
    public List<GameObject> planets = new List<GameObject>();
    public GameObject currentPlanet;

    public EventHandler<Planet> PlanetChanged;

    public List<GameObject> planetsToSpawn;
    public float timeToCheckForNewPlanetSpawn;
    private float _currentTimeSinceNewPlanetSpawnCheck;

    private void Awake()
    {
        _currentTimeSinceNewPlanetSpawnCheck = timeToCheckForNewPlanetSpawn;
        Instance = this;
    }

    private void Start()
    {
        currentPlanet = planets[0];
        currentPlanet.GetComponent<Planet>().SetActivePlanet(true);
    }

    private void FixedUpdate()
    {
        if (planetsToSpawn.Count <= 0) return;
        if (_currentTimeSinceNewPlanetSpawnCheck <= 0)
        {
            CheckForNewSpawn();
            _currentTimeSinceNewPlanetSpawnCheck = timeToCheckForNewPlanetSpawn;
        }
        _currentTimeSinceNewPlanetSpawnCheck -= Time.deltaTime;
    }


    public void NextPlanet()
    {
        var lastIndex = planets.IndexOf(currentPlanet);
        if (lastIndex == planets.Count - 1)
        {
            lastIndex = -1;
        }

        var oldPlanet = currentPlanet.GetComponent<Planet>();
        oldPlanet.PlayExitAnimation(true);
        oldPlanet.SetActivePlanet(false);
        currentPlanet = planets[lastIndex + 1];
        var newPlanet = currentPlanet.GetComponent<Planet>();
        newPlanet.SetActivePlanet(true);
        newPlanet.PlayEntryAnimation(true);
        OnPlanetChanged();
    }

    public void PreviousPlanet()
    {
        var lastIndex = planets.IndexOf(currentPlanet);
        if (lastIndex == 0)
        {
            lastIndex = planets.Count;
        }

        var oldPlanet = currentPlanet.GetComponent<Planet>();
        oldPlanet.PlayExitAnimation(false);
        oldPlanet.SetActivePlanet(false);
        currentPlanet = planets[lastIndex - 1];
        var newPlanet = currentPlanet.GetComponent<Planet>();
        newPlanet.SetActivePlanet(true);
        newPlanet.PlayEntryAnimation(false);
        OnPlanetChanged();
    }

    private void OnPlanetChanged()
    {
        PlanetChanged?.Invoke(this, currentPlanet.GetComponent<Planet>());
    }

    public void CheckForNewSpawn()
    {
        var totalFollowers = 0d;
        var totalPopulation = 0d;
        var totalInfluence = 0d;

        foreach (var planet in planets)
        {
            var actualPlanet = planet.GetComponent<Planet>();
            totalFollowers += actualPlanet.currentFollowers;
            totalPopulation += actualPlanet.totalPop;
            totalInfluence += actualPlanet.influence;
        }

        // ReSharper disable once IntDivisionByZero
        // ist safe weil totalPopulation nie = 0 
        var averageReputation = totalFollowers / totalPopulation;

        var averageInfluence = (0.5 - totalInfluence / (planets.Count * 100)) / 0.5;

        if (averageInfluence < 0)
        {
            averageInfluence *= -1;
        }

        var spawnNewPlanetProp = 0.6 * averageReputation + 0.3 * averageInfluence + 0.1 * (GameManager.Instance.godPower / 100f);
        if (spawnNewPlanetProp + Random.Range(0f, 0.9f) > 1f)
        {
            SpawnNewPlanet();
        }
    }

    private void SpawnNewPlanet()
    {
        var spawned = Instantiate(planetsToSpawn[0]);
        planetsToSpawn.RemoveAt(0);
        planets.Add(spawned);
    }
}