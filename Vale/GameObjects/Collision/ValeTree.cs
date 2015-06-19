using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace Vale.GameObjects.Collision
{
    #region Definitions
    public enum Quadrant : int
    {
        NW = 0,
        NE = 1,
        SW = 2,
        SE = 3
    }
    #endregion

    public class ValeTree
    {
        #region Definitions
        public class QuadNode
        {
            private static int _id = 0;
            public readonly int ID = _id++;

            public AABB Bounds { get; internal set; }

            public QuadNode Parent { get; internal set; }

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
            public bool HasChildren { get { return _nodes[0] != null; } }

            internal List<GameObject> _nodeObjs = new List<GameObject>();
            public ReadOnlyCollection<GameObject> Objects;

            public QuadNode (AABB bounds)
            {
                Bounds = bounds;
                Nodes = new ReadOnlyCollection<QuadNode>(_nodes);
                Objects = new ReadOnlyCollection<GameObject>(_nodeObjs);
            }

            public QuadNode(Vector2 origin, float width, float height)
                : this(new AABB(origin, width, height)) { }
        }
        public class RegionNode : QuadNode
        {
            private static int _id = 0;

            public bool Solid { get; internal set; }

            public RegionNode (AABB bounds) : base (bounds)
            {

            }
        }
        #endregion

        #region Attributes
        private QuadNode root = null;
        public QuadNode Root { get { return root; } }

        private Dictionary<GameObject, QuadNode> objectToNodeLookup = new Dictionary<GameObject, QuadNode>();

        private readonly Vector2 minLeafSize;
        private readonly int maxLeafObjs;
        #endregion

        #region Constructor(s)
        public ValeTree(Vector2 minLeafSize, int maxLeafObjs)
        {
            this.minLeafSize = minLeafSize;
            this.maxLeafObjs = maxLeafObjs;
        }
        #endregion

        #region Public Methods
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

        public void Remove(GameObject obj)
        {
            if (!objectToNodeLookup.ContainsKey(obj))
                return; //oops
            QuadNode objNode = objectToNodeLookup[obj];
            RemoveObjectFromNode(obj);
            if (objNode.Parent != null)
                CheckChildNodes(objNode.Parent);

        }

        public List<GameObject> Query(AABB bounds) 
        {
            List<GameObject> results = new List<GameObject>();
            if (root != null)
                Query(bounds, root, results);
            return results;
        }

        public int GetQuadObjectCount() { return 0; } //TODO

        public int GetQuadNodeCount() { return 0; } //TODO

        public List<QuadNode> GetAllNodes() { return new List<QuadNode>(); }
        #endregion

        #region Helper Methods
        private void InsertNodeObject(QuadNode node, GameObject obj)
        {
            if (!node.Bounds.Contains(obj.Bounds))
                Console.WriteLine("you did it wrong");
            return; //oops, object is out of node bounds.  this cannot happen if implemented correctly
            if (!node.HasChildren && node.Objects.Count + 1 > maxLeafObjs)
            {
                SetupChildNodes(node);

                List<GameObject> childObjects = new List<GameObject>(node.Objects);
                List<GameObject> childrenToRelocate = new List<GameObject>();

                foreach (GameObject childObject in childObjects)
                {
                    foreach (QuadNode child in node.Nodes)
                    {
                        if (child == null)
                            continue;
                        if (child.Bounds.Contains(childObject.Bounds))
                            childrenToRelocate.Add(childObject);
                    }
                }

                foreach (GameObject childObject in childrenToRelocate)
                {
                    RemoveObjectFromNode(childObject);
                    InsertNodeObject(node, childObject);
                }
            }
            foreach (QuadNode child in node.Nodes)
            {
                if (child != null)
                {
                    if (child.Bounds.Contains(obj.Bounds))
                    {
                        InsertNodeObject(child, obj);
                        return;
                    }
                }
            }

            AddObjectToNode(node, obj);
        }

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
            SetupChildNodes(newRoot);
            newRoot[rootQuadrant] = root;
            root = newRoot;
        }

        private void Query(AABB bounds, QuadNode node, List<GameObject> results)
        {
            if (node == null) return;

            if (bounds.Intersects(node.Bounds))
            {
                foreach (GameObject quadObject in node.Objects)
                {
                    if (bounds.Intersects(quadObject.Bounds))
                        results.Add(quadObject);
                }

                foreach (QuadNode child in node.Nodes)
                    Query(bounds, child, results);
            }
        }

        private void SetupChildNodes(QuadNode node)
        {
            if (minLeafSize.X <= node.Bounds.Width / 2 && minLeafSize.Y <= node.Bounds.Height / 2)
            {
                node[Quadrant.NW] = new QuadNode(node.Bounds.Origin,
                                                 node.Bounds.HalfWidth,
                                                 node.Bounds.HalfHeight);
                node[Quadrant.NE] = new QuadNode(new Vector2(node.Bounds.Center.X, node.Bounds.Origin.Y),
                                                 node.Bounds.HalfWidth,
                                                 node.Bounds.HalfHeight);
                node[Quadrant.SW] = new QuadNode(new Vector2(node.Bounds.Origin.X, node.Bounds.Center.Y),
                                                 node.Bounds.HalfWidth,
                                                 node.Bounds.HalfHeight);
                node[Quadrant.SE] = new QuadNode(node.Bounds.Center,
                                                 node.Bounds.HalfWidth,
                                                 node.Bounds.HalfHeight);
            }
        }

        private void CheckChildNodes(QuadNode node)
        {
            if(GetQuadObjectCount(node) <= maxLeafObjs)
            {
                // Decompose children
                List<GameObject> childObjects = GetChildObjects(node);
                foreach (GameObject childObject in childObjects)
                {
                    if(!node.Objects.Contains(childObject))
                    {
                        RemoveObjectFromNode(childObject);
                        AddObjectToNode(node, childObject);
                    }
                }
                if (node[Quadrant.NW] != null)
                {
                    node[Quadrant.NW].Parent = null;
                    node[Quadrant.NW] = null;
                }
                if (node[Quadrant.NE] != null)
                {
                    node[Quadrant.NE].Parent = null;
                    node[Quadrant.NE] = null;
                }
                if (node[Quadrant.SW] != null)
                {
                    node[Quadrant.SW].Parent = null;
                    node[Quadrant.SW] = null;
                }
                if (node[Quadrant.SE] != null)
                {
                    node[Quadrant.SE].Parent = null;
                    node[Quadrant.SE] = null;
                }
                if (node.Parent != null)
                    CheckChildNodes(node.Parent);
                else
                {
                    //Root node
                    int numChildrenWithObjects = 0;
                    QuadNode nodeWithObjects = null;
                    foreach (QuadNode child in node.Nodes)
                    {
                        if(child != null && GetQuadObjectCount(child) > 0)
                        {
                            numChildrenWithObjects++;
                            nodeWithObjects = child;
                            if (numChildrenWithObjects > 1) break;
                        }
                    }
                    if(numChildrenWithObjects == 1)
                    {
                        foreach(QuadNode child in node.Nodes)
                        {
                            if (child != nodeWithObjects)
                                child.Parent = null;
                        }
                        root = nodeWithObjects;
                    }
                }
            }

        } //TODO

        private int GetQuadObjectCount (QuadNode node)
        {
            int count = node.Objects.Count;
            foreach (QuadNode child in node.Nodes)
            {
                if (child != null)
                    count += GetQuadObjectCount(child);
            }
            return count;
        }

        private int GetQuadNodeCount(QuadNode node, int count)
        {
            if (node == null) return count;
            foreach (QuadNode child in node.Nodes)
            {
                if (child != null)
                    count++;
            }
            return count;
        }

        private List<GameObject> GetChildObjects(QuadNode node)
        {
            List<GameObject> results = new List<GameObject>();
            results.AddRange(node._nodeObjs);
            foreach(QuadNode child in node.Nodes)
            {
                if (child != null)
                    results.AddRange(GetChildObjects(child));
            }
            return results;
        }

        private void GetChildNodes(QuadNode node, ICollection<QuadNode> results)
        {
            foreach (QuadNode child in node.Nodes)
            {
                if(child != null)
                {
                    results.Add(child);
                    GetChildNodes(child, results);
                }
            }
        }

        #region Event Handling
        private void AddObjectToNode(QuadNode node, GameObject obj)
        {
            node._nodeObjs.Add(obj);
            objectToNodeLookup.Add(obj, node);
            obj.BoundsChanged += new EventHandler(collider_BoundsChanged);
        }

        private void RemoveObjectFromNode(GameObject obj)
        {
            QuadNode node = objectToNodeLookup[obj];
            node._nodeObjs.Remove(obj);
            objectToNodeLookup.Remove(obj);
            obj.BoundsChanged -= new EventHandler(collider_BoundsChanged);
        }

        private void ClearObjectsFromNode(QuadNode node)
        {
            List<GameObject> quadObjects = new List<GameObject>(node.Objects);
            foreach (GameObject quadObject in quadObjects)
            {
                RemoveObjectFromNode(quadObject);
            }
        }

        void collider_BoundsChanged(object sender, EventArgs e)
        {
            GameObject quadObject = sender as GameObject;
            if (quadObject != null)
            {
                QuadNode node = objectToNodeLookup[quadObject];
                if (!node.Bounds.Contains(quadObject.Bounds) || node.HasChildren)
                {
                    RemoveObjectFromNode(quadObject);
                    Insert(quadObject);
                    if (node.Parent != null)
                        CheckChildNodes(node.Parent);
                }
            }
        }
        #endregion
        #endregion
    }
}
