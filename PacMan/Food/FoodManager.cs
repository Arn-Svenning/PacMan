using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid.Food
{
    public class FoodManager 
    {
        public static int score;
        List<Food> foodList = new List<Food>();

        List<BigFood> bigFoodList = new List<BigFood>();

        public Rectangle bigFoodRec;
        public Rectangle foodRec;
       
        string s;

       public bool eatableGhost;
        float eatTimer;
        
        public void LoadFood(string s, Vector2 pos)
        {
            bigFoodRec = new Rectangle((int)pos.X, (int)pos.Y, AssetManager.bigFood.Width / 3, AssetManager.bigFood.Height / 3);
            foodRec = new Rectangle((int)pos.X, (int)pos.Y, AssetManager.food.Width / 3, AssetManager.food.Height / 3);
            eatTimer = 5 * 60;
            if (s == "f")
            {
                foodList.Add(new Food(pos, AssetManager.food, foodRec));
            }
            else if(s == "b")
            {
                bigFoodList.Add(new BigFood(pos, AssetManager.bigFood, bigFoodRec));
            }
        }
        public void Update(GameTime gameTime, Player player)
        {
            

            //// regular food
            foreach (Food food in foodList)
            {
                food.Update(player);
                
            }

            for (int i = 0; i < foodList.Count; i++)
            {
                if (foodList[i].eaten == true)
                {
                    foodList.Remove(foodList[i]);
                    score += 10;
                }
               

            }
            if (foodList.Count == 0)
            {
                Game1.currentGameState = Game1.GameState.end;
            }
            ///// big food
            foreach (BigFood bigFood in bigFoodList)
            {
                bigFood.Update(player);
            }

            for(int i = 0; i < bigFoodList.Count; i++)
            {
                if(bigFoodList[i].eaten == true)
                {
                    bigFoodList.Remove(bigFoodList[i]);

                    eatableGhost = true;
                    
                }
                
               
            }

            if (eatableGhost == true)
            {

                eatTimer--;

                if (eatTimer <= 0)
                {
                    eatableGhost = false;
                    eatTimer = 5 * 60;
                }
            }
           


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Food food in foodList)
            {
                food.Draw(spriteBatch);
            }
            foreach(BigFood bigFood in bigFoodList)
            {
                bigFood.Draw(spriteBatch);
            }
        }
    }
}
