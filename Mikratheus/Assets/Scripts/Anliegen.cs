using System;
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

    private Coroutine _timeoutRoutine;

    public EventHandler PleaComplete;

    private Planet _planet;

    public void Init(Planet planet)
    {
        _planet = planet;
        _timeoutRoutine = _planet.StartCoroutine(PleaTimeout());
    }
    
    public void Approve()
    {
        Debug.Log("Approve",_planet);
        _planet.StopCoroutine(_timeoutRoutine);
        _planet.updateFollowerInfluence(approveFollowerMod, approveInfluence);
        GameManager.Instance.payGodPowerCost(approveCost);
        _planet.RemoveEvent();
        OnPleaComplete();
    }

    public void Deny()
    {
        Debug.Log("Deny",_planet);
        _planet.StopCoroutine(_timeoutRoutine);
        _planet.updateFollowerInfluence(denyFollowerMod, denyInfluence);
        GameManager.Instance.payGodPowerCost(denyCost);
        _planet.RemoveEvent();
        OnPleaComplete();
    }

    public void Fail()
    {
        Debug.Log("Failed!",_planet);
        _planet.updateFollowerInfluence(ignoreFollowerMod, ignoreInfluence);
        _planet.RemoveEvent();
        OnPleaComplete();
    }

    private void OnPleaComplete()
    {
        PleaComplete?.Invoke(this,EventArgs.Empty);
    }

    public IEnumerator PleaTimeout()
    {
        yield return new WaitForSeconds(timelimitsec);
        Fail();
    }
    
}
