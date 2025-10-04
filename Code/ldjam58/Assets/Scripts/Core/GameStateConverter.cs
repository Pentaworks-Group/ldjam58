using System;

using Assets.Scripts.Core.Model;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core
{
    public class GameStateConverter
    {
        private readonly GameMode mode;

        public GameStateConverter(GameMode gameMode)
        {
            this.mode = gameMode;
        }

        public GameState Convert()
        {
            var gameState = new GameState
            {
                CreatedOn = DateTime.Now,
                CurrentScene = Constants.Scenes.GameName,
                Mode = mode
            };

            gameState.World = GenerateWorld();

            return gameState;
        }

        private World GenerateWorld()
        {
            var world = new World()
            {
                Size = new GameFrame.Core.Math.Vector2(16, 16),
                Resolution = 16,
                Chunks = new System.Collections.Generic.List<WorldChunk>()
                {
                    new WorldChunk()
                    {
                        Position = new GameFrame.Core.Math.Vector2Int(2, 2),
                        DefaultTileHeight = 1,
                        Tiles = new System.Collections.Generic.List<WorldTile>()
                        {
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(5, 2, 5)
                            },
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(5, 2, 6)
                            },
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(6, 2, 6)
                            },
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(6, 2, 5)
                            },
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(1, 3, 1)
                            },
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(9, 9, 9)
                            },
                            new WorldTile()
                            {
                                Position = new GameFrame.Core.Math.Vector3Int(1, 3, 9)
                            }
                        }
                    }

                }
            };

            return world;
        }
    }
}
