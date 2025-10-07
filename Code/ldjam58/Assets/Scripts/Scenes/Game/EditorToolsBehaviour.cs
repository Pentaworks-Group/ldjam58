
using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Model;

using Newtonsoft.Json;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game
{
    public class EditorToolsBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Camera Camera;
        [SerializeField]
        private LayerMask raycastLayerMask;
        [SerializeField]
        private WorldBehaviour worldBehaviour;
        [SerializeField]
        private TMP_InputField inputField;

        [SerializeField]
        private UnityEngine.UI.Button previousLevelButton;
        [SerializeField]
        private UnityEngine.UI.Button nextLevelButton;

        private EditorToolBehaviour selectedTool;

        private EventSystem eventSystem;

        private GameState gameState;

        private LevelDefinition previousLevelDefinition;
        private LevelDefinition nextLevelDefinition;
        private int editorEnableClickCount = 0;

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(OnGameInitialized);
        }

        void Start()
        {
            eventSystem = EventSystem.current;
        }

        private void OnEnable()
        {
            HookActions();
        }

        private void OnDisable()
        {
            UnhookActions();
        }

        private void OnGameInitialized()
        {
            this.gameState = Base.Core.Game.State;

            CheckSkipLevel(ref previousLevelDefinition, previousLevelButton, -1);
            CheckSkipLevel(ref nextLevelDefinition, nextLevelButton, 1);
        }

        public void HookActions()
        {
            var moveAction = InputSystem.actions.FindAction("Click");
            moveAction.performed += ExecuteClick;
        }

        public void UnhookActions()
        {
            var moveAction = InputSystem.actions.FindAction("Click");
            moveAction.performed -= ExecuteClick;
        }

        public void EnableEditorCounter()
        {
            editorEnableClickCount++;
            if (editorEnableClickCount > 10)
            {
                EnableEditor();
            }
        }

        private void EnableEditor() {
            worldBehaviour.PenguinBehaviour.HookKeyboardMovement();
            gameObject.SetActive(true);
        }


        public void SaveMap()
        {
            var cleanedChunks = CleanChunks(gameState.CurrentLevel.Chunks);

            var saveContainer = new
            {
                Chunks = cleanedChunks
            };

            var json = GameFrame.Core.Json.Handler.Serialize(saveContainer, Formatting.Indented, new JsonSerializerSettings());

            inputField.text = json;
        }

        public void SelectTool(EditorToolBehaviour toolToSelect)
        {
            if (selectedTool != null)
            {
                selectedTool.DeselectButton();

                if (selectedTool == toolToSelect)
                {
                    selectedTool = null;
                    return;
                }
            }

            selectedTool = toolToSelect;
            toolToSelect.SelectButton();
        }

        public void ExecuteClick(InputAction.CallbackContext context)
        {
            if (selectedTool != null)
            {
                selectedTool.ExecuteClick();
            }
        }

        public void RaiseLevel()
        {
            if (GetTileWithRaycast(out var tile, out var chunk))
            {
                var desiredHeight = tile.Position.Y + 1;
                if (chunk.DefaultTileHeight.HasValue && chunk.DefaultTileHeight.Value == desiredHeight)
                {
                    chunk.RemoveTile(tile);
                }
                else
                {
                    tile.Position = tile.Position.Add(0, +1, 0);
                }

                worldBehaviour.ReRenderWorld();
            }
        }

        public void LowerLevel()
        {
            if (GetTileWithRaycast(out var tile, out var chunk))
            {
                var desiredHeight = tile.Position.Y - 1;

                if (chunk.DefaultTileHeight.HasValue)
                {
                    if (chunk.DefaultTileHeight.Value == desiredHeight)
                    {
                        chunk.RemoveTile(tile);

                        if (chunk.Tiles.Count == 0)
                        {
                            gameState.CurrentLevel.RemoveChunk(chunk);
                        }
                        else
                        {
                            chunk.DefaultTileHeight = null;
                        }
                    }
                    else
                    {
                        tile.Position = tile.Position.Add(0, -1, 0);
                    }
                }
                else
                {
                    if (desiredHeight <= 0)
                    {
                        chunk.RemoveTile(tile);

                        if (chunk.Tiles.Count == 0)
                        {
                            gameState.CurrentLevel.RemoveChunk(chunk);
                        }
                        else
                        {
                            chunk.DefaultTileHeight = null;
                        }
                    }
                    else
                    {
                        tile.Position = tile.Position.Add(0, -1, 0);
                    }
                }

                worldBehaviour.ReRenderWorld();
            }
        }

        public void RaiseChunkLevel()
        {
            if (GetChunkWithRaycast(out var chunk))
            {
                if (chunk.DefaultTileHeight.HasValue)
                {
                    chunk.DefaultTileHeight += 1;
                }
                else
                {
                    chunk.DefaultTileHeight = 1;
                }

                worldBehaviour.ReRenderWorld();
            }
        }

        public void LowerChunkLevel()
        {
            if (GetChunkWithRaycast(out var chunk))
            {
                if (chunk.DefaultTileHeight.HasValue)
                {
                    var desiredValue = chunk.DefaultTileHeight - 1;

                    if (desiredValue < 1)
                    {
                        if (chunk.Tiles == null || chunk.Tiles.Count == 0)
                        {
                            gameState.CurrentLevel.RemoveChunk(chunk);
                        }
                        else
                        {
                            chunk.DefaultTileHeight = null;
                        }

                    }
                    else
                    {
                        chunk.DefaultTileHeight -= 1;
                    }
                }
                else if (chunk.Tiles == default || chunk.Tiles.Count < 1)
                {
                    gameState.CurrentLevel.RemoveChunk(chunk);
                }

                worldBehaviour.ReRenderWorld();
            }
        }

        public void TogglePenguinGravity(EditorToolBehaviour gravityButton)
        {
            if (worldBehaviour.PenguinBehaviour.TryGetComponent<Rigidbody>(out Rigidbody body))
            {
                body.useGravity = !body.useGravity;
                if (body.useGravity)
                {
                    body.linearDamping = 0;
                    body.angularDamping = 0;
                }
                else
                {
                    body.linearDamping = 1;
                    body.angularDamping = 1;
                }

                gravityButton.ToggleButton();
            }
        }

        public void OnNextLevel()
        {
            gameState.CurrentLevel = new LevelConverter().Convert(nextLevelDefinition);
            gameState.Penguin.Position = default;

            worldBehaviour.ReloadWorld();

            CheckSkipLevel(ref previousLevelDefinition, previousLevelButton, -1);
            CheckSkipLevel(ref nextLevelDefinition, nextLevelButton, 1);
        }

        public void OnPreviousLevel()
        {
            gameState.CurrentLevel = new LevelConverter().Convert(previousLevelDefinition);
            gameState.Penguin.Position = default;

            worldBehaviour.ReloadWorld();

            CheckSkipLevel(ref previousLevelDefinition, previousLevelButton, -1);
            CheckSkipLevel(ref nextLevelDefinition, nextLevelButton, 1);
        }

        private Boolean GetTileWithRaycast(out WorldTile tile, out WorldChunk chunk)
        {
            if (MakeRaycast(out Vector3 hitPoint))
            {
                GetWorldTileFromPosition(hitPoint, out tile, out chunk);
                return true;
            }

            tile = null;
            chunk = null;
            return false;
        }

        private Boolean GetChunkWithRaycast(out WorldChunk chunk)
        {
            if (MakeRaycast(out Vector3 hitPoint))
            {
                GetWorldChunkFromPosition(hitPoint, out chunk);
                return true;
            }

            chunk = null;
            return false;
        }

        private Boolean MakeRaycast(out Vector3 hitPoint)
        {
            hitPoint = Vector3.zero;

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Pointer.current.position.ReadValue()
            };

            

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                return false;
            }

            int layerMask = ~(1 << LayerMask.NameToLayer("IgnoreForToolRaycast"));
            Ray ray = Camera.main.ScreenPointToRay(Pointer.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f);
            if (Physics.Raycast(ray, out var hit, 10000, layerMask))
            {
                hitPoint = hit.point;
                Debug.Log("hitpoint: " + hitPoint);
                return true;
            }
            else
            {
                hitPoint = Vector3.zero;
                return false;
            }
        }

        private void GetWorldChunkFromPosition(Vector3 position, out WorldChunk chunk)
        {
            var resolution = gameState.CurrentLevel.Resolution;
            float x = position.x + 0.5f;
            int xC = (int)(x / resolution);

            float z = position.z + 0.5f;
            int yC = (int)(z / resolution);
            chunk = GetChunkFromIndixes(xC, yC);
        }

        private void GetWorldTileFromPosition(Vector3 position, out WorldTile tile, out WorldChunk chunk)
        {
            var resolution = gameState.CurrentLevel.Resolution;
            float x = position.x + 0.5f;
            int xC = (int)(x / resolution);
            int xT = (int)(x % resolution);

            float z = position.z + 0.5f;
            int yC = (int)(z / resolution);
            int yT = (int)(z % resolution);
            chunk = GetChunkFromIndixes(xC, yC);
            if (!chunk.GetTileMap().TryGetValue(xT, yT, out tile))
            {
                var height = 0;
                if (chunk.DefaultTileHeight.HasValue)
                {
                    height = chunk.DefaultTileHeight.Value;
                }
                tile = new WorldTile()
                {
                    Position = new GameFrame.Core.Math.Vector3Int(xT, height, yT)
                };
                chunk.AddTile(tile);
            }

            int calcX = chunk.Position.X * resolution + tile.Position.X;
            int calcZ = chunk.Position.Y * resolution + tile.Position.Z;
            Debug.Log("chunk: " + chunk.Position + "  tile: " + tile.Position + "  calced: " + calcX + "," + calcZ);
        }

        private WorldChunk GetChunkFromIndixes(int xC, int yC)
        {
            WorldChunk chunk;
            if (!gameState.CurrentLevel.GetChunkMap().TryGetValue(xC, yC, out chunk))
            {
                chunk = new WorldChunk()
                {
                    Position = new GameFrame.Core.Math.Vector2Int(xC, yC)
                };
                gameState.CurrentLevel.AddChunk(chunk);
            }

            return chunk;
        }

        private List<WorldChunk> CleanChunks(List<WorldChunk> chunks)
        {
            var cleanChunks = new List<WorldChunk>();

            foreach (var sourceChunk in chunks)
            {
                var isClean = true;

                if (cleanChunks.Any(c => c.Position == sourceChunk.Position))
                {
                    isClean = false;
                }

                if (!sourceChunk.DefaultTileHeight.HasValue && (sourceChunk.Tiles == null || sourceChunk.Tiles.Count < 1))
                {
                    isClean = false;
                }

                if (isClean)
                {
                    if (CleanTiles(sourceChunk.Tiles, out var cleanedTiles))
                    {
                        cleanChunks.Add(new WorldChunk()
                        {
                            Position = sourceChunk.Position,
                            DefaultTileHeight = sourceChunk.DefaultTileHeight,
                            Tiles = cleanedTiles
                        });
                    }
                }
            }

            return cleanChunks;
        }

        private Boolean CleanTiles(List<WorldTile> sourceTiles, out List<WorldTile> cleanedTiles)
        {
            cleanedTiles = default;

            if (sourceTiles?.Count > 0)
            {
                cleanedTiles = new List<WorldTile>();

                foreach (var tile in sourceTiles)
                {
                    if (tile.Position != default)
                    {
                        if (!cleanedTiles.Any(t => t.Position == tile.Position))
                        {
                            cleanedTiles.Add(new WorldTile() { Position = tile.Position });
                        }
                    }
                }
            }

            return true;
        }

        private LevelDefinition GetLevelDefinition(Int32 levelOffset)
        {
            var currentLevelDefinition = gameState.Mode.Levels.FirstOrDefault(l => l.Reference == gameState.CurrentLevel.Reference);

            var currentLevelIndex = gameState.Mode.Levels.IndexOf(currentLevelDefinition);

            var newLevelDefinitionIndex = currentLevelIndex + levelOffset;

            if (newLevelDefinitionIndex >= 0 && newLevelDefinitionIndex < gameState.Mode.Levels.Count)
            {
                return gameState.Mode.Levels[newLevelDefinitionIndex];
            }

            return default;
        }

        private void CheckSkipLevel(ref LevelDefinition levelDefinition, UnityEngine.UI.Button button, Int32 indexOffset)
        {
            levelDefinition = GetLevelDefinition(indexOffset);
            button.interactable = levelDefinition != default;
        }
    }
}