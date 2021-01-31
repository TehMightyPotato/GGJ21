using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUIHandler : MonoBehaviour
{
    public Text planetFollowerText;
    public Text planetInfluenceText;
    public Image influenceImage;

    public GameObject speechBubble;

    private Planet _currentPlanet;

    private IEnumerator Start()
    { 
        while(PlanetManager.Instance.currentPlanet == null)
        {
            yield return new WaitForEndOfFrame();
        }
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
        planetFollowerText.text = _currentPlanet.currentFollowers.ToString() + "/" + _currentPlanet.totalPop.ToString();
        // planetInfluenceText.text = _currentPlanet.influence.ToString() + "/100";
        influenceImage.fillAmount = _currentPlanet.influence / 100f;
        if (influenceImage.fillAmount < 0.2f)
        {
            influenceImage.color = new Color(255, 0, 0);
        }

        else if (influenceImage.fillAmount > 0.8f)
        {
            influenceImage.color = new Color(255, 0, 0);
        }
        else
        {
            influenceImage.color = new Color(0, 255, 0);
        }
    }
}