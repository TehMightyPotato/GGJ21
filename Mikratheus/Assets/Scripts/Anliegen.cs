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
    public string question;
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
}
