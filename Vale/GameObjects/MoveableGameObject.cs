using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Vale.GameObjects.Collision;
using Vale.ScreenSystem.Screens;

namespace Vale.GameObjects
{
    /// <summary>
    /// Represents a game object in the world that can move.
    /// </summary>
    public class MoveableGameObject : GameObject, IMove
    {
        public MoveableGameObject(GameplayScreen game, Vector2 position, Vector2 size)
            : base(game, position, size)
        {
        }



        public Vector2 Velocity { get; set; }
        public event EventHandler ObjectMoved;
        public event EventHandler ObjectStoppedMoving;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Move(gameTime);
        }


        public Vector2 Move(GameTime gameTime)
        {
            previousBounds = Bounds;
            if (Velocity == Vector2.Zero)
                return Position;

            Vector2 distance = Velocity * gameTime.ElapsedGameTime.Milliseconds;
            bounds = new AABB(Bounds.Origin + distance, PreviousBounds.Width, SpriteHeight);
            if (!Game.Map.Query(Bounds))
                OnTerrainCollision();

            if (Bounds != PreviousBounds)
                RaiseBoundsChanged();

            return Position;
        }
    }
}
