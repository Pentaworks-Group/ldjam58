using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Model;
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

        public Pinguin Pinguin { get; set; }
        public List<Level> Levels { get; set; }
        public Level CurrentLevel { get; set; }
        public List<Food> Foods { get; set; }
        public List<Obstacle> Obstacles { get; set; }
    }
}
