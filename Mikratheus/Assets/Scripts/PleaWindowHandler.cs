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
    public Text pleaApproveButtonText;
    public Text pleaDenyButtonText;
    
    public void OpenPleaPanel()
    {
        var plea = PlanetManager.Instance.currentPlanet.GetComponent<Planet>().activeEvent;
        pleaImage.sprite = plea.sprite;
        pleaText.text = plea.message;
        pleaQuestionText.text = plea.question;
        pleaHeaderFromText.text = plea.sender;
        pleaHeaderSubjectText.text = plea.subject;
        pleaApproveButtonText.text = plea.approveButtonText;
        pleaDenyButtonText.text = plea.denyButtonText;
        pleaPanel.SetActive(true);
    }

    public void ClosePleaPanel()
    {
        pleaPanel.SetActive(false);
    }
}
