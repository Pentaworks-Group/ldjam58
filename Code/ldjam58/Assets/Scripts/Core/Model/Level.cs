using GameFrame.Core.Math;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Model
{
    public class Level
    {
        public String Name { get; set; }
        public Single? Seed { get; set; }
        public Int32? Width { get; set; }
        public Int32? Height { get; set; }
        public Vector2 PinguinStartPosition { get; set; }
        public List<Food> Foods { get; set; }
        public List<Obstacle> Obstacles { get; set; }
    }
}
