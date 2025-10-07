using System;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitions
{
    public class PenguinDefinition : BaseDefinition
    {
        public String Name { get; set; }
        public String Sprite { get; set; }
        public Single? MaxStrength { get; set; }
    }
}
