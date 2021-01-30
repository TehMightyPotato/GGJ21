using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PleaEventsLoader : MonoBehaviour
{
    public static PleaEventsLoader Instance;
    
    public Anliegen[] pleas;
    private void Awake()
    {
        Instance = this;
        pleas = Resources.LoadAll<Anliegen>("Anliegen");
    }

    public Anliegen GetRandomPlea()
    {
        return pleas[Random.Range(0, pleas.Length)];
    }
}
