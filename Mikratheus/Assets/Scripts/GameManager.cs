using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int updateRateInSeconds;
    [SerializeField] private float _currentUpdateTime;

    public Text gpText;  

    //Event Handler
    public UnityEvent planetUpdate;

    // Spielinfos
    [Header("Spielinformationen")] public int totalFollower;
    [Header("GodPower Settings")] public int godPower;
    public int godPowerBaseIncrease;
    public int godPowerLimit;
    public float godPowerIntervall;
    public float followerThreshold1;
    public int threshold1Increase;
    public float followerThreshold2;
    public int threshold2Increase;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(IncreaseGodPower());
    }

    private void FixedUpdate()
    {
        if (_currentUpdateTime <= 0)
        {
            planetUpdate.Invoke();
            _currentUpdateTime = updateRateInSeconds;
            return;
        }

        _currentUpdateTime -= Time.deltaTime;
        // Total Follower berechnen
        var planetList = PlanetManager.Instance.planets;
        int totalFollowerCount = 0;
        for (int i = 0; i < planetList.Count; i++)
        {
            totalFollowerCount += planetList[i].GetComponent<Planet>().currentFollowers;
        }

        totalFollower = totalFollowerCount;
    }

    public void PayGodPowerCost(int cost)
    {
        godPower -= cost;
        gpText.text = "Godpower/Max: " + godPower.ToString() + "/100";
    }

    public IEnumerator IncreaseGodPower()
    {
        while (true)
        {
            // Followeranteil pro Planet beeinflusst die Godpower
            var planetList = PlanetManager.Instance.planets;
            for (int i = 0; i < planetList.Count; i++)
            {
                Planet planetScript = planetList[i].GetComponent<Planet>();
                var anteilFollower = planetScript.currentFollowers / planetScript.totalPop;
                if (anteilFollower >= followerThreshold2)
                {
                    godPower += threshold2Increase;
                }
                else if (anteilFollower >= followerThreshold1)
                {
                    godPower += threshold1Increase;
                }
            }

            godPower += godPowerBaseIncrease;
            if (godPower > godPowerLimit)
            {
                godPower = godPowerLimit;
            }

            gpText.text = "Godpower/Max: " + godPower.ToString() + "/100";

            yield return new WaitForSeconds(godPowerIntervall);
        }
    }
}