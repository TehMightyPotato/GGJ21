using Assets.Scripts;
using Assets.Scripts.QGSystem;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PleaManager : MonoBehaviour
{

    public static PleaManager Instance;

    private Dictionary<string, Plea[]> _pleasDict;
    private Dictionary<string, Plea[]> _pleasStartDict;

    public Dictionary<string, QG_Quest> questDict;

    private void Awake()
    {
        Instance = this;
        _pleasDict = new Dictionary<string, Plea[]>();
        _pleasStartDict = new Dictionary<string, Plea[]>();

        questDict = new Dictionary<string, QG_Quest>();

        string[] planetNames = { "konteos", "borea", "eco_827", "nobola" };

        questDict["konteos"] = new QG_Quest(
            "konteos",
            Resources.Load<QG_EventPool>("Anliegen/konteos/stages/kt_s1_start"),
            new List<QG_EventPool>(Resources.LoadAll<QG_EventPool>("Anliegen/konteos/stages"))
        );

        questDict["borea"] = new QG_Quest(
            "borea",
            Resources.Load<QG_EventPool>("Anliegen/borea/stages/br_s1_start"),
            new List<QG_EventPool>(Resources.LoadAll<QG_EventPool>("Anliegen/borea/stages"))
        );

        questDict["eco_827"] = new QG_Quest(
            "eco_827",
            Resources.Load<QG_EventPool>("Anliegen/eco_827/stages/ec_s1_start"),
            new List<QG_EventPool>(Resources.LoadAll<QG_EventPool>("Anliegen/eco_827/stages"))
        );

        questDict["nobola"] = new QG_Quest(
            "nobola",
            Resources.Load<QG_EventPool>("Anliegen/nobola/stages/nb_s1_start"),
            new List<QG_EventPool>(Resources.LoadAll<QG_EventPool>("Anliegen/nobola/stages"))
        );

        //_pleasDict["borea"] = Resources.LoadAll<Plea>("Anliegen/borea");
        //_pleasStartDict["borea"] = Resources.LoadAll<Plea>("Anliegen/borea/start");

        //_pleasDict["eco_827"] = Resources.LoadAll<Plea>("Anliegen/eco_827");
        //_pleasStartDict["eco_827"] = Resources.LoadAll<Plea>("Anliegen/eco_827/start");

        //_pleasDict["konteos"] = Resources.LoadAll<Plea>("Anliegen/konteos");
        //_pleasStartDict["konteos"] = Resources.LoadAll<Plea>("Anliegen/konteos/start");

        //_pleasDict["nobola"] = Resources.LoadAll<Plea>("Anliegen/nobola");
        //_pleasStartDict["nobola"] = Resources.LoadAll<Plea>("Anliegen/nobola/start");
    }

    public Plea GetRandomPlea(string planet)
    {
        if (questDict.ContainsKey(planet))
            return (Plea)questDict[planet].NextEvent();

        var pleasForPlanet = _pleasDict[planet];
        return pleasForPlanet[Random.Range(0, pleasForPlanet.Length)];
    }

    public Plea GetStartPleaForPlanet(string planet)
    {
        if (questDict.ContainsKey(planet))
            return (Plea)questDict[planet].NextEvent();

        var pleas = _pleasStartDict[planet];
        //Obacht, wenn 0 dann peng!
        return pleas[0];
    }
}