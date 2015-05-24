using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vale.GameObjects.Skills.Projectiles
{
    class ReturnProjectile : LineProjectile
    {
        private int travelDuration = 750;
        private int elapsedTime = 0;
        private bool returning = false;

        public ReturnProjectile(Game1 game, SpriteBatch spriteBatch, string textureName)
            : base(game, spriteBatch, textureName)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!returning && ((elapsedTime += gameTime.ElapsedGameTime.Milliseconds) >= travelDuration))
            {
                //returning
                Velocity *= -1;
                returning = true;
            }

        }
    }
}
