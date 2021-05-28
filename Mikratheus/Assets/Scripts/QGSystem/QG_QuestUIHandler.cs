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

        private QG_Quest curQuest;

        private Vector3 _origin = new Vector3(-1.8f, 0, 2);

        private float HORIZ_GAP = 0.7f;
        private float VERT_GAP = 0.7f;

        private Dictionary<QG_EventPool, Vector3> nodePosRegistry = new Dictionary<QG_EventPool, Vector3>();

        private void Awake()
        {
            Instance = this;
        }

        private void ClearPreviousDrawing()
        {
            foreach (Transform child in uiQuestPanel.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void DrawQuest(QG_Quest quest)
        {
            ClearPreviousDrawing();

            //Debug.Log("Begin Draw Quest");
            //Debug.Log(quest);

            curQuest = quest;

            int poolsCount = curQuest.eventPools.Count();

            HashSet<QG_EventPool> wavesAhead = new HashSet<QG_EventPool>(curQuest.eventPools);
            wavesAhead.Remove(curQuest.start);

            HashSet<QG_EventPool> wave = new HashSet<QG_EventPool>();
            wave.Add(curQuest.start);

            HashSet<QG_EventPool> wavesBehind = new HashSet<QG_EventPool>();

            int waveCount = 0;

            // ------------ node draw ------------

            while (wave.Count() != 0)
            {
                DrawNodesVert(waveCount, wave);

                HashSet<QG_EventPool> newWave = new HashSet<QG_EventPool>();

                foreach (QG_EventPool pool in wave)
                {

                    foreach (QG_Event event_ in pool.pool)
                    {
                        foreach (QG_EventPool p in event_.endingEventPools)
                        {
                            if (!wavesBehind.Contains(p) && !wave.Contains(p))
                            {
                                newWave.Add(p);
                            }
                        }
                    }

                }

                foreach (QG_EventPool p in newWave)
                    if (wavesAhead.Contains(p))
                        wavesAhead.Remove(p);

                foreach (QG_EventPool p in wave)
                    wavesBehind.Add(p);

                wave = newWave;

                waveCount++;
            }

            // ------------ arrow draw ------------

            DrawArrow(new Vector3(_origin.x - HORIZ_GAP, 0, 0), nodePosRegistry[curQuest.start], true);

            foreach (QG_EventPool p1 in curQuest.eventPools)
            {
                List<QG_EventPool> outPools = new List<QG_EventPool>();

                foreach (QG_Event event_ in p1.pool)
                {
                    foreach (QG_EventPool p2 in event_.endingEventPools)
                    {
                        if (!outPools.Contains(p2))
                        {
                            DrawArrow(nodePosRegistry[p1], nodePosRegistry[p2], p1.connUsed[p2]);
                            outPools.Add(p2);
                        }
                    }
                }
            }


    }

        private void DrawNodesVert(int horizDist, HashSet<QG_EventPool> nodes)
        {
            //Debug.Log(horizDist);

            float x = _origin.x + horizDist * HORIZ_GAP;

            int nodesCount = nodes.Count();

            if (nodesCount == 0)
                return;

            else if (nodesCount == 1)
                DrawNode(nodes.ElementAt(0), x, _origin.y, horizDist);

            else
            {
                float yOffset = - (horizDist - 1) * HORIZ_GAP / 2;

                for (int i = 0; i < nodesCount; i++)
                    DrawNode(nodes.ElementAt(i), x, _origin.y + yOffset + i * VERT_GAP, horizDist);
            }

        }

        private void DrawNode(QG_EventPool node, float x, float y, int n)
        {
            Vector3 pos = nodePosRegistry[node] = new Vector3(x, y, 0);

            GameObject newUINode = Instantiate(uiNodePrefab, uiQuestPanel.transform) as GameObject;
            newUINode.transform.Translate(pos);

            Color nodeColor;

            if (node.isActive())
                nodeColor = Color.green;
            else if (curQuest.poolsQueue.Contains(node))
                nodeColor = Color.yellow;
            else
                nodeColor = Color.gray;

            if (node.pool.Count() == 0)
                nodeColor = Color.black;

            if (node.used)
                newUINode.transform.Find("Inner").gameObject.SetActive(true);

            newUINode.GetComponent<Image>().color = nodeColor;
            newUINode.transform.Find("Text").gameObject.GetComponent<Text>().text = node.name_;
        }

        private void DrawArrow(Vector3 start, Vector3 end, bool used)
        {

            GameObject newUILine = Instantiate(uiLinePrefab, uiQuestPanel.transform) as GameObject;

            newUILine.transform.Translate(start);

            LineRenderer lineRenderer = newUILine.GetComponent<LineRenderer>();

            if (used)
            {
                lineRenderer.startColor = Color.gray;
                lineRenderer.endColor = Color.gray;
            }
            else
            {
                lineRenderer.startColor = Color.black;
                lineRenderer.endColor = Color.black;
            }

            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            //List<Vector3> pos = new List<Vector3>();
            //pos.Add(start);
            //pos.Add(end);
            //lineRenderer.SetPositions(pos.ToArray());
            lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
            lineRenderer.SetPosition(1, (end - start) * 100);
            lineRenderer.useWorldSpace = false;

        }

    }
}
