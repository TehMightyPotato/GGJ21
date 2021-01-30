using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Anliegen/Anliegen")]
public class Anliegen : ScriptableObject
{
    public Sprite sprite;
    public string message;
    public string question;
    public string sender;
    public string subject;
    public int timelimitsec;

    // Yay
    public string approveButtonText;
    public int approveCost;
    public int approveFollowerMod;
    public int approveInfluence;

    // Nay
    public string denyButtonText;
    public int denyCost;
    public int denyFollowerMod;
    public int denyInfluence;

    // Timey
    public int ignoreFollowerMod;
    public int ignoreInfluence;

    private Planet _planet;

    public void Init(Planet planet)
    {
        _planet = planet;
        _planet.StartCoroutine(PleaTimeout());
    }
    
    public void Approve()
    {
        Debug.Log("Approve");
        _planet.updateFollowerInfluence(approveFollowerMod, approveInfluence);
        GameManager.Instance.payGodPowerCost(approveCost);
    }

    public void Deny()
    {
        Debug.Log("Deny");
        _planet.updateFollowerInfluence(denyFollowerMod, denyInfluence);
        GameManager.Instance.payGodPowerCost(denyCost);
    }

    public void Fail()
    {
        Debug.Log("Failed!");
        _planet.updateFollowerInfluence(ignoreFollowerMod, ignoreInfluence);
    }

    public IEnumerator PleaTimeout()
    {
        yield return new WaitForSeconds(timelimitsec);
        Fail();
    }
    
}
