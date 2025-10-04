using GameFrame.Core.Definitions;
using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Definitons
{
    public class ObstaclePosDefinition : BaseDefinition
    {
        public ObstacleDefinition ObstacleDefinition { get; set; }
        public Vector2 Position { get; set; }
    }
}
