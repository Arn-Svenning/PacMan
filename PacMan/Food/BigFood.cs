using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid.Food
{
    public class BigFood : FoodObjects
    {
        //public bool eatableGhost;
        public float eatTimer;

        public BigFood(Vector2 foodPos, Texture2D foodTex, Rectangle foodRec) : base(foodPos, foodTex, foodRec)
        {
            eatTimer = 3 * 60;
            
        }
        public void Update(Player player)
        {
            if (foodRec.Intersects(player.playerRec))
            {
                eaten = true;
                
            }
            else
            {
                
                eaten = false;
            }

            
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(foodTex, foodPos, Color.White);
        }
    }
}
