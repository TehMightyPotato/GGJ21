using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUIHandler : MonoBehaviour
{
    public Text planetFollowerText;
    public Text planetTotalPopText;

    public GameObject speechBubble;

    private Planet _currentPlanet;
    
    private void Start()
    {
        PlanetManager.Instance.PlanetChanged += OnPlanetChanged;
        _currentPlanet = PlanetManager.Instance.currentPlanet.GetComponent<Planet>();
        OnPlanetChanged(this,_currentPlanet);
    }

    private void OnPlanetChanged(object sender, Planet planet)
    {
        _currentPlanet.EventGenerated -= OnEventGenerated;
        _currentPlanet = planet;
        planetFollowerText.text = _currentPlanet.currentFollowers.ToString();
        planetTotalPopText.text = _currentPlanet.totalPop.ToString();
        speechBubble.SetActive(_currentPlanet.eventIsActive);
        _currentPlanet.EventGenerated += OnEventGenerated;
    }

    private void OnEventGenerated(object sender, EventArgs e)
    {
        speechBubble.SetActive(_currentPlanet.eventIsActive);
    }
}
