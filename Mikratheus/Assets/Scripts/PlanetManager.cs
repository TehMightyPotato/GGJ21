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
        currentPlanet.GetComponent<Planet>().SetActivePlanet(true);
        for (int i = 1; i < planets.Count; i++)
        {
            planets[i].GetComponent<Planet>().SetActivePlanet(false);
        }
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
        PlanetChanged?.Invoke(this,currentPlanet.GetComponent<Planet>());
    }
}