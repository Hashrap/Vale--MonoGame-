using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vale.GameObjects
{
    public interface IUpdate
    {
        void Update(GameTime gameTime);
    }
}
