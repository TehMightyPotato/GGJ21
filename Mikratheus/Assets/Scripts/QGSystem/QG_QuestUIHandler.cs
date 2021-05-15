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
        public static QG_QuestUIHandler Instance;

        public GameObject uiQuestPanel;
        public GameObject uiNodePrefab;
        public GameObject uiLinePrefab;

        private Vector3 _origin = new Vector3(-2, 0, 0);

        private Dictionary<QG_EventPool, Vector3> nodePosRegistry = new Dictionary<QG_EventPool, Vector3>();

        private void Awake()
        {
            Instance = this;
        }

        public void DrawQuest(QG_Quest quest)
        {
            int poolsCount = quest.eventPools.Count();

            HashSet<QG_EventPool> wavesAhead = new HashSet<QG_EventPool>(quest.eventPools);
            wavesAhead.Remove(quest.start);

            HashSet<QG_EventPool> wave = new HashSet<QG_EventPool>();
            wave.Add(quest.start);

            HashSet<QG_EventPool> waveBehind = new HashSet<QG_EventPool>();

            int waveCount = 0;

            while (wave.Count() != 0)
            {
                HashSet<QG_EventPool> newWave = new HashSet<QG_EventPool>();

                DrawNodesVert(waveCount, wave);

                foreach (QG_EventPool pool in wave)
                {
                    foreach (QG_Event event_ in pool.pool)
                    {
                        foreach (QG_EventPool p in event_.endingEventPools)
                            if (!waveBehind.Contains(p))
                                newWave.Add(p);
                    }
                }

                foreach (QG_EventPool p in newWave)
                    if (wavesAhead.Contains(p))
                        wavesAhead.Remove(p);

                foreach (QG_EventPool p in wave)
                    waveBehind.Add(p);

                wave = newWave;

                waveCount++;
            }

        }

        private void DrawNodesVert(int horizDist, HashSet<QG_EventPool> nodes)
        {
            Debug.Log(horizDist);

            float HORIZ_GAP = 0.7f;
            float VERT_GAP = 0.7f;

            float x = _origin.x + horizDist * HORIZ_GAP;

            int nodesCount = nodes.Count();

            if (nodesCount == 0)
                return;

            if (nodesCount == 1)
                DrawNode(nodes.ElementAt(0), x, _origin.y, horizDist);

            else if (nodesCount % 2 == 1)
            {
                int i;

                for (i = 0; i < (nodesCount - 1) / 2 - 1; i++)
                {
                    DrawNode(nodes.ElementAt(i), x, _origin.y + (i + 1) * VERT_GAP, horizDist);
                }

                i++;
                DrawNode(nodes.ElementAt(i), x, _origin.y, nodesCount);

                for (i = i + 1; i < nodesCount - 1; i++)
                {
                    DrawNode(nodes.ElementAt(i), x, _origin.y - (i + 1) * VERT_GAP, horizDist);
                }
            }

            else
            {
                int i;

                for (i = 0; i < nodesCount / 2 - 1; i++)
                {
                    DrawNode(nodes.ElementAt(i), x, _origin.y + (i + 1) * VERT_GAP, horizDist);
                }

                for (i = i + 1; i < nodesCount - 1; i++)
                {
                    DrawNode(nodes.ElementAt(i), x, _origin.y - (i + 1) * VERT_GAP, horizDist);
                }
            }

        }

        private void DrawNode(QG_EventPool node, float x, float y, int n)
        {
            Vector3 pos = nodePosRegistry[node] = new Vector3(x, y, 0);

            GameObject newUINode = Instantiate(uiNodePrefab, uiQuestPanel.transform) as GameObject;
            newUINode.transform.Translate(pos);

            newUINode.GetComponent<Image>().color = node.isActive ? Color.yellow : Color.gray;
            newUINode.GetComponent<TextMesh>().text = n.ToString();

            // start / invisible
            // active
            // inactive
            // end
        }

        private void DrawArrow(Vector3 start, Vector3 end)
        {

            GameObject newUILine = Instantiate(uiLinePrefab, uiQuestPanel.transform) as GameObject;
            LineRenderer lineRenderer = newUILine.GetComponent<LineRenderer>();
            lineRenderer.startColor = Color.gray;
            lineRenderer.endColor = Color.black;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            lineRenderer.useWorldSpace = true;

        }

    }
}
