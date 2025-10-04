using System;

namespace Assets.Scripts.Core.Persistence
{
    public class SavedGamePreview : GameFrame.Core.Persistence.SavedGamePreview<GameState>
    {
        public DateTime SavedOn { get; set; }
        public DateTime StartedOn { get; set; }
        public Double TimeElapsed { get; set; }

        public override void Init(GameState gameState, String key)
        {
            base.Init(gameState, key);

            this.SavedOn = gameState.SavedOn;
            this.StartedOn = gameState.CreatedOn;
            this.TimeElapsed = gameState.TimeElapsed;
        }
    }
}
