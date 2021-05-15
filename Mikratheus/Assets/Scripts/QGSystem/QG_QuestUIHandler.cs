using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.QGSystem
{
    public class QG_QuestUIHandler : MonoBehaviour
    {
        public GameObject uiQuestPanel;
        public GameObject uiNode;

        private IEnumerator Start()
        {
            while (PlanetManager.Instance.currentPlanet == null)
            {
                yield return new WaitForEndOfFrame();
            }
            Planet _currentPlanet = PlanetManager.Instance.currentPlanet.GetComponent<Planet>();
            QG_Quest _currentQuest = PleaManager.Instance.questDict[_currentPlanet.name];

            GameObject newUINode = Instantiate(uiNode, uiQuestPanel.transform) as GameObject;
            newUINode.transform.Translate(-2, 0, 0);
            newUINode.GetComponent<Image>().color = new Color(0, 255, 0);

            Debug.Log("done");
        }

        private void DrawNode()
        {

            // start / invisible
            // active
            // inactive
            // end
        }

        private void DrawArrow()
        {

        }

    }
}
