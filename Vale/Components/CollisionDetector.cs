using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vale.ScreenSystem;
using Microsoft.Xna.Framework;

namespace Vale.Components
{
    public class CollisionDetector
    {

        private GameplayScreen gameScreen;

        public CollisionDetector(GameplayScreen gameScreen)
        {
            this.gameScreen = gameScreen;
        }

        public Vector2 NextPlayerPosition
        {
            get;
            set;
        }

        public void Update(GameTime gameTime)
        {
            this.gameScreen.Player.NextPosition = CalculateTileCollisionCorrection(
                    objectPosition: this.gameScreen.Player.Position,
                    objectVelocity: this.gameScreen.Player.Velocity,
                    objectWidth: this.gameScreen.Player.BoundingBox.Width,
                    objectHeight: this.gameScreen.Player.BoundingBox.Height,
                    tileWidth: this.gameScreen.GameMap.TileWidth,
                    tileHeight: this.gameScreen.GameMap.TileHeight);

            bool eventDetect = false;
            foreach (var npc in this.gameScreen.Npcs)
            {
                if (this.gameScreen.Player.BoundingBox.Intersects(npc.EventBox))
                {
                    eventDetect = true;
                    this.gameScreen.Player.InteractionEventDetect(npc);
                }

                // TODO: Fix Jitter.
                // Take into account the player's next position, whether determined already by tile collisions
                // or if it still needs to be calculated. Then resolve collisions independently on each axis.
                #region Solid Collision
                if (this.gameScreen.Player.BoundingBox.Intersects(npc.BoundingBox))
                {
                    Vector2 playerPosition;
                    if (this.gameScreen.Player.NextPosition == Vector2.Zero)
                    {
                        playerPosition = this.gameScreen.Player.Position + this.gameScreen.Player.Velocity;
                    }
                    else
                    {
                        playerPosition = this.gameScreen.Player.NextPosition;
                    }
                    float xCorrection = HorizontalCorrectionCalculation(
                        positionA: playerPosition,
                        widthA: this.gameScreen.Player.BoundingBox.Width,
                        heightA: this.gameScreen.Player.BoundingBox.Height,
                        positionB: npc.Position,
                        widthB: npc.BoundingBox.Width,
                        heightB: npc.BoundingBox.Height
                        );
                    float yCorrection = VerticalCorrectionCalculation(
                        positionA: playerPosition,
                        widthA: this.gameScreen.Player.BoundingBox.Width,
                        heightA: this.gameScreen.Player.BoundingBox.Height,
                        positionB: npc.Position,
                        widthB: npc.BoundingBox.Width,
                        heightB: npc.BoundingBox.Height
                        );
                    if (Math.Abs(xCorrection) < Math.Abs(yCorrection))
                    {
                        if (xCorrection > 0)
                        {
                            // Correct to the right.
                            playerPosition.X = npc.BoundingBox.Left + npc.BoundingBox.Width;
                        }
                        else
                        {
                            // Correct to the left.
                            playerPosition.X = npc.BoundingBox.Left - this.gameScreen.Player.BoundingBox.Width;
                        }
                    }
                    else if (Math.Abs(yCorrection) < Math.Abs(xCorrection))
                    {
                        if (yCorrection > 0)
                        {
                            // Correct to the bottom
                            playerPosition.Y = npc.BoundingBox.Bottom;
                        }
                        else
                        {
                            playerPosition.Y = npc.BoundingBox.Top - this.gameScreen.Player.BoundingBox.Height;
                        }
                    }
                    else
                    {
                        playerPosition.X += xCorrection;
                        playerPosition.Y += yCorrection;
                        this.gameScreen.Player.Velocity = Vector2.Zero;
                    }

                    if (this.gameScreen.Player.NextPosition != Vector2.Zero)
                    {
                        this.gameScreen.Player.NextPosition = playerPosition;
                    }
                    else if ((int)this.gameScreen.Player.Position.X != (int)playerPosition.X
                        || (int)this.gameScreen.Player.Position.Y != (int)playerPosition.Y)
                    {
                        this.gameScreen.Player.NextPosition = playerPosition;
                    }
                }
                #endregion Solid Collision
            }

            if (!eventDetect)
            {
                this.gameScreen.Player.ClearEvent();
            }
            // Calculate collisions for NPCs
            //foreach (var npc in this.gameScreen.Npcs)
            //{
            //    npc.NextPosition = CalculateTileCollisionCorrection(
            //        objectPosition: npc.Position,
            //        objectVelocity: npc.Velocity,
            //        objectWidth: npc.BoundingBox.Width,
            //        objectHeight: npc.BoundingBox.Height,
            //        tileWidth: this.gameScreen.GameMap.TileWidth,
            //        tileHeight: this.gameScreen.GameMap.TileHeight);
            //}
            // Calculate collisions for the enemies
        }

