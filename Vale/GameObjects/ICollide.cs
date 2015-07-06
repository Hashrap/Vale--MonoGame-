using System;
using Vale.GameObjects.Collision;

namespace Vale.GameObjects
{
    internal interface ICollide
    {
        AABB Bounds { get; }
        event EventHandler BoundsChanged;
    }
}
