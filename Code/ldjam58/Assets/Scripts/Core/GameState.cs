using System.Collections.Generic;

using Assets.Scripts.Core.Model;

using GameFrame.Core.Definitions;

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

    }
}
