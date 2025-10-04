using Assets.Scripts.Core.Model;
using GameFrame.Core.Definitions;
using System;
using System.Collections.Generic;

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

    }
}
