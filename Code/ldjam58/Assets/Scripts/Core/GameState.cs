using System;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Model;

namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        private GameMode mode;
        public GameMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                if (mode != value)
                {
                    mode = value;
                }
            }
        }

        public Penguin Penguin { get; set; }
        public Level CurrentLevel { get; set; }

        public Double TimeElapsed { get; set; } = 0.0;
        public String DeathReason { get; set; }
    }
}
