using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Grid.Food
{
    public class FoodObjects
    {
        protected Vector2 foodPos;
        protected Texture2D foodTex;

        public Rectangle foodRec;
        public bool eaten;
        public bool eatableGhost;

        public FoodObjects(Vector2 foodPos, Texture2D foodTex, Rectangle foodRec)
        {
            this.foodPos = foodPos;
            this.foodTex = foodTex;

            eatableGhost = false;
            eaten = false;
            this.foodRec = foodRec;
        }
    }
}
