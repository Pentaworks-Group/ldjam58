using System;
using System.ComponentModel;

using Assets.Scripts.Core.Model;

using GameFrame.Core.Collections;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class TerrainGenerator
    {
        private readonly Level level;
        private readonly GameObject chunkContainer;

        private readonly Material terrainMaterial;
        private readonly PhysicsMaterial iceMaterial;
        private readonly PhysicsMaterial snowMaterial;
        private readonly Map<Int32, WorldChunk> chunkMap;

        private Map<Int32, ChunkBehaviour> chunkBehaviourMap;

        public TerrainGenerator(GameObject chunkContainer, Material terrainMaterial, PhysicsMaterial ice, PhysicsMaterial snow, Level world)
        {
            this.chunkContainer = chunkContainer;

            this.level = world;
            this.terrainMaterial = terrainMaterial;
            this.iceMaterial = ice;
            this.snowMaterial = snow;

            this.chunkMap = world.GetChunkMap();
        }

        public Map<Int32, ChunkBehaviour> Generate()
        {
            this.chunkBehaviourMap = new Map<Int32, ChunkBehaviour>();

            for (int z = 0; z < level.Size.Y; z++)
            {
                for (int x = 0; x < level.Size.X; x++)
                {
                    var mapChunk = new GameObject($"Chunk-{x}-{z}", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(ChunkBehaviour));

                    mapChunk.transform.parent = chunkContainer.transform;
                    mapChunk.transform.localPosition = new Vector3(x * level.Resolution, 0f, z * level.Resolution);

                    Generate(x, z, mapChunk);
                }
            }

            return chunkBehaviourMap;
        }

        public ChunkBehaviour Generate(Int32 chunkX, Int32 chunkZ, GameObject chunk)
        {
            _ = this.chunkMap.TryGetValue(chunkX, chunkZ, out var worldChunk);
            var chunkTileMap = worldChunk?.GetTileMap();

            var filter = chunk.GetComponent<MeshFilter>();
            var collider = chunk.GetComponent<MeshCollider>();
            var renderer = chunk.GetComponent<MeshRenderer>();
            var chunkBehaviour = chunk.GetComponent<ChunkBehaviour>();

            chunkBehaviourMap[chunkX, chunkZ] = chunkBehaviour;

            var mesh = new Mesh();
            chunkBehaviour.Mesh = mesh;

            var worldPosition = filter.gameObject.transform.position;

            var verticies = new Vector3[(level.Resolution + 1) * (level.Resolution + 1)];
            var uvs = new Vector2[verticies.Length];

            var index = 0;

            for (int x = 0; x <= level.Resolution; x++)
            {
                for (int z = 0; z <= level.Resolution; z++)
                {
                    var y = 0f;

                    if (worldChunk != default)
                    {
                        if (worldChunk.DefaultTileHeight.HasValue)
                        {
                            y = worldChunk.DefaultTileHeight.Value;
                        }

                        var tileX = Math.Min(x, level.Resolution - 1);
                        var tileZ = Math.Min(z, level.Resolution - 1);                                               

                        if (chunkTileMap.TryGetValue(tileX, tileZ, out var tile))
                        {
                            y = tile.Position.Y;
                        }
                    }

                    if (x == 0 && z == 0)
                    {
                        var yLeft = GetNeighbourTileHeight(chunkX - 1, chunkZ, level.Resolution, z);
                        var yBottomLeft = GetNeighbourTileHeight(chunkX - 1, chunkZ - 1, level.Resolution, level.Resolution);
                        var yBottom = GetNeighbourTileHeight(chunkX, chunkZ - 1, x, level.Resolution);

                        if (y != yLeft || y != yBottom || y != yBottom)
                        {
                            var average = (y + yLeft + yBottomLeft + yBottom) / 4f;

                            y = average;
                            SetNeighbourTileHeight(chunkX - 1, chunkZ, level.Resolution, z, average);
                            SetNeighbourTileHeight(chunkX - 1, chunkZ - 1, level.Resolution, level.Resolution, average);
                            SetNeighbourTileHeight(chunkX, chunkZ - 1, x, level.Resolution, average);
                        }
                    }
                    else if (x == 0)
                    {
                        var yBottom = GetNeighbourTileHeight(chunkX - 1, chunkZ, level.Resolution, z);

                        if (y != yBottom)
                        {
                            var average = (y + yBottom) / 2;

                            y = average;
                            SetNeighbourTileHeight(chunkX - 1, chunkZ, level.Resolution, z, average);
                        }
                    }
                    else if (z == 0)
                    {
                        var yLeft = GetNeighbourTileHeight(chunkX, chunkZ - 1, x, level.Resolution);

                        if (y != yLeft)
                        {
                            var average = (y + yLeft) / 2;

                            y = average;
                            SetNeighbourTileHeight(chunkX, chunkZ - 1, x, level.Resolution, average);
                        }
                    }

                    verticies[index] = new Vector3(x, y, z);
                    index++;
                }
            }

            for (var i = 0; i < uvs.Length; i++)
            {
                var vector = verticies[i];

                uvs[i] = new Vector2(vector.x + worldPosition.x, vector.z + worldPosition.z);
            }

            var triangles = new int[level.Resolution * level.Resolution * 6];

            var triangle = 0;
            var vertex = 0;

            for (int x = 0; x < level.Resolution; x++)
            {
                for (int z = 0; z < level.Resolution; z++)
                {
                    triangles[triangle] = vertex;
                    triangles[triangle + 1] = vertex + 1;
                    triangles[triangle + 2] = vertex + level.Resolution + 1;
                    triangles[triangle + 3] = vertex + 1;
                    triangles[triangle + 4] = vertex + level.Resolution + 2;
                    triangles[triangle + 5] = vertex + level.Resolution + 1;

                    vertex++;
                    triangle += 6;
                }

                vertex++;
            }

            mesh.Clear();

            mesh.vertices = verticies;
            mesh.triangles = triangles;
            mesh.uv = uvs;

            //mesh.RecalculateBounds();
            //mesh.RecalculateNormals();

            collider.sharedMaterial = iceMaterial;
            //collider.sharedMaterial = snowMaterial;

            collider.sharedMesh = mesh;
            filter.mesh = mesh;
            renderer.material = terrainMaterial;

            return chunkBehaviour;
        }

        public void Stitch()
        {
            //foreach (var chunk in chunkBehaviourMap.GetAll())
            //{
            //    mesh.RecalculateBounds();
            //    mesh.RecalculateNormals();
            //}

            //if (x == 0 && z == 0)
            //{
            //    var yLeft = GetNeighbourTileHeight(chunkX - 1, chunkZ, level.Resolution, z);
            //    var yBottomLeft = GetNeighbourTileHeight(chunkX - 1, chunkZ - 1, level.Resolution, level.Resolution);
            //    var yBottom = GetNeighbourTileHeight(chunkX, chunkZ - 1, x, level.Resolution);

            //    if (y != yLeft || y != yBottom || y != yBottom)
            //    {
            //        var average = (y + yLeft + yBottomLeft + yBottom) / 4f;

            //        y = average;
            //        SetNeighbourTileHeight(chunkX - 1, chunkZ, level.Resolution, z, average);
            //        SetNeighbourTileHeight(chunkX - 1, chunkZ - 1, level.Resolution, level.Resolution, average);
            //        SetNeighbourTileHeight(chunkX, chunkZ - 1, x, level.Resolution, average);
            //    }
            //}
            //else if (x == 0)
            //{
            //    var yBottom = GetNeighbourTileHeight(chunkX - 1, chunkZ, level.Resolution, z);

            //    if (y != yBottom)
            //    {
            //        var average = (y + yBottom) / 2;

            //        y = average;
            //        SetNeighbourTileHeight(chunkX - 1, chunkZ, level.Resolution, z, average);
            //    }
            //}
            //else if (z == 0)
            //{
            //    var yLeft = GetNeighbourTileHeight(chunkX, chunkZ - 1, x, level.Resolution);

            //    if (y != yLeft)
            //    {
            //        var average = (y + yLeft) / 2;

            //        y = average;
            //        SetNeighbourTileHeight(chunkX, chunkZ - 1, x, level.Resolution, average);
            //    }
            //}
        }

        private Single GetNeighbourTileHeight(Int32 chunkX, Int32 chunkZ, Int32 tileX, Int32 tileZ)
        {
            if (this.chunkBehaviourMap.TryGetValue(chunkX, chunkZ, out var neightbourChunk))
            {
                var index = GetIndex(tileX, tileZ);

                var vector = neightbourChunk.Mesh.vertices[index];

                return vector.y;
            }

            return default;
        }

        private Boolean SetNeighbourTileHeight(Int32 chunkX, Int32 chunkZ, Int32 tileX, Int32 tileZ, Single newY)
        {
            if (this.chunkBehaviourMap.TryGetValue(chunkX, chunkZ, out var neightbourChunk))
            {
                var index = GetIndex(tileX, tileZ);

                var verticies = neightbourChunk.Mesh.vertices;

                var vector = verticies[index];

                verticies[index] = new Vector3(vector.x, newY, vector.z);

                neightbourChunk.Mesh.SetVertices(verticies);

                return true;
            }

            return false;
        }

        private Int32 GetIndex(Int32 tileX, Int32 tileZ)
        {
            return tileX * (level.Resolution+1) + tileZ;
        }
    }
}
