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
    
    public void OpenPleaPanel()
    {
        var plea = PlanetManager.Instance.currentPlanet.GetComponent<Planet>().activeEvent;
        pleaImage.sprite = plea.sprite;
        pleaText.text = plea.message;
        pleaQuestionText.text = plea.question;
        pleaHeaderFromText.text = plea.sender;
        pleaHeaderSubjectText.text = plea.subject;
        pleaApproveButton.onClick.RemoveAllListeners();
        pleaApproveButton.onClick.AddListener(plea.Approve);
        pleaApproveButtonText.text = plea.approveButtonText;
        pleaDenyButton.onClick.RemoveAllListeners();
        pleaDenyButton.onClick.AddListener(plea.Deny);
        pleaDenyButtonText.text = plea.denyButtonText;
        pleaPanel.SetActive(true);
    }

    public void ClosePleaPanel()
    {
        pleaPanel.SetActive(false);
    }
}
