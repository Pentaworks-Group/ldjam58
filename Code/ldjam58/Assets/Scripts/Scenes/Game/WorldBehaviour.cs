using System;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Model;

using GameFrame.Core.Collections;
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
        public PhysicsMaterial iceMaterial;
        public PhysicsMaterial snowMaterial;

        private GameState gameState;
        public PenguinBehaviour PenguinBehaviour { get; private set; }

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

        public void ReRenderWorld()
        {
            foreach (Transform child in chunkContainer.transform)
            {
                Destroy(child.gameObject); //works since gameobjects are destroyed after frame
            }
            RenderWorld();
        }

        private Boolean RenderWorld()
        {
            if (gameState.CurrentLevel != default)
            {
                var terrainGenerator = new TerrainGenerator(chunkContainer, terrainMaterial, iceMaterial, snowMaterial, gameState.CurrentLevel);

                var chunkBehaviourMap = terrainGenerator.Generate();
                                
                //terrainGenerator.Stitch();

                foreach (var chunk in chunkBehaviourMap.GetAll())
                {
                    chunk.Mesh.RecalculateBounds();
                    chunk.Mesh.RecalculateNormals();
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

                this.PenguinBehaviour = penguinObject.GetComponent<PenguinBehaviour>();

                PenguinBehaviour.Init(gameState.Penguin);

                PenguinBehaviour.transform.position = gameState.Penguin.Position.ToUnity();

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
