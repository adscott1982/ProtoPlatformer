using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class PlayerRecord
    {
        public PlayerRecord(float startDelay, List<TimePosition> timePositions)
        {
            this.StartDelay = startDelay;
            this.TimePositions = timePositions;
        }

        public float StartDelay { get; private set; }
        public List<TimePosition> TimePositions { get; private set; }
    }
}
