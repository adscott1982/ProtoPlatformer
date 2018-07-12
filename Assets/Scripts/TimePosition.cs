using System;
using UnityEngine;

namespace Assets.Scripts
{
    public struct TimePosition
    {
        public TimePosition(float seconds, Vector2 position)
        {
            this.Seconds = seconds;
            this.Position = position;
        }

        public float Seconds { get; private set; }

        public Vector2 Position { get; private set;  }
    }
}
