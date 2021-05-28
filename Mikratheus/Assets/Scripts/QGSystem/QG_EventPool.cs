﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.QGSystem
{

    [CreateAssetMenu(menuName = "Anliegen/AnliegenMenge")]

    public class QG_EventPool : ScriptableObject
    {
        public List<QG_Event> pool;

        public int activeEvents = 0;

        public string name_;

        public bool used = false;
        public Dictionary<QG_EventPool, bool> connUsed = new Dictionary<QG_EventPool, bool>();

        public void init(QG_Quest quest)
        {
            used = false;
            activeEvents = 0;

            foreach (QG_Event e in pool)
            {
                e.quest = quest;

                foreach (QG_EventPool p in e.endingEventPools)
                    connUsed[p] = false;
            }
        }

        public bool isActive()
        {
            return activeEvents > 0;
        }
    }
}
