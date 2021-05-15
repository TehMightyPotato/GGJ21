using Assets.Scripts.QGSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class QG_Quest
    {
        public string name;

        public QG_EventPool start;

        public List<QG_EventPool> eventPools;

        public List<QG_EventPool> currentPoolsQueue;
        public HashSet<QG_Event> currentEvents;

        public QG_EventPool end;

        public QG_Quest(string name_, QG_EventPool start, List<QG_EventPool> eventPools)
        {
            name = name_;
            this.start = start;
            this.eventPools = eventPools;
            currentPoolsQueue = new List<QG_EventPool>();
            currentEvents = new HashSet<QG_Event>();

            currentPoolsQueue.Add(start);

            foreach (QG_EventPool p in this.eventPools)
            {
                foreach (QG_Event e in p.pool)
                {
                    e.quest = this;
                }
            }
        }

        public void EventUpdate(QG_Event event_, String ending)
        {
            event_.ending = ending;

            int endingIndex = event_.endings.FindIndex(str => str.Equals(ending));
            QG_EventPool newPool = event_.endingEventPools[endingIndex];
            newPool.isActive = true;

            currentPoolsQueue.Add(newPool);
        }

        // returns null if no event in pool
        public QG_Event NextEvent()
        {
            if (currentPoolsQueue.Count() == 0)
                return null;

            for (int i = 0; i < currentPoolsQueue.Count(); i++)
            {
                QG_EventPool curPool = currentPoolsQueue[i];

                int curPoolCount = curPool.pool.Count();

                if (curPoolCount == 0)
                    continue; // quest is ended

                currentPoolsQueue[i].isActive = false;
                currentPoolsQueue.RemoveAt(i);

                QG_Event nextEvent = curPool.pool[Random.Range(0, curPoolCount)];
                currentEvents.Add(nextEvent);
                QG_QuestUIHandler.Instance.DrawQuest(this); // --------------
                return nextEvent;
            }

            return null;
        }

    }
}
