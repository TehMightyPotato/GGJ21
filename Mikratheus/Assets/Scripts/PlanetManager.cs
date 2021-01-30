using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
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
        currentPlanet = planets[0];
        currentPlanet.GetComponent<Planet>().SetActivePlanet(true);
        for (int i = 1; i < planets.Count; i++)
        {
            planets[i].GetComponent<Planet>().SetActivePlanet(false);
        }
    }

    private void FixedUpdate()
    {
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
        oldPlanet.SetActivePlanet(false);
        oldPlanet.PlayExitAnimation();
        currentPlanet = planets[lastIndex + 1];
        var newPlanet = currentPlanet.GetComponent<Planet>();
        newPlanet.SetActivePlanet(true);
        newPlanet.PlayEntryAnimation();
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
        oldPlanet.SetActivePlanet(false);
        oldPlanet.PlayExitAnimation();
        currentPlanet = planets[lastIndex - 1];
        var newPlanet = currentPlanet.GetComponent<Planet>();
        newPlanet.SetActivePlanet(true);
        newPlanet.PlayEntryAnimation();
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

        var averageInfluence = (0.5 - totalInfluence / planets.Count) / 0.5;

        if (averageInfluence < 0)
        {
            averageInfluence *= -1;
        }

        var spawnNewPlanetProp = 0.6 * averageReputation + 0.3 * averageInfluence + 0.1 * GameManager.Instance.godPower;
        if (spawnNewPlanetProp + Random.Range(0f, 0.51f) > 1f)
        {
            Debug.Log("Spawned new Planet");
            SpawnNewPlanet();
            return;
        }

        Debug.Log("Did not spawn new Planet");
    }

    private void SpawnNewPlanet()
    {
        var spawned = Instantiate(planetsToSpawn[0]);
        planetsToSpawn.RemoveAt(0);
        planets.Add(spawned);
    }
}