using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Model;

using GameFrame.Core.Collections;
using GameFrame.Core.Extensions;

using Unity.VisualScripting;

using UnityEngine;

using UnityVector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Scenes.Game
{
    public class WorldBehaviour : MonoBehaviour
    {
        public GameObject chunkContainer;
        public GameObject penguinTemplate;
        public GameObject foodTemplate;
        public GameObject foodContainer;
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

                RenderFoods();
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

                if (TryGetPosition(startingPosition.X, startingPosition.Y, out var position))
                {
                    gameState.Penguin = new Penguin()
                    {
                        Position = position
                    };
                }
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

        private void FillFoods()
        {
            var currentLevel = gameState.CurrentLevel;
            var mode = gameState.Mode;
            var levelDefinition = gameState.Mode.Levels.FirstOrDefault(l => l.Reference == currentLevel.Reference);

            if (levelDefinition.ActiveFoodLimit.HasValue && levelDefinition.ActiveFoodLimit.Value > 0)
            {
                if (currentLevel.AvailableFoods?.Count > 0)
                {
                    if (gameState.Mode.IsRandomGenerated)
                    {
                        // Not yet supported
                    }
                    else
                    {
                        if (currentLevel.Foods == default)
                        {
                            currentLevel.Foods = new List<Food>();
                        }

                        while (currentLevel.Foods.Count < levelDefinition.ActiveFoodLimit.Value)
                        {
                            var food = default(Food);

                            if (levelDefinition.FoodRandomOrder.HasValue)
                            {
                                if (levelDefinition.FoodRandomOrder.Value)
                                {
                                    food = currentLevel.AvailableFoods.GetRandomEntry();
                                }
                                else 
                                { 
                                    food = currentLevel.AvailableFoods[0];
                                }

                                if (food != default)
                                {
                                    currentLevel.AvailableFoods.Remove(food);
                                    currentLevel.Foods.Add(food);

                                    if (TryGetPosition((Int32)food.Position.X, (Int32)food.Position.Z, out var position))
                                    {
                                        food.Position = position;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RenderFoods()
        {
            FillFoods();

            if (gameState.CurrentLevel.Foods.Count > 0)
            {
                foreach (var food in gameState.CurrentLevel.Foods)
                {
                    RenderFood(food);
                }
            }
        }

        private void RenderFood(Food food)
        {
            var foodObject = Instantiate(foodTemplate, foodContainer.transform);

            var foodBehaviour = foodObject.GetComponent<FoodBehaviour>();

            foodBehaviour.Init(food);
            
            foodBehaviour.transform.position = food.Position.ToUnity();

            foodObject.SetActive(true);
        }

        private Boolean TryGetPosition(Int32 x, Int32 z, out GameFrame.Core.Math.Vector3 vector)
        {
            vector = GameFrame.Core.Math.Vector3.Zero;

            if (gameState.CurrentLevel.GetChunkMap().TryGetValue(x, z, out var chunk))
            {
                var height = 0f;

                if (chunk.DefaultTileHeight.HasValue)
                {
                    height = chunk.DefaultTileHeight.Value;
                }

                var centerOffset = gameState.CurrentLevel.Resolution / 2;

                if (chunk.GetTileMap().TryGetValue(centerOffset, centerOffset, out var centerTile))
                {
                    if (centerTile.Position.Y != height)
                    {
                        height = centerTile.Position.Y;
                    }
                }

                var startPositionOffset = (0.5f * gameState.CurrentLevel.Resolution);

                var startPositionX = x * gameState.CurrentLevel.Resolution + startPositionOffset;
                var startPositionZ = z * gameState.CurrentLevel.Resolution + startPositionOffset;

                vector = new GameFrame.Core.Math.Vector3(startPositionX, height, startPositionZ);

                return true;
            }

            return default;
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