        private float HorizontalCorrectionCalculation(Vector2 positionA, float widthA, float heightA, Vector2 positionB, float widthB, float heightB)
        {
            float halfWidthA = widthA / 2.0f;
            float halfWidthB = widthB / 2.0f;
            float centerA = positionA.X + halfWidthA;
            float centerB = positionB.X + halfWidthB;
            float distanceX = centerA - centerB;
            float minDistanceX = halfWidthA + halfWidthB;
            if (Math.Abs(distanceX) >= minDistanceX)
            {
                // No collision in the X.
                return 0;
            }
            else
            {
                return (distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX);
            }
        }

        private float VerticalCorrectionCalculation(Vector2 positionA, float widthA, float heightA, Vector2 positionB, float widthB, float heightB)
        {
            float halfHeightA = heightA / 2.0f;
            float halfHeightB = heightB / 2.0f;
            float centerA = positionA.Y + halfHeightA;
            float centerB = positionB.Y + halfHeightB;
            float distanceY = centerA - centerB;
            float minDistanceY = halfHeightA + halfHeightB;
            if (Math.Abs(distanceY) >= minDistanceY)
            {
                // No collision in the Y.
                return 0;
            }
            else
            {
                return (distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY);
            }
        }

        private Vector2 CalculateTileCollisionCorrection(Vector2 objectPosition, Vector2 objectVelocity, int objectWidth, int objectHeight, int tileWidth, int tileHeight)
        {
            bool collisionX = false;
            bool collisionY = false;

            // Update projected position in the X direction
            objectPosition = new Vector2(objectPosition.X + objectVelocity.X, objectPosition.Y);
            Rectangle objectBounds = new Rectangle((int)(objectPosition.X), (int)(objectPosition.Y), objectWidth, objectHeight);
            int leftTile = objectBounds.Left / tileWidth;
            int topTile = objectBounds.Top / tileHeight;
            int rightTile = (int)Math.Ceiling((float)objectBounds.Right / tileWidth) - 1;
            int bottomTile = (int)Math.Ceiling(((float)objectBounds.Bottom / tileHeight)) - 1;

            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    if (x >= 0 && x < this.gameScreen.GameMap.WorldWidth && y >= 0 && y < this.gameScreen.GameMap.WorldHeight &&
                        this.gameScreen.GameMap.CollisionLayer[y, x] != null)
                    {

                        Rectangle tileBox = this.gameScreen.GameMap.CollisionLayer[y, x];
                        float halfWidthA = objectBounds.Width / 2.0f;
                        float halfWidthB = tileBox.Width / 2.0f;
                        float centerA = objectPosition.X + halfWidthA;
                        float centerB = tileBox.Left + halfWidthB;
                        float distanceX = centerA - centerB;
                        float minDistanceX = halfWidthA + halfWidthB;
                        if (Math.Abs(distanceX) >= minDistanceX)
                        {
                            // do nothing
                        }
                        else
                        {
                            collisionX = true;
                            objectPosition += new Vector2(distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX, 0);
                            objectBounds.X = (int)(objectPosition.X);
                        }
                    }
                }
            }

            // Update the object position in the Y direction
            objectPosition = new Vector2(objectPosition.X, objectPosition.Y + objectVelocity.Y);
            objectBounds = new Rectangle((int)(objectPosition.X), (int)(objectPosition.Y), objectWidth, objectHeight);
            leftTile = objectBounds.Left / tileWidth;
            topTile = objectBounds.Top / tileHeight;
            rightTile = (int)Math.Ceiling((float)objectBounds.Right / tileWidth) - 1;
            bottomTile = (int)Math.Ceiling(((float)objectBounds.Bottom / tileHeight)) - 1;

            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    if (x >= 0 && x < this.gameScreen.GameMap.WorldWidth && y >= 0 && y < this.gameScreen.GameMap.WorldHeight &&
                        this.gameScreen.GameMap.CollisionLayer[y, x] != null)
                    {

                        Rectangle tileBox = this.gameScreen.GameMap.CollisionLayer[y, x];
                        float halfHeightA = objectBounds.Height / 2.0f;
                        float halfHeightB = tileBox.Height / 2.0f;
                        float centerA = objectPosition.Y + halfHeightA;
                        float centerB = tileBox.Top + halfHeightB;
                        float distanceY = centerA - centerB;
                        float minDistanceY = halfHeightA + halfHeightB;
                        if (Math.Abs(distanceY) >= minDistanceY)
                        {
                            // do nothing
                        }
                        else
                        {
                            collisionY = true;
                            if (distanceY > 0)
                            {
                                // clear the player downwards.
                                objectPosition.Y = tileBox.Bottom;
                            }
                            else
                            {
                                // Clear the player upwards
                                objectPosition.Y = tileBox.Top - objectBounds.Height;
                            }
                            objectBounds.Y = (int)(objectPosition.Y);
                        }
                    }
                }
            }
            if (collisionX || collisionY)
            {
                if (collisionX)
                {
                    this.gameScreen.Player.Velocity *= Vector2.UnitY;
                }
                if (collisionY)
                {
                    this.gameScreen.Player.Velocity *= Vector2.UnitX;
                }
                return objectPosition;
            }
            else
            {
                return Vector2.Zero;
            }
        }
    }
}
