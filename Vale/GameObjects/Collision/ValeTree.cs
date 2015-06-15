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

        public ValeTree(Vector2 minLeafSize, int maxLeafObjs)
        {
            this.minLeafSize = minLeafSize;
            this.maxLeafObjs = maxLeafObjs;
        }

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
        }

        public void Remove(GameObject obj) { } // TODO

        public void Query() { } // TODO

        private void ExpandRoot(AABB newChildbounds)
        {
            bool isNorth = root.Bounds.Origin.Y < newChildbounds.Origin.Y;
            bool isWest = root.Bounds.Origin.X < newChildbounds.Origin.X;

            Quadrant rootQuadrant;
            if (isNorth)
                rootQuadrant = isWest ? Quadrant.NW : Quadrant.NE;
            else
                rootQuadrant = isWest ? Quadrant.SW : Quadrant.SE;

            float newX = (rootQuadrant == Quadrant.NW || rootQuadrant == Quadrant.SW)
                ? root.Bounds.Origin.X
                : root.Bounds.Origin.X - root.Bounds.Width;
            float newY = (rootQuadrant == Quadrant.NW || rootQuadrant == Quadrant.NE)
                ? root.Bounds.Origin.Y
                : root.Bounds.Origin.X - root.Bounds.Height;

            AABB newRootBounds = new AABB(new Vector2(newX, newY), root.Bounds.Width * 2, root.Bounds.Height * 2);
            QuadNode newRoot = new QuadNode(newRootBounds);
            //SetupChildNodes(newRoot); //TODO
            newRoot[rootQuadrant] = root;
            root = newRoot;
        }

        private void InsertNodeObject(QuadNode node, GameObject obj) { } //TODO

        private void SetupChildNodes(QuadNode node)
        {
            if(minLeafSize.X <= node.Bounds.Width / 2 && minLeafSize.Y <= node.Bounds.Height / 2)
            {
                node[Quadrant.NW] = new QuadNode(new Vector2(node.Bounds.Origin.X, node.Bounds.Origin.Y), node.Bounds.Width / 2,
                                  node.Bounds.Height / 2);
                node[Quadrant.NE] = new QuadNode(new Vector2(node.Bounds.Origin.X + node.Bounds.Width / 2, node.Bounds.Origin.Y),
                                                  node.Bounds.Width / 2,
                                                  node.Bounds.Height / 2);
                node[Quadrant.SW] = new QuadNode(new Vector2(node.Bounds.Origin.X, node.Bounds.Origin.Y + node.Bounds.Height / 2),
                                                  node.Bounds.Width / 2,
                                                  node.Bounds.Height / 2);
                node[Quadrant.SE] = new QuadNode(new Vector2(node.Bounds.Origin.X + node.Bounds.Width / 2,
                                                  node.Bounds.Origin.Y + node.Bounds.Height / 2),
                                                  node.Bounds.Width / 2, node.Bounds.Height / 2);
            }
        }

        void collider_BoundsChanged(object sender, EventArgs e) { } //TODO
    }
}
