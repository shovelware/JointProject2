using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Colours
{
    class Enemy
    {
        
        //position and scale
        const int BLOCKWIDTH = 8;
        const int BLOCKHEIGHT = 8;

        const byte TINY = 1, SMAL = 2, MEDI = 4, LARG = 8, HUGE = 16, XBOX = 32;
        byte size;

        public byte Size { get { return size; } }

        Texture2D enemyTexTiny, enemyTexSmal, enemyTexMedi, enemyTexLarg, enemyTexHuge, enemytexXbox;
        Vector2 pos;

        public Vector2 Pos { get { return pos; } }

        Vector2 posadj = new Vector2(BLOCKWIDTH / 2, BLOCKHEIGHT / 2);

        public Vector2 Posadj { get { return posadj; } }

        Vector2 origin = new Vector2(0, 0);
        Random rng = new Random();
        
        //movement
        float speed;

        float patternTime;
        //int tempPatternTime;
        float horTime;
        float verTime;
        sbyte movePattern;
        //sbyte tempPattern;

        const byte MAXPATTERNS = 0;

        const int UPDATE = 60;

        sbyte dirX = 1;// -1 left, 0 none, 1 right
        sbyte dirY = 1;// -1 up, 0 none, 1 down

        public sbyte DirX{ get { return dirX; } }
        public sbyte DirY{ get { return dirY; } }
        
        //combat
        bool alive;
        bool active;

        public bool Alive{ get { return alive; } }
        public bool Active{ get { return active; } }

        int healthmax;
        int health;

        public int HealthMax{ get { return healthmax; } }
        public int Health{ get { return health; } }
        
        const byte WHT = 0, RED = 1, GRN = 2, BLU = 3, CYN = 4, MGN = 5, YLO = 6, BLK = 7;
        byte colour;

        public byte Colour{ get { return colour; } }

        UI hud;

        /// <summary>
        /// Constructs an enemy with default values.
        /// </summary>
        public Enemy(UI ui, Texture2D[] textures)
        {
            hud = ui;
            enemyTexTiny = textures[0];
            enemyTexSmal = textures[1];
            enemyTexMedi = textures[2];
            enemyTexLarg = textures[3];
            enemyTexHuge = textures[4];
            enemytexXbox = textures[5];

            pos.X = rng.Next(0, 641);
            pos.Y = 500;

            size = TINY;
            colour = WHT;

            healthmax = size * 8;
            health = healthmax;

            alive = true;
            active = true;
        }

        public Enemy(UI ui, Texture2D[] textures, byte startsize, byte startcolour)
        {
            hud = ui;
            enemyTexTiny = textures[0];
            enemyTexSmal = textures[1];
            enemyTexMedi = textures[2];
            enemyTexLarg = textures[3];
            enemyTexHuge = textures[4];
            enemytexXbox = textures[5];


            pos.X = rng.Next(0, 641);
            pos.Y = 500;

            if (startsize == TINY || startsize == SMAL || startsize == MEDI || startsize == LARG || startsize == HUGE || startsize == XBOX)
            {
                size = startsize;
            }

            else
            {
                size = TINY;
            }

            if (startcolour > 0 && startcolour < 7)
            {
                colour = startcolour;
            }

            healthmax = size * 8;
            health = healthmax;

            alive = true;
            active = true;
        }

        public Enemy(UI ui, Texture2D[] textures, byte startsize, byte startcolour, int posX, int posY)
        {
            hud = ui;
            enemyTexTiny = textures[0];
            enemyTexSmal = textures[1];
            enemyTexMedi = textures[2];
            enemyTexLarg = textures[3];
            enemyTexHuge = textures[4];
            enemytexXbox = textures[5];

            pos.X = posX;
            pos.Y = posY;

            if (startsize == TINY || startsize == SMAL || startsize == MEDI || startsize == LARG || startsize == HUGE || startsize == XBOX)
            {
                size = startsize;
            }

            else
            {
                size = TINY;
            }

            if (startcolour > 0 && startcolour < 7)
            {
                colour = startcolour;
            }

            healthmax = size * 8;
            health = healthmax;

            alive = true;
            active = true;
        }

        /// <summary>
        /// Creates a new enemy with passed values
        /// </summary>
        /// <param name="ui">UI for drawing status</param>
        /// <param name="textures">Array of enemy textures</param>
        /// <param name="startsize">Start size, 1 Tiny, 2 Small, 4 Medium, 8 Large, 16 Huge, 32 Xbox, 64 RNG</param>
        /// <param name="startcolour">Start colour, 0 White, 1 Red, 2 Green, 3 Blue, 4 Cyan, 5 Magenta, 6 Yellow, 7 Black, 8 RNG RGBCMY, 9 RNG All</param>
        /// <param name="posX">X position, use 9999 to RNG</param>
        /// <param name="posY">Y position, use 9999 to RNG near top, 99999 to RNG anywhere</param>
        /// <param name="startpattern">Start move pattern, 0 to 5, straight, zigzag1, zigdownzagdown, downleftdownright, dldldrdr, wavy</param>
        public Enemy(UI ui, Texture2D[] textures, byte startsize, byte startcolour, int posX, int posY, sbyte startpattern)
        {


            hud = ui;
            enemyTexTiny = textures[0];
            enemyTexSmal = textures[1];
            enemyTexMedi = textures[2];
            enemyTexLarg = textures[3];
            enemyTexHuge = textures[4];
            enemytexXbox = textures[5];
            if (posX == 9999)
            {
                pos.X = rng.Next(0, 641);
            }

            else
            {
                pos.X = posX;
            }

            if (posY == 9999)
            {
                pos.Y = rng.Next(0, 100);
            }

            else if (posY == 99999)
            {
                pos.Y = rng.Next(0, 800);
            }

            else
            {
                pos.Y = posY;
            }


            if (startsize == TINY || startsize == SMAL || startsize == MEDI || startsize == LARG || startsize == HUGE || startsize == XBOX)
            {
                size = startsize;
            }

            else if (startsize == 64)
            {
                switch (RollNumber(1, 6))
                {
                    case 1:
                        size = TINY;
                        break;
                    case 2:
                        size = SMAL;
                        break;
                    case 3:
                        size = MEDI;
                        break;
                    case 4:
                        if (RollDice(1, 16))
                        {
                            size = LARG;
                        }
                        else
                        {
                            size = MEDI;
                        }
                        break;
                    case 5:
                        if (RollDice(1, 16))
                        {
                            size = HUGE;
                        }
                        else
                        {
                            size = LARG;
                        }
                        break;
                    case 6:
                        if (RollDice(1, 16))
                        {
                            size = XBOX;
                        }
                        else
                        {
                            size = HUGE;
                        }
                        break;
                }
            }

            else
            {
                size = TINY;
            }

            if (startcolour > 0 && startcolour < 7)
            {
                colour = startcolour;
            }

            else if (startcolour == 8)
            {
                colour = (byte)RollNumber(1, 6);
            }

            else if (startcolour == 9)
            {
                colour = (byte)RollNumber(0, 7);
            }

            if (startpattern >= 0 && startpattern <= MAXPATTERNS)
            {
                movePattern = startpattern;
            }

            healthmax = size * 8;
            health = healthmax;

            alive = true;
            active = true;
        }

        /// <summary>
        /// Draws the enemy.
        /// </summary>
        public void Draw(SpriteBatch sprbat)
        {
            if (active)
            {
                switch (size)
                {
                    case TINY:
                        sprbat.Draw(enemyTexTiny, new Rectangle((int)(pos.X - posadj.X * size), (int)(pos.Y - posadj.Y * size), BLOCKWIDTH * size, BLOCKHEIGHT * size), new Rectangle(colour * BLOCKWIDTH * size, 0, BLOCKWIDTH * size, BLOCKHEIGHT * size), Color.White);
                        break;

                    case SMAL:
                        sprbat.Draw(enemyTexSmal, new Rectangle((int)(pos.X - posadj.X * size), (int)(pos.Y - posadj.Y * size), BLOCKWIDTH * size, BLOCKHEIGHT * size), new Rectangle(colour * BLOCKWIDTH * size, 0, BLOCKWIDTH * size, BLOCKHEIGHT * size), Color.White);
                        break;

                    case MEDI:
                        sprbat.Draw(enemyTexMedi, new Rectangle((int)(pos.X - posadj.X * size), (int)(pos.Y - posadj.Y * size), BLOCKWIDTH * size, BLOCKHEIGHT * size), new Rectangle(colour * BLOCKWIDTH * size, 0, BLOCKWIDTH * size, BLOCKHEIGHT * size), Color.White);
                        break;

                    case LARG:
                        sprbat.Draw(enemyTexLarg, new Rectangle((int)(pos.X - posadj.X * size), (int)(pos.Y - posadj.Y * size), BLOCKWIDTH * size, BLOCKHEIGHT * size), new Rectangle(colour * BLOCKWIDTH * size, 0, BLOCKWIDTH * size, BLOCKHEIGHT * size), Color.White);
                        break;

                    case HUGE:
                        sprbat.Draw(enemyTexHuge, new Rectangle((int)(pos.X - posadj.X * size), (int)(pos.Y - posadj.Y * size), BLOCKWIDTH * size, BLOCKHEIGHT * size), new Rectangle(colour * BLOCKWIDTH * size, 0, BLOCKWIDTH * size, BLOCKHEIGHT * size), Color.White);
                        break;

                    case XBOX:
                        sprbat.Draw(enemytexXbox, new Rectangle((int)(pos.X - posadj.X * size), (int)(pos.Y - posadj.Y * size), BLOCKWIDTH * size, BLOCKHEIGHT * size), new Rectangle(colour * BLOCKWIDTH * size, 0, BLOCKWIDTH * size, BLOCKHEIGHT * size), Color.White);
                        break;
                }
            }
        }

        /// <summary>
        /// Moves the enemy. 
        /// </summary>
        public void Move()
        {
            switch (size)
            {
                case TINY:
                    speed = 1;
                    break;
                case SMAL:
                    speed = 0.5f;;
                    break;
                case MEDI:
                    speed = 0.25f;
                    break;
                case LARG:
                    speed = 0.5f;
                    break;
                case HUGE:
                    speed = 0.25f;
                    break;
                case XBOX:
                    speed = 0.125f;
                    break;
            }

            if (pos.Y > 800)
            {
                speed = size;
            }

            if (active)
            {
                pos.Y += dirY * speed;

                //while (horTime > 0)
                //{
                //    pos.X += dirX * speed;
                //    horTime--;

                //    if (movePattern != -1 && horTime <= 0)
                //    {
                //        UpdatePattern(1);
                //    }
                //}

                //while (verTime > 0)
                //{
                //    pos.Y += dirY * speed;
                //    verTime--;

                //    if (movePattern != -1 && verTime <= 1)
                //    {
                //        UpdatePattern(0);
                //    }
                //}

                //if (movePattern == -1 && patternTime <= 0)
                //{
                //    patternTime = tempPatternTime;
                //    movePattern = tempPattern;
                //}

                //if (patternTime <= 0)
                //{
                //    ChangePattern();
                //}


                //if (movePattern != -1 && verTime == 0 && horTime == 0)
                //{
                //    UpdatePattern(2);
                //}

            }
        }

        ///// <summary>
        ///// Updates move pattern
        ///// </summary>
        ///// <param name="dir">0 = ver, 1 = hor, 2 = both</param>
        //private void UpdatePattern(byte dir)
        //{
        //    switch (movePattern)
        //    {
        //        case 0://straight down
        //            dirY = 1;
        //            dirX = 0;
        //            horTime = patternTime;
        //            verTime = patternTime;
        //            patternTime--;
        //            break;
        //        case 1://Zig zag
        //            break;
        //        case 2://zig straight zag straight
        //            break;
        //        case 3://down left down right
        //            break;
        //        case 4://down left down left down right down right
        //            break;
        //        case 5://smooth
        //            break;
        //        case 6:
        //            break;
        //        case 7:
        //            break;
        //        case 8:
        //            break;
        //    }
        //}

        private void ChangePattern()
        {
            movePattern = 0;
            patternTime = (int)rng.Next(3, 10) * UPDATE;
        }

        /// <summary>
        /// Moves the enemy. 
        /// </summary>
        public void MoveRandom(Vector2 target, int speed)
        {
           
        }

        /// <summary>
        /// Will attempt to avoid any bullets moving within a certain distance of Enemy, depending on passes.
        /// </summary>
        public void AvoidSide(Vector2 avoidPos)
        {
            //if (movePattern != -1)
            //{
            //    tempPattern = movePattern;
            //    movePattern = -1;
            //}

            if (pos.X < avoidPos.X)
            {
                horTime = 0.1f * UPDATE;
                dirX = -1;
            }

            else if (pos.X < avoidPos.X)
            {
                horTime = 0.1f * UPDATE;
                dirX = 1;
            }

            //else if (pos.X < avoidPos.X)
            //{
            //    if (RollDice(1, 2))
            //    {
            //        dirX = 1;
            //    }
            //    else
            //    {
            //        dirX = -1;
            //    }
            //}
        }

        /// <summary>
        /// Will attempt to avoid any bullets moving within a certain distance of Enemy, depending on passes.
        /// </summary>
        public void AvoidFront()
        {
            //verTime = 0.1f * UPDATE;
            //dirY = -1;
        }

        public void CycleColour()
        {
            if (colour < BLK)
            {
                colour++;
            }
            else
            {
                colour = WHT;
            }
        }

        public Rectangle GetRect()
        {                
            return new Rectangle((int)(pos.X - posadj.X * size), (int)(pos.Y - posadj.Y * size), BLOCKWIDTH * size, BLOCKHEIGHT * size);
        }
        
        public Rectangle GetAvoidFrontRect()
        {
            return new Rectangle((int)(pos.X - posadj.X * size), (int)(pos.Y - posadj.Y * size), BLOCKWIDTH * size, BLOCKHEIGHT * size);
        }

        public Rectangle GetAvoidSideRect()
        {
            return new Rectangle((int)(pos.X - posadj.X * size * 2), (int)(pos.Y - posadj.Y * size), BLOCKWIDTH * size * 2, BLOCKHEIGHT * size * 1);
        }


        public Rectangle GetAvoidLeftRect()
        {
            return new Rectangle((int)(pos.X - posadj.X * size), (int)(pos.Y - posadj.Y * size), (BLOCKWIDTH * size) / 2, BLOCKHEIGHT * size * 1);
        }


        public Rectangle GetAvoidRightRect()
        {
            return new Rectangle((int)(pos.X + posadj.X * size), (int)(pos.Y - posadj.Y * size), (BLOCKWIDTH * size) / 2, BLOCKHEIGHT * size * 1);
        }

        /// <summary>
        /// Changes size of Enemy
        /// </summary>
        /// <param name="mode">0 = down, 1 = up, 2 = assign</param>
        /// <param name="newsize">TINY (1), SMALL (2), MEDIUM (4), LARGE (8), HUGE (16), XBOX (32)</param>
        public void ChangeSize(byte mode, byte newsize)
        {
            switch(mode)
            {
                case 0:
                    switch (size)
                    {
                        case TINY:
                            break;
                        case SMAL:
                            size = TINY;
                            break;
                        case MEDI:
                            size = SMAL;
                            break;
                        case LARG:
                            size = MEDI;
                            break;
                        case HUGE:
                            size = LARG;
                            break;
                        case XBOX:
                            size = HUGE;
                            break;
                    }
                    UpdateHealth();
                    break;
                case 1:
                    switch (size)
                    {
                        case TINY:
                            size = SMAL;
                            break;
                        case SMAL:
                            size = MEDI;
                            break;
                        case MEDI:
                            size = LARG;
                            break;
                        case LARG:
                            size = HUGE;
                            break;
                        case HUGE:
                            size = XBOX;
                            break;
                        case XBOX:
                            break;
                    }
                    UpdateHealth();
                    break;
                case 3:
                        if (newsize == TINY || newsize == SMAL || newsize == MEDI || newsize == LARG || newsize == HUGE || newsize == XBOX)
                        {
                            size = newsize;
                        }
                    UpdateHealth();
                    break;
            }

        }

        /// <summary>
        /// Routines to be completed on spawn.
        /// </summary>
        public void Spawn()
        {

        }

        public void Death()
        {
            active = false;
        }

        public void GlomTest(Enemy other)
        {
            if (RollDice(1, size))
            {
                Glom(other);
            }

            else
            {
                switch (dirX)
                {
                    case -1:
                        dirX = 1;
                        horTime = 3 * UPDATE;
                        break;
                    case 1:
                        dirX = -1;
                        horTime = 3 * UPDATE;
                        break;
                }
            }
        }

        private void Glom(Enemy other)
        {

        }

        //private Enemy Split()
        //{
        //    return;
        //}

        public void Hit(int damage, byte hitcolour)
        {
            bool healed = false;

            if (colour == hitcolour)
            {
                Heal(damage);
                healed = true;
            }

            if (colour == BLK)
            {
                if (hitcolour == WHT)
                {
                    Hurt(damage * 2);
                }
            }

            else if (colour == WHT)
            {

                if (hitcolour == BLK)
                {
                    Hurt(damage * 2);
                }
            }

            else if (hitcolour == BLK)
            {
                Hurt(damage * 4);
            }

            else if (hitcolour == WHT)
            {
                Heal(damage * 4);
            }

            else if (colour == hitcolour + 3 || colour == hitcolour - 3)
            {
                Hurt(damage * 2);
            }

            else if(!healed)
            {
                Hurt(damage);
            }
        }

        public void Hurt(int damage)
        {
            health -= damage;
        }

        private void UpdateHealth()//REMOVE
        {
            healthmax = size * 8;
            health = healthmax;
        }

        public void Heal(int damage)
        {
            if (health + damage < healthmax)
            {
                health += damage;
            }

            else if (health + damage >= healthmax)
            {
                health = healthmax;
            }
        }

        public void CheckPos()
        {
            if (pos.Y - posadj.Y * size > 896)
            {
                active = false;
            }
        }

        public void CheckHealth()
        {
            if (health <= 0)
            {
                if (alive)
                {
                    alive = false;
                    Death();
                }
            }
        }

        private bool RollDice(int chance, int sides)
        {
            int roll = rng.Next(0, sides + 1);
            if (roll <= chance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int RollNumber(int min, int max)
        {
            return (int)rng.Next(min, max + 1);
        }
    }
}
