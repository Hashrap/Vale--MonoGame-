using System;
using Vale.GameObjects.Collision;

namespace Vale.GameObjects
{
    public interface ICollide
    {
        AABB Bounds { get; }
        event EventHandler BoundsChanged;
    }
}
