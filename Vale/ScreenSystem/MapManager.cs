﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace Vale.ScreenSystem
{
    class MapManager
    {
        [FlagsAttribute]
        public enum State { Walkable = 0x1 }
        public struct Tile
        {
            private int x;
            public int X { get; set; }
            private int y;
            public int Y { get; set; }
            private int width;
            public int Width { get; set; }
            private int height;
            public int Height { get; set; }

            private State _state;
            public State State { get; set; }
        }

        private Tile[,] _map;

        public MapManager()
        {

        }

        public void Load(string filename)
        { }

        public void Import(int[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            _map = new Tile[width, height];

            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    _map[x, y].State = (State)map[x, y];
                    
                }
            }
        }
    }
}