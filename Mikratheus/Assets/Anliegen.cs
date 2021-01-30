using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Anliegen/Anliegen")]
public class Anliegen : ScriptableObject
{
    public string message;
    public int timelimitsec;

    // Yay
    public int approveCost;
    public int approveFollowerMod;

    // Nay
    public int denyCost;
    public int denyFollowerMod;

    // Timey
    public int ignoreFollowerMod;

}
