using System;
using Microsoft.Xna.Framework;

namespace Vale.GameObjects.Collision
{
    public class AABB
    {
        public float[] Max { get; set; }
        public float[] Min { get; set; }

        #region Properties
        public Vector2 Origin
        {
            get { return new Vector2(Min[0], Min[1]); }
        }
        public Vector2 Center
        {
            get { return new Vector2((Min[0] + Max[0]) / 2, (Min[1] + Max[1]) / 2);  }
        }
        public float Width
        {
            get { return Max[0] - Min[0]; }
        }
        public float Height
        {
            get { return Max[1] - Min[1]; }
        }
        public float HalfWidth
        {
            get { return Width / 2; }
        }
        public float HalfHeight
        {
            get { return Height / 2; }
        }
        #endregion
        #region Constructors

        public AABB(float xMin, float xMax, float yMin, float yMax)
        {
            Max = new float[2] { xMax, yMax};
            Min = new float[2] { xMin, yMin};
        }
        public AABB(Vector2 origin, Vector2 opposite)
        {
            Max = new float[2] { opposite.X, opposite.Y };
            Min = new float[2] { origin.X, origin.Y };
        }
        public AABB(Vector2 origin, float width, float height)
        {
            Min = new float[2] { origin.X, origin.Y };
            Max = new float[2] { origin.X + width, origin.Y + height };
        }
        public AABB(float halfWidth, float halfHeight, Vector2 center)
        {
            Min = new float[2] { center.X - halfWidth, center.Y - halfHeight };
            Max = new float[2] { center.X + halfWidth, center.Y + halfHeight };
        }
        #endregion
        #region Tests

        public static bool TestCollision(AABB a, AABB b)
        {
            if (a.Max[0] < b.Min[0] || a.Min[0] > b.Max[0])
                return false;
            if (a.Max[1] < b.Min[1] || a.Min[1] > b.Max[1])
                return false;
            return true;
        }

        public static bool TestCollision(AABB a, Circle b)
        {
            return false; //(TestCollision(a, b.Center) || test line segments vs circle
        }

        public static bool TestCollision(AABB a, Vector2 point)
        {
            if (point.X < a.Min[0] || point.X > a.Max[0])
                return false;
            if (point.Y < a.Min[1] || point.Y > a.Max[1])
                return false;
            return true;
        }

        public static bool TestCollision(AABB a, float aX, float aY, float bX, float bY)
        {
            return false;
        }


        #endregion
    }
}
