using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid
{
    public class AssetManager
    {
        public static Texture2D pacman;
        public static Texture2D pacmanStart;
        public static Texture2D empty;
        public static Texture2D red;
        public static Texture2D redStart;
        public static Texture2D wall;
        public static Texture2D green;
        public static Texture2D greenStart;
        public static Texture2D pink;
        public static Texture2D pinkStart;
        public static Texture2D food;
        public static Texture2D ghostEyes;
        public static Texture2D bigFood;
        public static Texture2D ghostFear;
        public static Texture2D ready;
        public static Texture2D set;
        public static Texture2D go;
        public static Texture2D startScreen;
        public static Texture2D lives;
        public static Texture2D spaceStart;

        public static Texture2D trophy;

        public static SpriteFont font;
        public static Effect normalEffect;

        public static SoundEffect waka;

        public Vector2 pos, dims, frameSize;

        public Texture2D myModel;

        MouseState newMouse;

        public AssetManager(Texture2D myModel, Vector2 POS, Vector2 DIMS)
        {
            pos = new Vector2(POS.X, POS.Y);
            dims = new Vector2(DIMS.X, DIMS.Y);


            this.myModel = myModel;


        }
        public virtual bool Hover(Vector2 OFFSET)
        {
            return HoverImg(OFFSET);
        }

        public virtual bool HoverImg(Vector2 OFFSET)
        {
            newMouse = Mouse.GetState();

            Vector2 mousePos = new Vector2(newMouse.X, newMouse.Y);

            if (mousePos.X >= (pos.X + OFFSET.X) - dims.X / 2 && mousePos.X <= (pos.X + OFFSET.X) + dims.X / 2 && mousePos.Y >= (pos.Y + OFFSET.Y) - dims.Y / 2 && mousePos.Y <= (pos.Y + OFFSET.Y) + dims.Y / 2)
            {
                return true;
            }

            return false;
        }

        public virtual void Draw(Vector2 OFFSET, SpriteBatch spriteBatch)
        {
            if (myModel != null)
            {
                spriteBatch.Draw(myModel, new Rectangle((int)(pos.X + OFFSET.X), (int)(pos.Y + OFFSET.Y), (int)dims.X, (int)dims.Y), null, Color.White, 0, new Vector2(myModel.Bounds.Width / 2, myModel.Bounds.Height / 2), new SpriteEffects(), 0);
            }
        }

        public static void Load(ContentManager content)
        {
            pacman = content.Load<Texture2D>("PacMan spriteSheet");
            pacmanStart = content.Load<Texture2D>("pecboy_0");
            empty = content.Load<Texture2D>("empty-Tex_Pacman");
            red = content.Load<Texture2D>("ghost-red-sheet");
            redStart = content.Load<Texture2D>("ghost_red1");
            green = content.Load<Texture2D>("ghost-green-sheet");
            greenStart = content.Load<Texture2D>("ghost_green1");
            pink = content.Load<Texture2D>("ghost-pink-sheet");
            pinkStart = content.Load<Texture2D>("ghost_pink1");
            wall = content.Load<Texture2D>("black-tile");
            food = content.Load<Texture2D>("point");
            ghostEyes = content.Load<Texture2D>("ghost_eyes");
            bigFood = content.Load<Texture2D>("point-big");
            ghostFear = content.Load<Texture2D>("ghost-fear-sheet");
            ready = content.Load<Texture2D>("Ready");
            set = content.Load<Texture2D>("Set");
            go = content.Load<Texture2D>("Go");
            startScreen = content.Load<Texture2D>("icon");
            lives = content.Load<Texture2D>("pecboy_2");
            spaceStart = content.Load<Texture2D>("space-to-start");

            font = content.Load<SpriteFont>("font");
            trophy = content.Load<Texture2D>("trophy");

            waka = content.Load<SoundEffect>("waka waka");
        }
    }
}
