using System;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Model;

using GameFrame.Core.Extensions;

using UnityEngine;

using UnityVector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Scenes.Game
{
    public class WorldBehaviour : MonoBehaviour
    {
        public GameObject chunkContainer;
        public GameObject penguinTemplate;
        public GameObject rootContainer;

        public Material terrainMaterial;

        private GameState gameState;

        private void OnGameInitialized()
        {
            Base.Core.Game.OnPauseToggled.AddListener(OnPauseToggled);

            this.gameState = Base.Core.Game.State;

            if (RenderWorld())
            {
                RenderPenguin();

                RenderObstacles();
            }
        }

        private Boolean RenderWorld()
        {
            if (gameState.CurrentLevel != default)
            {
                var terrainGenerator = new TerrainGenerator(terrainMaterial, gameState.CurrentLevel);

                var chunkMap = gameState.CurrentLevel.GetChunkMap();

                for (int z = 0; z < gameState.CurrentLevel.Size.Y; z++)
                {
                    for (int x = 0; x < gameState.CurrentLevel.Size.X; x++)
                    {
                        var mapChunk = new GameObject($"Chunk-{x}-{z}", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));

                        mapChunk.transform.parent = chunkContainer.transform;
                        mapChunk.transform.localPosition = new UnityVector3(x * gameState.CurrentLevel.Resolution, 0f, z * gameState.CurrentLevel.Resolution);

                        _ = chunkMap.TryGetValue(x, z, out var worldChunk);

                        terrainGenerator.Generate(worldChunk, mapChunk);
                    }
                }

                return true;
            }

            return false;
        }

        private void RenderPenguin()
        {
            if (gameState.Penguin == default)
            {
                var startingPosition = gameState.CurrentLevel.PenguinStartPosition;

                var y = 0f;

                if (gameState.CurrentLevel.GetChunkMap().TryGetValue(startingPosition.X, startingPosition.Y, out var chunk))
                {
                    if (chunk.DefaultTileHeight.HasValue)
                    {
                        y = chunk.DefaultTileHeight.Value;
                    }

                    var centerOffset = gameState.CurrentLevel.Resolution / 2;

                    if (chunk.GetTileMap().TryGetValue(centerOffset, centerOffset, out var centerTile))
                    {
                        if (centerTile.Position.Y != y)
                        {
                            y = centerTile.Position.Y;
                        }
                    }
                }

                var startPositionOffset = (0.5f * gameState.CurrentLevel.Resolution);

                var startPositionX = startingPosition.X * gameState.CurrentLevel.Resolution + startPositionOffset;
                var startPositionZ = startingPosition.Y * gameState.CurrentLevel.Resolution + startPositionOffset;

                gameState.Penguin = new Penguin()
                {
                    Position = new GameFrame.Core.Math.Vector3(startPositionX, y, startPositionZ)
                };
            }

            if (gameState.Penguin != default)
            {
                var penguinObject = GameObject.Instantiate(penguinTemplate, rootContainer.transform);

                var penguinBehaviour = penguinObject.GetComponent<PenguinBehaviour>();

                penguinBehaviour.Init(gameState.Penguin);

                penguinBehaviour.transform.position = gameState.Penguin.Position.ToUnity();

                penguinObject.SetActive(true);
            }
        }

        private void RenderObstacles()
        {

        }

        private void OnPauseToggled(Boolean isPaused)
        {

        }

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(OnGameInitialized);
        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            //HookActions();
        }

        private void OnDisable()
        {
            //UnhookActions();
        }
    }
}
