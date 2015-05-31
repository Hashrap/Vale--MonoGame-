using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vale.GameObjects.Actors;
using Vale.ScreenSystem;

namespace Vale.GameObjects.Skills.Projectiles
{
    class ReturnProjectile : LineProjectile
    {
        private int travelDuration = 750;
        private int elapsedTime = 0;
        private bool returning = false;

        public int timeLeft { get { return travelDuration * 2 - elapsedTime; } }
        public ReturnProjectile(GameScreen gameScreen, Texture2D texture, GameActor owner, Vector2 origin, float rotation, float speed)
            : base(gameScreen, texture, owner, origin, rotation, speed)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (!returning && (elapsedTime >= travelDuration))
            {
                //returning
                //Velocity *= -1; // maybe instead velocity should instead guide towards the Owner?
                returning = true;
            }
            else if (returning)
            {
                UpdateVelocity();
            }
        }

        private void UpdateVelocity()
        {
            Vector2 target = Owner.Position;
            Vector2 currentPosition = Position;

            float xDelta = target.X - currentPosition.X;
            float yDelta = target.Y - currentPosition.Y;
            float distance = (float)Math.Sqrt(Math.Pow(xDelta, 2) + Math.Pow(yDelta, 2));

            float speed = distance / timeLeft;

            Velocity = new Vector2(speed * (xDelta / distance), speed * (yDelta / distance));
        }
    }
}
