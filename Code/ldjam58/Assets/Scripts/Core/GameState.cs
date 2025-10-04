using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Pinguin Pinguin { get; set; }
        public List<Level> Levels { get; set; }
        public Level CurrentLevel { get; set; }
        public List<Food> Foods { get; set; }
        public List<Obstacle> Obstacles { get; set; }

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
