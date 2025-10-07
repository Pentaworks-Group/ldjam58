using System;

using Assets.Scripts.Core.Definitions;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Model
{
    public class Food
    {
        public Guid ID { get; set; }
        public FoodDefinition Definition { get; set; }
        public Vector3 Position { get; set; }
        public Int32 Score { get; set; }
    }
}
