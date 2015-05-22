using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vale
{
    public interface IUpdatable
    {
        void Update(GameTime gameTime);
    }
}
