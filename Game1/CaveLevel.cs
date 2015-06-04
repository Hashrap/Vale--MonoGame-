//Spencer Corkran
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGen
{
    public class CaveLevel:Map
    {
        public CaveLevel(int size_y, int size_x)
        : base(size_y, size_x, false)
        {
            board = new Tile[base.Size_Y, base.Size_X];
        }

        //This method creates the baseline map to begin the aging process
        public void randomFill(int wall_chance)
        {
            //randomizes everything but the edges of the map
            for (int i = 1; i < base.Size_Y - 1; i++)
            //goes through each row
            {
                for (int j = 1; j < base.Size_X - 1; j++)
                //goes through each column in a row
                {
                    if (rng.Next(100) <= wall_chance)
                        board[i, j] = Tile.Wall;
                    else
                        board[i, j] = Tile.Floor;
                }
            }
        }

        /* This method loops through each tile that isn't on the edge.  If a
         * tile is surrounded by 5+ walls (including itself), it becomes or
         * remains a wall.  Otherwise, it becomes a floor tile.*/
        public void ageDungeon(int variant)
        {
            Tile[,] board2 = board;
            /* all 9 tiles centered around the source
             * (including the source tile)*/
            int yi, xi, ii, jj;
            for (yi = 1; yi < base.Size_Y - 1; yi++)
                for (xi = 1; xi < base.Size_X - 1; xi++)
                {
                    /*running total of surrounding walls,
                     * including the source tile*/
                    int adjWallCount = 0;

                    for (ii = -1; ii <= 1; ii++)
                        for (jj = -1; jj <= 1; jj++)
                        {
                            if (board[yi + ii, xi + jj] != Tile.Floor)
                                adjWallCount++;
                        }
                    /* Variant one tends to 'erode' the walls,
                     * while variant two tends to 'deposit' them.*/
                    switch(variant)
                    {
                        case 2:
                            //5 or more walls OR less than 2 walls surrounding source tile
                            if (adjWallCount >= 5 || adjWallCount < 2)
                                board2[yi, xi] = Tile.Wall;
                            else
                                board2[yi, xi] = Tile.Floor;
                            break;
                        case 1: 
                            //5 or more walls surrounding source tile
                            if (adjWallCount >= 5)
                                board2[yi, xi] = Tile.Wall;
                            else
                                board2[yi, xi] = Tile.Floor;
                            break;
                    }
                }
            //copy the updated board 
            for (yi = 1; yi < base.Size_Y - 1; yi++)
                for (xi = 1; xi < base.Size_X - 1; xi++)
                    board[yi, xi] = board2[yi, xi];
        }
    }
}