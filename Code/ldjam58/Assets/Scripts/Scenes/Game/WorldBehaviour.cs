using System;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Model;

using GameFrame.Core.Collections;
using GameFrame.Core.Extensions;

using UnityVector3 = UnityEngine.Vector3;

using UnityEngine;

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

            RenderWorld();            
            RenderPenguin();
        }

        private void RenderWorld()
        {
            if (gameState.World != default)
            {
                var tileMap = GenerateChunkMap(gameState.World);

                var terrainGenerator = new TerrainGenerator(terrainMaterial, gameState.World);

                for (int x = 0; x < gameState.World.Size.X; x++)
                {
                    for (int z = 0; z < gameState.World.Size.Y; z++)
                    {
                        var mapChunk = new GameObject($"Chunk-{x}-{z}", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));

                        mapChunk.transform.parent = chunkContainer.transform;
                        mapChunk.transform.localPosition = new UnityVector3(x * gameState.World.Resolution, 0f, z * gameState.World.Resolution);

                        _ = tileMap.TryGetValue(x, z, out var worldChunk);

                        terrainGenerator.Generate(worldChunk, mapChunk);
                    }
                }
            }
        }

        private void RenderPenguin()
        {
            if (gameState.Penguin != default)
            {
                var penguinObject = GameObject.Instantiate(penguinTemplate, rootContainer.transform);

                var penguinBehaviour = penguinObject.GetComponent<PenguinBehaviour>();

                penguinBehaviour.Init(gameState.Penguin);

                penguinBehaviour.transform.position = gameState.Penguin.position.ToUnity();
            }
        }

        private Map<Int32, WorldChunk> GenerateChunkMap(World world)
        {
            var map = new Map<Int32, WorldChunk>();

            foreach (var chunk in world.Chunks)
            {
                map[chunk.Position.X, chunk.Position.Y] = chunk;
            }

            return map;
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
