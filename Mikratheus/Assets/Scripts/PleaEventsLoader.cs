using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PleaEventsLoader : MonoBehaviour
{
    public static PleaEventsLoader Instance;

    public Dictionary<string, Anliegen[]> pleasDict;
    
    private void Awake()
    {
        Instance = this;
        pleasDict = new Dictionary<string, Anliegen[]>();
        pleasDict["borea"] = Resources.LoadAll<Anliegen>("Anliegen/borea");
        pleasDict["eco_827"] = Resources.LoadAll<Anliegen>("Anliegen/eco_827");
        pleasDict["konteos"] = Resources.LoadAll<Anliegen>("Anliegen/konteos");
        pleasDict["nobola"] = Resources.LoadAll<Anliegen>("Anliegen/nobola");
    }

    public Anliegen GetRandomPlea(string planet)
    {
        var pleasForPlanet = pleasDict[planet];
        return pleasForPlanet[Random.Range(0, pleasForPlanet.Length)];
    }
}
