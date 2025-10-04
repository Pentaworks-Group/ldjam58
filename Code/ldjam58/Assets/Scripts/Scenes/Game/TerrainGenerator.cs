using Assets.Scripts.Core.Model;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class TerrainGenerator
    {
        private readonly Level world;
        private readonly Material terrainMaterial;
        private readonly PhysicsMaterial iceMaterial;
        private readonly PhysicsMaterial snowMaterial;

        public TerrainGenerator(Material terrainMaterial, PhysicsMaterial ice, PhysicsMaterial snow, Level world)
        {
            this.world = world;
            this.terrainMaterial = terrainMaterial;
            this.iceMaterial = ice;
            this.snowMaterial = snow;
        }

        public void Generate(WorldChunk worldChunk, GameObject chunk)
        {
            var chunkTileMap = worldChunk?.GetTileMap();

            var filter = chunk.GetComponent<MeshFilter>();
            var collider = chunk.GetComponent<MeshCollider>();
            var renderer = chunk.GetComponent<MeshRenderer>();

            var mesh = new Mesh();

            var worldPosition = filter.gameObject.transform.position;

            var verticies = new Vector3[(world.Resolution + 1) * (world.Resolution + 1)];
            var uvs = new Vector2[verticies.Length];

            var index = 0;

            for (int x = 0; x <= world.Resolution; x++)
            {
                for (int z = 0; z <= world.Resolution; z++)
                {
                    var y = 0;

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

            //collider.sharedMaterial = iceMaterial;
            //collider.sharedMaterial = snowMaterial;
            collider.sharedMesh = mesh;
            filter.mesh = mesh;
            renderer.material = terrainMaterial;
        }
    }
}
