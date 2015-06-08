using System;
using Microsoft.Xna.Framework;

namespace Vale.GameObjects.Collision
{
    public class Circle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public static bool TestCollision(Circle a, Circle b)
        {
            float d2 = Vector2.DistanceSquared(a.Center, b.Center);
            float sumOfRadii = a.Radius + b.Radius;
            return d2 <= sumOfRadii * sumOfRadii;
        }
        public static bool TestCollision(Circle a, AABB b)
        {
            return false;
        }
    }
}
