
using Assets.Scripts.Core;
using Assets.Scripts.Core.Model;
using Newtonsoft.Json;
using System;
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

        private EditorToolBehaviour selectedTool;

        private EventSystem eventSystem;
        private bool isOverUI;

        private GameState gameState;

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(OnGameInitialized);
        }

        void Start()
        {
            eventSystem = EventSystem.current;
        }

        void Update()
        {
            isOverUI = eventSystem.IsPointerOverGameObject();
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

        public void SaveMap()
        {
            var json = GameFrame.Core.Json.Handler.Serialize(gameState.CurrentLevel.Chunks, Formatting.Indented, new JsonSerializerSettings());
            inputField.text = json;
        }

        public void SelectTool(EditorToolBehaviour toolToSelect)
        {
            if (selectedTool != null)
            {
                selectedTool.DeselectButton();
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
                worldBehaviour.ReRenderWorld();
            }
        }

        public void TogglePenguinGravity(EditorToolBehaviour gravityButton)
        {
            if (worldBehaviour.PenguinBehaviour.TryGetComponent<Rigidbody>(out Rigidbody body))
            {
                body.useGravity = !body.useGravity;
                gravityButton.ToggleButton();
            }
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
            if (isOverUI)
            {
                hitPoint = Vector3.zero;
                return false;
            }
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000))
            {
                //Debug.Log("Did Hit " + hit.collider.gameObject.name + " x: " + hit.point.x + " z: " + hit.point.z);
                hitPoint = hit.point;
            }
            else
            {
                hitPoint = Vector3.zero;
            }
            return true;
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
    }
}