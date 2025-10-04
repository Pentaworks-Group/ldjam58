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

        private World world;
        public World World
        {
            get
            {
                return world;
            }
            set
            {
                if (world != value)
                {
                    world = value;
                }
            }
        }
    }
}
