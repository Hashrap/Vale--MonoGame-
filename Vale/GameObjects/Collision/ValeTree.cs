using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace Vale.GameObjects.Collision
{
    public enum Quadrant : int
    {
        NW = 0,
        NE = 1,
        SW = 2,
        SE = 3
    }

    public class ValeTree
    {
        public class QuadNode
        {
            private static int _id = 0;
            public readonly int ID = _id++;

            public QuadNode Parent { get; internal set; }

            public AABB Bounds { get; internal set; }
            internal List<GameObject> _nodeObjs = new List<GameObject>();
            public ReadOnlyCollection<GameObject> Objects;

            private QuadNode[] _nodes = new QuadNode[4];
            public ReadOnlyCollection<QuadNode> Nodes;
            public QuadNode this[Quadrant direction]
            {
                get
                {
                    switch (direction)
                    {
                        case Quadrant.NW:
                            return _nodes[0];
                        case Quadrant.NE:
                            return _nodes[1];
                        case Quadrant.SW:
                            return _nodes[2];
                        case Quadrant.SE:
                            return _nodes[3];
                        default:
                            return null;
                    }
                }
                set
                {
                    switch (direction)
                    {
                        case Quadrant.NW:
                            _nodes[0] = value;
                            break;
                        case Quadrant.NE:
                            _nodes[1] = value;
                            break;
                        case Quadrant.SW:
                            _nodes[2] = value;
                            break;
                        case Quadrant.SE:
                            _nodes[3] = value;
                            break;
                    }
                    if (value != null)
                        value.Parent = this;
                }
            }

            public QuadNode (AABB bounds)
            {
                Bounds = bounds;
                Nodes = new ReadOnlyCollection<QuadNode>(_nodes);
                Objects = new ReadOnlyCollection<GameObject>(_nodeObjs);
            }

            public QuadNode(Vector2 origin, float width, float height)
                : this(new AABB(origin, width, height)) { }

            public bool HasChildren() { return _nodes[0] != null; }
        }

        private QuadNode root = null;
        public QuadNode Root { get { return root; } }

        private readonly Vector2 minLeafSize;
        private readonly int maxLeafObjs;

        public void Insert(GameObject obj)
        {
            AABB bounds = obj.Bounds;
            if(root == null)
            {
                Vector2 rootSize = new Vector2((float)Math.Ceiling(bounds.Width / minLeafSize.X),
                                               (float)Math.Ceiling(bounds.Height / minLeafSize.Y));
                float multiplier = Math.Max(rootSize.X, rootSize.Y);
                rootSize = new Vector2(minLeafSize.X * multiplier, minLeafSize.Y * multiplier);
                root = new QuadNode(new AABB(bounds.Origin, rootSize));
            }

            while (!root.Bounds.Contains(bounds))
            {
                ExpandRoot(bounds);
            }

            InsertNodeObject(root, obj);
        } // TODO

        public void Remove(GameObject obj) { } // TODO

        public void Query() { } // TODO

        private void ExpandRoot(AABB newChildbounds) { } // TODO

        private void InsertNodeObject(QuadNode node, GameObject obj) { } //TODO
    }
}
