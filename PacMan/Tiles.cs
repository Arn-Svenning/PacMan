using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid
{
    public class Tiles
    {
        public Texture2D tex;
        public Vector2 pos;
        public bool wall;

        public Tiles(Texture2D tex, Vector2 pos, bool wall)
        {
            this.tex = tex;
            this.pos = pos;
            this.wall = wall;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, Color.White);
        }
    }
}
