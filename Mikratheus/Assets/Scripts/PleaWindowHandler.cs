using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PleaWindowHandler : MonoBehaviour
{
    public GameObject pleaPanel;
    public Image pleaImage;
    public Text pleaText;
    public Text pleaQuestionText;
    public Text pleaHeaderFromText;
    public Text pleaHeaderSubjectText;
    public Button pleaApproveButton;
    public Text pleaApproveButtonText;
    public Button pleaDenyButton;
    public Text pleaDenyButtonText;

    private Anliegen _currentPlea;
    
    public void OpenPleaPanel()
    {
        if (_currentPlea != null)
        {
            _currentPlea.PleaComplete -= OnPleaComplete;
        }
        _currentPlea = PlanetManager.Instance.currentPlanet.GetComponent<Planet>().activeEvent;
        pleaImage.sprite = _currentPlea.sprite;
        pleaText.text = _currentPlea.message;
        pleaQuestionText.text = _currentPlea.question;
        pleaHeaderFromText.text = _currentPlea.sender;
        pleaHeaderSubjectText.text = _currentPlea.subject;
        pleaApproveButton.onClick.RemoveAllListeners();
        pleaApproveButton.onClick.AddListener(_currentPlea.Approve);
        pleaApproveButtonText.text = _currentPlea.approveButtonText;
        pleaDenyButton.onClick.RemoveAllListeners();
        pleaDenyButton.onClick.AddListener(_currentPlea.Deny);
        pleaDenyButtonText.text = _currentPlea.denyButtonText;
        pleaPanel.SetActive(true);
        _currentPlea.PleaComplete += OnPleaComplete;
    }

    public void ClosePleaPanel()
    {
        pleaPanel.SetActive(false);
    }

    private void OnPleaComplete(object sender, EventArgs e)
    {
        ClosePleaPanel();
    }
}
