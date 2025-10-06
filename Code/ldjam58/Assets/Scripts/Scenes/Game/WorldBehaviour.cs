using System;
using System.Collections.Generic;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Model;

using GameFrame.Core.Extensions;

using TMPro;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class WorldBehaviour : MonoBehaviour
    {
        private readonly Dictionary<String, GameObject> foodTemplates = new Dictionary<String, GameObject>();
        private readonly Dictionary<Guid, FoodBehaviour> renderedFoods = new Dictionary<Guid, FoodBehaviour>();

        private GameState gameState;

        public GameObject chunkContainer;
        public GameObject penguinTemplate;
        public Transform foodTemplatesContainer;
        public Transform foodContainerTransform;
        public GameObject rootContainer;

        public TMP_Text currentLevelText;

        public Material terrainMaterial;
        public PhysicsMaterial iceMaterial;
        public PhysicsMaterial snowMaterial;

        public PenguinBehaviour PenguinBehaviour { get; private set; }

        private void OnGameInitialized()
        {
            Base.Core.Game.OnPauseToggled.AddListener(OnPauseToggled);

            this.gameState = Base.Core.Game.State;

            LoadTemplates();

            if (RenderWorld())
            {
                RenderPenguin();

                RenderObstacles();

                gameState.FillFoods();
                RenderFoods();

                currentLevelText.text = gameState.CurrentLevel.Name;
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
            if (gameState.Penguin != default)
            {
                if (gameState.Penguin.Position == GameFrame.Core.Math.Vector3.Zero)
                {
                    if (gameState.CurrentLevel.IsPenguinStartPositionRandom)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            if (TryGetRandomPositionOnChunk(out var position))
                            {
                                gameState.Penguin.Position = position;
                                break;
                            }
                        }
                    }
                    else
                    {
                        var startingPosition = gameState.CurrentLevel.PenguinStartPosition;

                        if (TryGetPosition(startingPosition.X, startingPosition.Y, out var position))
                        {
                            gameState.Penguin.Position = position;
                        }
                        else
                        {
                            throw new Exception("Penguin start position is not over land!");
                        }
                    }
                }

                var penguinObject = GameObject.Instantiate(penguinTemplate, rootContainer.transform);

                var penguinBehaviour = penguinObject.GetComponent<PenguinBehaviour>();

                penguinBehaviour.Init(gameState.Penguin);
                penguinBehaviour.Eaten.AddListener(OnFoodEaten);

                penguinBehaviour.transform.position = gameState.Penguin.Position.ToUnity();

                this.PenguinBehaviour = penguinBehaviour;

                penguinObject.SetActive(true);
            }
            else
            {
                throw new Exception("GameState has no Peeeenguiiin!");
            }
        }

        private void RenderFoods()
        {
            if (gameState.CurrentLevel.Foods.Count > 0)
            {
                foreach (var food in gameState.CurrentLevel.Foods)
                {
                    if (!renderedFoods.ContainsKey(food.ID))
                    {
                        var foodBehaviour = RenderFood(food);

                        renderedFoods[food.ID] = foodBehaviour;
                    }
                }
            }
        }

        private FoodBehaviour RenderFood(Food food)
        {
            var foodObject = GetTemplateCopy(foodTemplates, food.Definition.Reference, foodContainerTransform);

            var foodBehaviour = foodObject.GetComponent<FoodBehaviour>();

            foodBehaviour.Init(food);

            var hasValidPosition = false;

            if (gameState.CurrentLevel.IsFoodPositionRandom)
            {
                for (int i = 0; i < 100; i++)
                {
                    if (TryGetRandomPositionOnChunk(out var position))
                    {
                        foodBehaviour.transform.position = position.ToUnity();
                        hasValidPosition = true;
                        break;
                    }
                }
            }
            else
            {
                if (TryGetPosition((Int32)food.Position.X, (Int32)food.Position.Z, out var position))
                {
                    foodBehaviour.transform.position = position.ToUnity();
                    hasValidPosition = true;
                }
            }

            if (!hasValidPosition)
            {
                throw new Exception("Food was positioned outside of the playable area");
            }

            foodObject.SetActive(true);

            return foodBehaviour;
        }

        private void OnFoodEaten(FoodBehaviour foodBehaviour)
        {
            gameState.FoodEaten++;
            gameState.CurrentLevel.Foods.Remove(foodBehaviour.Food);

            renderedFoods.Remove(foodBehaviour.Food.ID);

            Destroy(foodBehaviour.gameObject);

            gameState.FillFoods();

            if (gameState.CurrentLevel.Foods.Count == 0)
            {
                Base.Core.Game.ChangeScene(Assets.Scripts.Constants.Scenes.LevelCompleted);
            }
            else
            {
                RenderFoods();
            }
        }

        private Boolean TryGetRandomPositionOnChunk(out GameFrame.Core.Math.Vector3 position)
        {
            position = default;

            for (int i = 0; i < 10; i++)
            {
                var randomX = UnityEngine.Random.Range(0, gameState.CurrentLevel.Size.X);
                var randomY = UnityEngine.Random.Range(0, gameState.CurrentLevel.Size.Y);

                if (TryGetPosition(randomX, randomY, out position))
                {
                    return true;
                }
            }

            return false;
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

        private void LoadTemplates()
        {
            if (this.foodTemplatesContainer != null)
            {
                foreach (Transform template in foodTemplatesContainer)
                {
                    this.foodTemplates[template.name] = template.gameObject;
                }
            }
        }

        public GameObject GetTemplateCopy(IDictionary<String, GameObject> cache, String templateRefernce, Transform parentTransform, Boolean inWorldSpace = true)
        {
            if (cache.TryGetValue(templateRefernce, out var template))
            {
                return Instantiate(template, parentTransform, inWorldSpace);
            }

            return null;
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
