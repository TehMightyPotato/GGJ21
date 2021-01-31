using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PleaEventsLoader : MonoBehaviour
{
    public static PleaEventsLoader Instance;

    private Dictionary<string, Anliegen[]> _pleasDict;
    private Dictionary<string, Anliegen[]> _pleasStartDict;

    private void Awake()
    {
        Instance = this;
        _pleasDict = new Dictionary<string, Anliegen[]>();
        _pleasStartDict = new Dictionary<string, Anliegen[]>();
        _pleasDict["borea"] = Resources.LoadAll<Anliegen>("Anliegen/borea");
        _pleasStartDict["borea"] = Resources.LoadAll<Anliegen>("Anliegen/borea/start");
        _pleasDict["eco_827"] = Resources.LoadAll<Anliegen>("Anliegen/eco_827");
        _pleasStartDict["eco_827"] = Resources.LoadAll<Anliegen>("Anliegen/eco_827/start");
        _pleasDict["konteos"] = Resources.LoadAll<Anliegen>("Anliegen/konteos");
        _pleasStartDict["konteos"] = Resources.LoadAll<Anliegen>("Anliegen/konteos/start");
        _pleasDict["nobola"] = Resources.LoadAll<Anliegen>("Anliegen/nobola");
        _pleasStartDict["nobola"] = Resources.LoadAll<Anliegen>("Anliegen/nobola/start");
    }

    public Anliegen GetRandomPlea(string planet)
    {
        var pleasForPlanet = _pleasDict[planet];
        return pleasForPlanet[Random.Range(0, pleasForPlanet.Length)];
    }

    public Anliegen GetStartPleaForPlanet(string planet)
    {
        var pleas = _pleasStartDict[planet];
        //Obacht, wenn 0 dann peng!
        return pleas[0];
    }
}