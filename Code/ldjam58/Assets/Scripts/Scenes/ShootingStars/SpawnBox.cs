using UnityEngine;

namespace Assets.Scripts.Scenes.ShootingStars
{
    public class SpawnBox
    {
        public SpawnBox(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
        public Rect Rectangle { get; set; }

        public Vector3 GetRandomPosition(float z)
        {
            var rando = new Vector3(Random.Range(Rectangle.xMin, Rectangle.xMax), Random.Range(Rectangle.yMin, Rectangle.yMax), z);

            return rando;
        }

        public override System.String ToString()
        {
            return this.Name;
        }
    }
}
