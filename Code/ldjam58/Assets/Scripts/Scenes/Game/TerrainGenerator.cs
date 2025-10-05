using System;
using System.ComponentModel;

using Assets.Scripts.Core.Model;

using GameFrame.Core.Collections;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class TerrainGenerator
    {
        private readonly Level world;
        private readonly Material terrainMaterial;
        private readonly PhysicsMaterial iceMaterial;
        private readonly PhysicsMaterial snowMaterial;
        private readonly Map<Int32, WorldChunk> chunkMap;
        private readonly Map<Int32, ChunkBehaviour> chunkBehaviourMap;

        public TerrainGenerator(Map<Int32, ChunkBehaviour> chunkBehaviourMap, Material terrainMaterial, PhysicsMaterial ice, PhysicsMaterial snow, Level world)
        {
            this.world = world;
            this.terrainMaterial = terrainMaterial;
            this.iceMaterial = ice;
            this.snowMaterial = snow;

            this.chunkMap = world.GetChunkMap();
            this.chunkBehaviourMap = chunkBehaviourMap;
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

            var verticies = new Vector3[(world.Resolution + 1) * (world.Resolution + 1)];
            var uvs = new Vector2[verticies.Length];

            var index = 0;

            for (int x = 0; x <= world.Resolution; x++)
            {
                for (int z = 0; z <= world.Resolution; z++)
                {
                    var y = 0f;

                    if (worldChunk != default)
                    {
                        if (worldChunk.DefaultTileHeight.HasValue)
                        {
                            y = worldChunk.DefaultTileHeight.Value;
                        }

                        if (chunkTileMap.TryGetValue(x, z, out var tile))
                        {
                            y = tile.Position.Y;
                        }
                    }

                    if (x == 0 && z == 0)
                    {
                        var yLeft = GetNeighbourTileHeight(chunkX - 1, chunkZ, world.Resolution, z);
                        var yBottomLeft = GetNeighbourTileHeight(chunkX - 1, chunkZ - 1, world.Resolution, world.Resolution);
                        var yBottom = GetNeighbourTileHeight(chunkX, chunkZ - 1, x, world.Resolution);

                        if (y != yLeft || y != yBottom || y != yBottom)
                        {
                            var average = (y + yLeft + yBottomLeft + yBottom) / 4f;

                            y = average;
                            SetNeighbourTileHeight(chunkX - 1, chunkZ, world.Resolution, z, average);
                            SetNeighbourTileHeight(chunkX - 1, chunkZ - 1, world.Resolution, world.Resolution, average);
                            SetNeighbourTileHeight(chunkX, chunkZ - 1, x, world.Resolution, average);
                        }
                    }
                    else if (x == 0)
                    {
                        var yBottom = GetNeighbourTileHeight(chunkX, chunkZ - 1, x, world.Resolution);

                        if (y != yBottom)
                        {
                            var average = (y + yBottom) / 2;

                            y = average;
                            SetNeighbourTileHeight(chunkX, chunkZ - 1, x, world.Resolution, average);
                        }
                    }
                    else if (z == 0)
                    {
                        var yLeft = GetNeighbourTileHeight(chunkX - 1, chunkZ, world.Resolution, z);

                        if (y != yLeft)
                        {
                            var average = (y + yLeft) / 2;

                            y = average;
                            SetNeighbourTileHeight(chunkX - 1, chunkZ, world.Resolution, z, average);
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

            var triangles = new int[world.Resolution * world.Resolution * 6];

            var triangle = 0;
            var vertex = 0;

            for (int x = 0; x < world.Resolution; x++)
            {
                for (int z = 0; z < world.Resolution; z++)
                {
                    triangles[triangle] = vertex;
                    triangles[triangle + 1] = vertex + 1;
                    triangles[triangle + 2] = vertex + world.Resolution + 1;
                    triangles[triangle + 3] = vertex + 1;
                    triangles[triangle + 4] = vertex + world.Resolution + 2;
                    triangles[triangle + 5] = vertex + world.Resolution + 1;

                    vertex++;
                    triangle += 6;
                }

                vertex++;
            }

            mesh.Clear();

            mesh.vertices = verticies;
            mesh.triangles = triangles;
            mesh.uv = uvs;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            collider.sharedMaterial = iceMaterial;
            //collider.sharedMaterial = snowMaterial;
            collider.sharedMesh = mesh;
            filter.mesh = mesh;
            renderer.material = terrainMaterial;

            return chunkBehaviour;
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

                var vector = neightbourChunk.Mesh.vertices[index];

                neightbourChunk.Mesh.vertices[index] = new Vector3(vector.x, newY, vector.z);

                return true;
            }

            return false;
        }

        private Int32 GetIndex(Int32 tileX, Int32 tileZ)
        {
            return tileX * (world.Resolution+1) + tileZ;
        }
    }
}
