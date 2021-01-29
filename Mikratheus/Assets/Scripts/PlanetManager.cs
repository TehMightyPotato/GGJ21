using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager Instance;
    public List<GameObject> planets = new List<GameObject>();
    public GameObject currentPlanet;

    public EventHandler<Planet> PlanetChanged;
    
    private void Awake()
    {
        Instance = this;
        currentPlanet = planets[0];
    }

    public void NextPlanet()
    {
        var lastIndex = planets.IndexOf(currentPlanet);
        if (lastIndex == planets.Count - 1)
        {
            lastIndex = -1;
        }

        currentPlanet.SetActive(false);
        currentPlanet = planets[lastIndex + 1];
        currentPlanet.SetActive(true);
        OnPlanetChanged();
    }

    public void PreviousPlanet()
    {
        var lastIndex = planets.IndexOf(currentPlanet);
        if (lastIndex == 0)
        {
            lastIndex = planets.Count;
        }

        currentPlanet.SetActive(false);
        currentPlanet = planets[lastIndex - 1];
        currentPlanet.SetActive(true);
        OnPlanetChanged();
    }

    private void OnPlanetChanged()
    {
        PlanetChanged?.Invoke(this,currentPlanet.GetComponent<Planet>());
    }
}