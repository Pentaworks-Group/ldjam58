using Assets.Scripts.Core.Definitons;
using GameFrame.Core.Math;
using System;

namespace Assets.Scripts.Core.Model
{
    public class Obstacle
    {
        public ObstacleDefinition Definition { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Size { get; set; }
    }
}
