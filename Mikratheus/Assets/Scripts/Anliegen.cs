using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Anliegen/Anliegen")]
public class Anliegen : ScriptableObject
{
    public Sprite sprite;
    public AudioClip audioClip;
    [TextArea] public string message;
    [TextArea] public string question;
    public string sender;
    public string subject;
    public int timelimitsec;

    // Yay
    [TextArea] public string approveButtonText;
    public int approveCost;
    public int approveFollowerMod;
    public int approveInfluence;

    // Nay
    [TextArea] public string denyButtonText;
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
        if (GameManager.Instance.godPower < Math.Abs(approveCost))
        {
            return;
        }

        Debug.Log("Approve", _planet);
        _planet.StopCoroutine(_timeoutRoutine);
        _planet.UpdateFollowerInfluence(approveFollowerMod, approveInfluence);
        GameManager.Instance.PayGodPowerCost(approveCost);
        _planet.RemoveEvent();
        OnPleaComplete();
    }

    public void Deny()
    {
        if (GameManager.Instance.godPower < Math.Abs(denyCost))
        {
            return;
        }

        Debug.Log("Deny", _planet);
        _planet.StopCoroutine(_timeoutRoutine);
        _planet.UpdateFollowerInfluence(denyFollowerMod, denyInfluence);
        GameManager.Instance.PayGodPowerCost(denyCost);
        _planet.RemoveEvent();
        OnPleaComplete();
    }

    public void Fail()
    {
        Debug.Log("Failed!", _planet);
        _planet.UpdateFollowerInfluence(ignoreFollowerMod, ignoreInfluence);
        _planet.RemoveEvent();
        OnPleaComplete();
    }

    private void OnPleaComplete()
    {
        PleaComplete?.Invoke(this, EventArgs.Empty);
    }

    public IEnumerator PleaTimeout()
    {
        yield return new WaitForSeconds(timelimitsec);
        Fail();
    }
}