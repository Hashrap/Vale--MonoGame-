//Spencer Corkran

using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonGen
{
    public class DungeonLevel : Map
    {
        internal class Node
        {
            internal static int _minimumLeafSize, _minimumWidth, _minimumHeight;
            internal static DungeonLevel level;
            internal Rectangle Bounds { get; set; }
            internal Node parent;
            internal Node[] children = new Node[2];
            internal bool HasChildren { get { return !(children[0] == null && children[1] == null); } }
            internal bool isRoot { get { return parent == null; } }
            internal Rectangle Room { get; set; }

            public Node(int top, int left, int width, int height, Node nodeParent = null)
            {
                Bounds = new Rectangle(top, left, width, height);
                parent = nodeParent;
            }

            public void Split(double min_position, double max_position)
            {
                int split;
                double splitRatio = (double)rng.Next((int)Math.Round(min_position * 100), (int)Math.Round(max_position * 100)) / 100; 

                //Vertical
                if (rng.Next(99) % 2 == 1 && Bounds.Width / 2 > _minimumWidth)
                {
                    split = (int)Math.Round(Bounds.Width * splitRatio);
                    if (split >= Node._minimumWidth &&
                        Bounds.Width - split >= Node._minimumWidth)
                    {
                        children[0] = new Node(Bounds.Top, Bounds.Left, split, Bounds.Height);
                        children[1] = new Node(Bounds.Top, Bounds.Left + split, Bounds.Width - split, Bounds.Height);
                    }
                }
                //Horizontal
                else if (Bounds.Height / 2 > _minimumHeight)
                {
                    split = (int)Math.Round((decimal)(Bounds.Height * splitRatio));
                    if (split >= Node._minimumHeight &&
                        Bounds.Height - split >= Node._minimumHeight)
                    {
                        children[0] = new Node(Bounds.Top, Bounds.Left, Bounds.Width, split);
                        children[1] = new Node(Bounds.Top + split, Bounds.Left, Bounds.Width, Bounds.Height - split);
                    }
                }

                if (HasChildren)
                {
                    foreach (Node child in children)
                    {
                        if (child != null)
                            child.Split(min_position, max_position);
                    }
                }
                else
                    CreateRoom();
            }

            public void CreateRoom()
            {
                int width = rng.Next(2, Bounds.Width - 2);
                int height = rng.Next(2, Bounds.Height - 2);
                int top = rng.Next(Bounds.Top + 1, (Bounds.Bottom - height) - 1);
                int left = rng.Next(Bounds.Left + 1, (Bounds.Right - width) - 1);

                level.CarveRoom(new Rectangle(top, left, width, height));
            }
        }

        internal struct Rectangle
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public int Top { get { return Y; } }
            public int Left { get { return X; } }
            public int Bottom { get { return Y + Height; } }
            public int Right { get { return X + Width; } }
            public int Width { get; private set; }
            public int Height { get; private set; }
            public int Area { get { return Width * Height; } }

            public Rectangle(int top, int left, int width, int height)
                : this()
            {
                X = left;
                Y = top;
                Width = width;
                Height = height;
            }

            public Rectangle(Point a, Point b)
                : this(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y))
            {
            }
        }

        Node root;

        public DungeonLevel(int size_x, int size_y)
            : base(size_y, size_x)
        {
            board = new Tile[base.Size_X, base.Size_Y];
            for (int x = 0; x < base.Size_Y; x++)
            //goes through each row
            {
                for (int y = 0; y < base.Size_X; y++)
                //goes through each column in a row
                {
                    board[x, y] = Tile.Wall;
                }
            }
        }
        
        public void DungeonGen(double min_position, double max_position, int minimum_width, int minimum_height)
        {
            Node._minimumWidth = minimum_width;
            Node._minimumHeight = minimum_height;
            Node._minimumLeafSize = minimum_height * minimum_width;
            Node.level = this;

            root = new Node(0,0,board.GetLength(0), board.GetLength(1));
            
            root.Split(min_position, max_position);
        }

        internal List<Node> GetLeaves()
        {
            List<Node> results = new List<Node>();
            GetLeaves(root, results);
            return results;
        }

        private void GetLeaves(Node node, List<Node> results)
        {
            if (!node.HasChildren)
                results.Add(node);
            else
            {
                foreach (Node child in node.children)
                {
                    GetLeaves(child, results);
                }
            }
        }

        private void CarveRoom(Rectangle bounds)
        {
            for (int x = bounds.Left; x < bounds.Right; x++)
            {
                for (int y = bounds.Top; y < bounds.Bottom; y++)
                {
                    board[x, y] = Tile.Walkable;
                }
            }
        }
    }
}