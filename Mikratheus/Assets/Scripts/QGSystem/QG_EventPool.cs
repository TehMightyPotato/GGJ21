using System;
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

        public bool isActive()
        {
            return activeEvents > 0;
        }
    }
}
