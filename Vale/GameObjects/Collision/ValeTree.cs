using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.GameObjects.Collision
{
    #region Definitions
    /// <summary>
    /// Useful shorthand for referring to child nodes
    /// </summary>
    public enum Quadrant : int
    {
        NW = 0,
        NE = 1,
        SW = 2,
        SE = 3
    }
    #endregion

    /// <summary>
    /// Data structure for spatially organizing GameActors
    /// </summary>
    /// <remarks>Used to optimize collision detection by reducing necessary pairwise tests</remarks>
    public class ValeTree
    {
        #region Definitions
        /// <summary>
        /// This nested class represents a node of our quadtree structure
        /// </summary>
        public class QuadNode
        {
            #region Attributes & Properties
            /// <summary>Last ID issued</summary>
            private static int _id = 0;
            /// <summary>Stores this node's unique ID #</summary>
            public readonly int ID = _id++;

            /// <summary>Bounds auto-property</summary>
            /// <value>The area covered by the node as an AABB</value>
            public AABB Bounds { get; internal set; }

            /// <summary>Parent auto-property</summary>
            /// <value>The parent node of this node</value>
            public QuadNode Parent { get; internal set; }

            /// <summary>Stores an array of child nodes for the Nodes wrapper and this property</summary>
            private QuadNode[] _nodes = new QuadNode[4];
            /// <summary>Read-only wrapper for the _nodes attribute</summary>
            /// <remarks>Useful for operations iterating over all children</remarks>
            public ReadOnlyCollection<QuadNode> Nodes;
            /// <summary>'this' property and array mask</summary>
            /// <remarks>Useful for operations on individual children</remarks>
            /// <param name="direction">The child node to get/set</param>
            /// <value>QuadNode to get/set in the corresponding child <paramref name="direction"/></value>
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
                    // If we're adding children, set their parent property
                    if (value != null)
                        value.Parent = this;
                }
            }
            /// <summary>HasChildren property</summary>
            /// <value>Boolean indicating whether the node has children</value>
            public bool HasChildren { get { return _nodes[0] != null; } }

            /// <summary>Internal list of objects located in this node</summary>
            internal List<GameActor> _nodeObjs = new List<GameActor>();
            /// <summary>Read-only wrapper for the _nodeObjs attribute</summary>
            public ReadOnlyCollection<GameActor> Objects;
            #endregion

            #region Constructor(s)
            /// <summary>
            /// QuadNode Constructor
            /// </summary>
            /// <param name="bounds">Used to specify node area</param>
            public QuadNode (AABB bounds)
            {
                Bounds = bounds;
                Nodes = new ReadOnlyCollection<QuadNode>(_nodes);
                Objects = new ReadOnlyCollection<GameActor>(_nodeObjs);
            }

            /// <summary>
            /// QuadNode Constructor Overload
            /// </summary>
            /// <param name="origin">Top-left corner</param>
            /// <param name="width">Node width</param>
            /// <param name="height">Node height</param>
            public QuadNode(Vector2 origin, float width, float height)
                : this(new AABB(origin, width, height)) { }
            #endregion
        }
        #endregion

        #region Attributes
        private QuadNode root = null;
        public QuadNode Root { get { return root; } }

        private Dictionary<GameActor, QuadNode> objectToNodeLookup = new Dictionary<GameActor, QuadNode>();

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
        /// <summary>
        /// Insert an object into the tree
        /// </summary>
        /// <param name="obj">Object to be inserted</param>
        public void Insert(GameActor obj)
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

        /// <summary>
        /// Remove an object from the tree
        /// </summary>
        /// <param name="obj">Object to be removed</param>
        public void Remove(GameActor obj)
        {
            if (!objectToNodeLookup.ContainsKey(obj))
                return; //oops
            QuadNode objNode = objectToNodeLookup[obj];
            RemoveObjectFromNode(obj);
            if (objNode.Parent != null)
                CheckChildNodes(objNode.Parent);

        }

        /// <summary>
        /// Query all objects intersecting an area
        /// </summary>
        /// <param name="bounds">Area to query</param>
        /// <returns>List of all GameActors intersecting with <paramref name="bounds"/></returns>
        public List<GameActor> Query(AABB bounds) 
        {
            List<GameActor> results = new List<GameActor>();
            if (root != null)
                Query(bounds, root, results);
            return results;
        }

        /// <summary>
        /// Query all objects of a different alignment intersecting a GameActor
        /// </summary>
        /// <param name="target">GameActor to query for</param>
        /// <returns>List of all GameActors of a different alignment intersecting with target GameActor </returns>
        public List<GameActor> Query(GameActor target)
        {
            List<GameActor> results = new List<GameActor>();
            if (root != null)
                Query(target.Bounds, root, results, (int)target.Alignment);
            return results;
        }

        /// <summary>
        /// Count all objects in the tree
        /// </summary>
        /// <returns>The number of all objects in the tree</returns>
        public int GetObjectCount()
        {
            if (root == null)
                return 0;
            int count = GetObjectCount(root);
            return count;
        }

        /// <summary>
        /// Count all nodes in the tree
        /// </summary>
        /// <returns>The number of all nodes in the tree</returns>
        public int GetNodeCount()
        {
            if (root == null)
                return 0;
            int count = GetNodeCount(root, 1);
            return count;
        }

        /// <summary>
        /// Get a list of all QuadNodes in the tree
        /// </summary>
        /// <returns>a list of every Quadnode in the tree</returns>
        public List<QuadNode> GetAllNodes()
        {
            List<QuadNode> results = new List<QuadNode>();
            if (root != null)
            {
                results.Add(root);
                GetChildNodes(root, results);
            }
            return results;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Inserts an object into the correct node
        /// </summary>
        /// <remarks>Recursive helper method for public void Insert(GameActor)</remarks>
        /// <param name="node">Node to check for viability</param>
        /// <param name="obj">Object to insert</param>
        private void InsertNodeObject(QuadNode node, GameActor obj)
        {
            if (!node.Bounds.Contains(obj.Bounds))
                return; //oops, object is out of node bounds.  this cannot happen if implemented correctly

            //If the proper node is already full and has no children, split the node into quarters
            if (!node.HasChildren && node.Objects.Count + 1 > maxLeafObjs)
            {
                //split
                SetupChildNodes(node);

                //we need to relocate objects into the new children
                List<GameActor> childObjects = new List<GameActor>(node.Objects);
                List<GameActor> childrenToRelocate = new List<GameActor>();

                //Count objects that fit entirely into a child
                foreach (GameActor childObject in childObjects)
                {
                    foreach (QuadNode child in node.Nodes)
                    {
                        if (child == null)
                            continue;
                        if (child.Bounds.Contains(childObject.Bounds))
                            childrenToRelocate.Add(childObject);
                    }
                }

                //Then move those objects into the appropriate child node
                foreach (GameActor childObject in childrenToRelocate)
                {
                    RemoveObjectFromNode(childObject);
                    InsertNodeObject(node, childObject);
                }
            }

            //If the node has already been split, we recurse into the appropriate child node and go again
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

            // If we made it here, it means a.) the node still has room for more objects before splitting
            // or b.) the object is not entirely contained within just one child node, therefore we have
            // found the correct node to hold this object
            //AddObjectToNode will attach an event listener and index the object's location in a dictionary
            AddObjectToNode(node, obj);
        }

        /// <summary>
        /// Expand the size of the root node
        /// </summary>
        /// <param name="newChildbounds">new object bounds outside of current root</param>
        private void ExpandRoot(AABB newChildbounds)
        {
            bool isNorth = root.Bounds.Y < newChildbounds.Y;
            bool isWest = root.Bounds.X < newChildbounds.X;

            //Determine which child node the root will become
            Quadrant rootQuadrant;
            if (isNorth)
                rootQuadrant = isWest ? Quadrant.NW : Quadrant.NE;
            else
                rootQuadrant = isWest ? Quadrant.SW : Quadrant.SE;

            //Create new root size
            float newX = (rootQuadrant == Quadrant.NW || rootQuadrant == Quadrant.SW)
                ? root.Bounds.X
                : root.Bounds.X - root.Bounds.Width;
            float newY = (rootQuadrant == Quadrant.NW || rootQuadrant == Quadrant.NE)
                ? root.Bounds.Y
                : root.Bounds.Y - root.Bounds.Height;

            //Create the new root
            AABB newRootBounds = new AABB(new Vector2(newX, newY), root.Bounds.Width * 2, root.Bounds.Height * 2);
            QuadNode newRoot = new QuadNode(newRootBounds);
            //Setup the new root's children
            SetupChildNodes(newRoot);
            newRoot[rootQuadrant] = root;
            //Change the root location
            root = newRoot;
        }

        /// <summary>
        /// Recursively populate a list of query results
        /// </summary>
        /// <remarks>Recursive helper method for public List() Query(AABB) method</remarks>
        /// <param name="bounds">Target area</param>
        /// <param name="node">Node to check</param>
        /// <param name="results">List reference to populate</param>
        private void Query(AABB bounds, QuadNode node, List<GameActor> results, int layer = -1)
        {
            if (node == null) return;

            //The target area intersects the current node
            if (bounds.Intersects(node.Bounds))
            {
                foreach (GameActor quadObject in node.Objects)
                {
                    // NOTE: currently adds object to list ONLY in the case of object bounds intersecting with the target
                    //bounds & faction mismatch. We may wish to instead to have it just return all potential collisions
                    //and let the GameplayScreen or the ICollide object handle it themselves.

                    // If the compared object is of a different alignment and has intersecting bounds, add it to the results
                    if ((int)quadObject.Alignment != layer && bounds.Intersects(quadObject.Bounds))
                        results.Add(quadObject);
                }

                // Recurse into child nodes
                foreach (QuadNode child in node.Nodes)
                    Query(bounds, child, results, layer);
            }
        }

        /// <summary>
        /// Splits a node into four child nodes and initializes each one
        /// </summary>
        /// <param name="node">Node to split</param>
        private void SetupChildNodes(QuadNode node)
        {
            // Make sure we won't go under the minimum leaf size
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

        /// <summary>
        /// Merge child nodes into the parent node if possible
        /// </summary>
        /// <param name="node">Node to check</param>
        private void CheckChildNodes(QuadNode node)
        {
            if(GetObjectCount(node) <= maxLeafObjs)
            {
                // Merge children
                List<GameActor> childObjects = GetChildObjects(node);
                foreach (GameActor childObject in childObjects)
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
                // Recurse and check the parent node as well
                if (node.Parent != null)
                    CheckChildNodes(node.Parent);
                else
                {
                    // We're in the root node
                    int numChildrenWithObjects = 0;
                    QuadNode nodeWithObjects = null;
                    foreach (QuadNode child in node.Nodes)
                    {
                        if(child != null && GetObjectCount(child) > 0)
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

        }

        /// <summary>
        /// Count all objects in a node and its children
        /// </summary>
        /// <remarks>Recursive helper method for public int GetObjectCount()</remarks>
        /// <param name="node">Node to count from</param>
        /// <returns>Number of objects found in <paramref name="node"/> and its children</returns>
        private int GetObjectCount (QuadNode node)
        {
            int count = node.Objects.Count;
            foreach (QuadNode child in node.Nodes)
            {
                if (child != null)
                    count += GetObjectCount(child);
            }
            return count;
        }

        /// <summary>
        /// Count all child nodes in a parent node
        /// </summary>
        /// <remarks>Helper method for public int GetNodeCount()</remarks>
        /// <param name="node">Node to check</param>
        /// <param name="count">Number of nodes already counted</param>
        /// <returns>Number of child nodes parented by <paramref name="node"/></returns>
        private int GetNodeCount(QuadNode node, int count)
        {
            if (node == null) return count;
            foreach (QuadNode child in node.Nodes)
            {
                if (child != null)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Return a list of all objects held by a node's children
        /// </summary>
        /// <param name="node">Node to check</param>
        /// <returns>List of all objects in <paramref name="node"/>'s children</returns>
        private List<GameActor> GetChildObjects(QuadNode node)
        {
            List<GameActor> results = new List<GameActor>();
            results.AddRange(node._nodeObjs);
            foreach(QuadNode child in node.Nodes)
            {
                if (child != null)
                    results.AddRange(GetChildObjects(child));
            }
            return results;
        }

        /// <summary>
        /// Return a collection of a node's children
        /// </summary>
        /// <param name="node">Node to retrieve children from</param>
        /// <param name="results">Collection reference to store <paramref name="node"/>'s children</param>
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
        /// <summary>
        /// Associate <paramref name="obj"/> with a <paramref name="node"/>
        /// </summary>
        /// <remarks>Adds the object to the node's records, adds the object/node pair to the
        /// object lookup dictionary, and adds an event listener to the object</remarks>
        /// <param name="node">Node to add <paramref name="obj"/> to</param>
        /// <param name="obj">Object to add to <paramref name="node"/></param>
        private void AddObjectToNode(QuadNode node, GameActor obj)
        {
            node._nodeObjs.Add(obj);
            objectToNodeLookup.Add(obj, node);
            obj.BoundsChanged += new EventHandler(collider_BoundsChanged);
        }

        /// <summary>
        /// Disassociate <paramref name="obj"/> with its QuadNode
        /// </summary>
        /// <remarks>Removes an object from its node's records, removes its dictionary
        /// entry, and removes the event listener from the object</remarks>
        /// <param name="obj">Object to remove</param>
        private void RemoveObjectFromNode(GameActor obj)
        {
            QuadNode node = objectToNodeLookup[obj];
            node._nodeObjs.Remove(obj);
            objectToNodeLookup.Remove(obj);
            obj.BoundsChanged -= new EventHandler(collider_BoundsChanged);
        }

        /// <summary>
        /// Remove all objects from a QuadNode
        /// </summary>
        /// <remarks>Iterates through all objects held in <paramref name="node"/> and
        /// calls RemoveObjectFromNode(GameActor) on each one</remarks>
        /// <param name="node">Node to clear objects from</param>
        private void ClearObjectsFromNode(QuadNode node)
        {
            List<GameActor> quadObjects = new List<GameActor>(node.Objects);
            foreach (GameActor quadObject in quadObjects)
            {
                RemoveObjectFromNode(quadObject);
            }
        }

        /// <summary>
        /// Event handler for moving ICollide objects
        /// </summary>
        /// <remarks>Called whenever an object's bounds move; Removes and reinserts the object if it must change nodes</remarks>
        /// <param name="sender">ICollide object raising the event</param>
        /// <param name="e">EventArgs</param>
        void collider_BoundsChanged(object sender, EventArgs e)
        {
            GameActor quadObject = sender as GameActor;
            if (quadObject != null)
            {
                //Check if the object can remain in its current node
                QuadNode node = objectToNodeLookup[quadObject];
                if (!node.Bounds.Contains(quadObject.Bounds) || node.HasChildren)
                {
                    // Object no longer fits. Easiest fix is to remove the object completely and reinsert it from the root
                    RemoveObjectFromNode(quadObject);
                    Insert(quadObject);
                    // Check whether the removal enables the node's children to merge
                    if (node.Parent != null)
                        CheckChildNodes(node.Parent);
                }
            }
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Iterate through the nodes and draw them
        /// </summary>
        /// <param name="texture">Texture to draw with</param>
        /// <param name="spriteBatch">SpriteBatch to draw with</param>
        public void Draw(Texture2D texture, SpriteBatch spriteBatch)
        {
            foreach (QuadNode node in GetAllNodes())
            {
                DrawBorder(node.Bounds, texture, spriteBatch);
            }
        }

        /// <summary>
        /// Draw a highlight or border around a rectangle
        /// </summary>
        /// <param name="aabb">Bounds of the rectangle to border</param>
        /// <param name="texture">Texture to draw with</param>
        /// <param name="spriteBatch">SpriteBatch to draw with</param>
        private void DrawBorder(AABB aabb, Texture2D texture, SpriteBatch spriteBatch)
        {
            Rectangle rect = aabb.ToRectangle();
            spriteBatch.Draw(texture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(texture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
            spriteBatch.Draw(texture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(texture, new Rectangle(rect.Right, rect.Top, 1, rect.Height + 1), Color.Red);
        }
        #endregion
        #endregion
    }
}
