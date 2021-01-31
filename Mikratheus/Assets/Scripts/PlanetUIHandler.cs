using System;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUIHandler : MonoBehaviour
{
    public Text planetNameText;
    public Text planetFollowerText;
    public Text planetInfluenceText;

    public GameObject speechBubble;

    private Planet _currentPlanet;

    private void Start()
    {
        PlanetManager.Instance.PlanetChanged += OnPlanetChanged;
        _currentPlanet = PlanetManager.Instance.currentPlanet.GetComponent<Planet>();
        OnPlanetChanged(this, _currentPlanet);
    }

    private void OnPlanetChanged(object sender, Planet planet)
    {
        _currentPlanet.EventStatusChanged -= OnPlanetEventStatusChanged;
        _currentPlanet.PlanetValuesUpdate -= OnPlanetValueUpdate;
        _currentPlanet = planet;

        UpdatePlanetStatUI();

        speechBubble.SetActive(_currentPlanet.eventIsActive);
        _currentPlanet.EventStatusChanged += OnPlanetEventStatusChanged;
        _currentPlanet.PlanetValuesUpdate += OnPlanetValueUpdate;
    }

    private void OnPlanetValueUpdate(object sender, EventArgs e)
    {
        UpdatePlanetStatUI();
    }

    private void OnPlanetEventStatusChanged(object sender, EventArgs e)
    {
        speechBubble.SetActive(_currentPlanet.eventIsActive);
    }


    private void UpdatePlanetStatUI()
    {
        planetNameText.text = "Current Planet: " + _currentPlanet.planetName;
        planetFollowerText.text = "Follower/Population: " + _currentPlanet.currentFollowers.ToString() + "/" + _currentPlanet.totalPop.ToString();
        planetInfluenceText.text = "Influence/Maximum: " + _currentPlanet.influence.ToString() + "/100";
    }
}