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
            get { return new Vector2(Left, Top); }
        }
        public Vector2 Opposite
        {
            get { return new Vector2(Right, Bottom); }
        }
        public Vector2 Center
        {
            get { return new Vector2((Left + Right) / 2, (Bottom + Top) / 2);  }
        }
        public float Width
        {
            get { return Right - Left; }
        }
        public float Height
        {
            get { return Bottom - Top; }
        }
        public float HalfWidth
        {
            get { return Width / 2; }
        }
        public float HalfHeight
        {
            get { return Height / 2; }
        }
        public float Bottom
        {
            get { return Max[1]; }
        }
        public float Top
        {
            get { return Min[1]; }
        }
        public float Left
        {
            get { return Min[0]; }
        }
        public float Right
        {
            get { return Max[0]; }
        }
        public float X
        {
            get { return Min[0]; }
        }
        public float Y
        {
            get { return Min[1]; }
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

        #region Member Methods
        public void Move(Vector2 v)
        {
            Max[0] += v.X;
            Min[0] += v.X;
            Max[1] += v.Y;
            Min[1] += v.Y;
        }
        public void Move(float x, float y)
        {
            Max[0] += x;
            Min[0] += x;
            Max[1] += y;
            Min[1] += y;
        }
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)Min[0], (int)Min[1], (int)Width, (int)Height);
        }
        #endregion

        #region Member Tests
        public bool Contains(Vector2 point)
        {
            return !(point.X < Left
                || point.X > Right
                || point.Y < Top
                || point.Y > Bottom);
        }
        public bool Contains(AABB box)
        {
            return !(box.Left < Left
                || box.Right > Right
                || box.Top < Top
                || box.Bottom > Bottom);
        }
        public bool Intersects(AABB box)
        {
            return !(box.Left > Right
                || box.Right < Left
                || box.Top > Bottom
                || box.Bottom < Top);
        }
        #endregion

        #region Static Tests
        public static bool Intersects(AABB a, AABB b)
        {
            return !(a.Left > b.Right
                || a.Right < b.Left
                || a.Top > b.Bottom
                || a.Bottom < b.Top);
        }

        public static bool Intersects(AABB a, Vector2 point)
        {
            return !(a.Left > point.X
                || a.Right < point.X
                || a.Top > point.Y
                || a.Bottom < point.Y);
        }
        public static bool Intersects(AABB a, float aX, float aY, float bX, float bY)
        {
            return Intersects(a, new AABB(aX, bX, aY, bY));
        }

        /*public static bool CircleInterset(AABB a, Circle b) { }
        public static bool ContainsAABB
        public static bool ContainsCircle
        public static bool ContainsPoint*/
        // TODO as needed
        #endregion

    }
}
