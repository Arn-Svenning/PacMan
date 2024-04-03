using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid
{
    public class GameObjects
    {
        //protected Vector2 pos;
        protected Texture2D tex;

        protected Vector2 destination;
        protected Vector2 direction;
        protected float speed;
        protected SpriteEffects spriteEffect;


        /// <summary>
        /// animation variables
        /// </summary>
        protected float elapsedTime;
        protected float frameTime;
        protected int numberOfFrames;
        protected int currentFrame;
        protected int frameWidth;
        protected int frameHeight;
        protected bool looping;
        protected float frameSpeed;
        protected Rectangle animationRect;
        protected enum MoveState { noMove, movingDown, movingUp, movingLeft, movingRight };
        protected MoveState currentMoveState = MoveState.noMove;
        protected enum ghostState { toPlayer, respawn, eatable}
        protected ghostState currentGhostState = ghostState.toPlayer;
        //public enum GameState { start, inGame, end}
        //public GameState currentGameState = GameState.start;

       
        public GameObjects(float speed, Texture2D tex, float frameSpeed, int numberOfFrames, bool looping)
        {
            this.speed = speed;
            this.tex = tex;
            this.frameTime = frameSpeed;
            this.numberOfFrames = numberOfFrames;
            this.looping = looping;

            frameWidth = (tex.Width / numberOfFrames);
            frameHeight = (tex.Height);

        }
    }
}
