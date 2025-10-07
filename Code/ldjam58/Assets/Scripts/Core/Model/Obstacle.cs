using Assets.Scripts.Core.Definitions;

using GameFrame.Core.Math;

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
