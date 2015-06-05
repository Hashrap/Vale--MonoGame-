using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.ScreenSystem.Screens;
using Vale.GameObjects;
using System.Diagnostics;

namespace Vale
{
    public class Camera // TODO: : GameObject
    {
        protected GameplayScreen game;

        public float Zoom { get; set; }
        // TODO: Should be part of GameObject:
        public Vector2 Position { get; set; }

        public float Rotation { get; set; }

        Rectangle Bounds { get; set; }

        GameActor target;

        Matrix TransformMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom) *
                Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            }
        }

        Vector2 worldDimensions;

        // TODO: Add parameter `GameObject target`
        public Camera(GameplayScreen game, Viewport viewport, Vector2 worldDimensions)
        // TODO:   :base(game)
        {
            this.game = game;
            this.worldDimensions = worldDimensions;
            Bounds = viewport.Bounds;
            Debug.WriteLine("Bounds: " + Bounds);
            Zoom = 1;
            Position = new Vector2(0, 0);
            Rotation = 0;
        }

        /// <summary>
        /// Sets the target that the camera is going to focus on.
        /// </summary>
        public void SetTarget(GameActor target)
        {
            this.target = target;
        }

        /// <summary>
        /// Converts from coordinates in the screen space to coordinates in the
        /// world space.
        /// </summary>
        /// <returns>Coordinate in world space.</returns>
        /// <param name="screenPosition">Coordinate on the screen</param>
        public Vector2 ScreenToWorldCoords(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(TransformMatrix));
        }

        /// <summary>
        /// Convert from World coordinates to screen space coordinates.
        /// </summary>
        /// <returns>Screen space coordinate of the given position</returns>
        /// <param name="worldPosition">World position.</param>
        public Vector2 WorldToScreenCoords(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TransformMatrix);
        }

        public void Update(GameTime gameTime)
        {
            Vector2 updatedPosition = target.Position;
            updatedPosition.X = MathHelper.Clamp(target.Position.X, Bounds.Width / 2, worldDimensions.X - Bounds.Width / 2);
            updatedPosition.Y = MathHelper.Clamp(target.Position.Y, Bounds.Height / 2, worldDimensions.Y - Bounds.Height / 2);
            Position = updatedPosition;
            Debug.WriteLine("Updated position: " + updatedPosition);
            Debug.WriteLine("World dimensions: " + worldDimensions);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: TransformMatrix);
        }
    }
}
