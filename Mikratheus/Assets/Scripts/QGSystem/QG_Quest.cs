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
        public string _name;

        private QG_EventPool _start;

        private List<QG_EventPool> _eventPools;

        private QG_EventPool _currentPool;
        private QG_Event _currentEvent;

        private QG_EventPool _end;

        public QG_Quest(string name, QG_EventPool start, List<QG_EventPool> eventPools)
        {
            _name = name;

            _eventPools = eventPools;
            _currentPool = _start = start;

            foreach (QG_EventPool p in _eventPools)
            {
                foreach (QG_Event e in p.pool)
                {
                    e._quest = this;
                }
            }
        }

        public void EventUpdate(String ending)
        {
            _currentEvent.ending = ending;
            int endingIndex = _currentEvent.endings.FindIndex(str => str.Equals(ending));
            _currentPool = _currentEvent.endingEventPools[endingIndex];
            _currentEvent = null;
        }

        // returns null if no event in pool
        public QG_Event NextEvent()
        {
            Debug.Log(_name);
            int curPoolCount = _currentPool.pool.Count();

            if (curPoolCount == 0)
                return null;

            _currentEvent = _currentPool.pool[Random.Range(0, curPoolCount)];
            return _currentEvent;
        }

    }
}
