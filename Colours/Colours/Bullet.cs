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
    class Bullet
    {
        Texture2D texProj, texHit;
        Vector2 pos, posadj;
        Vector2 origin = new Vector2(0, 0);

        public Vector2 Pos{ get { return pos; } }

        private readonly int[] DAMAGE = new int[3] { 4, 4, 8 };
        private readonly int[] SPEED = new int[3] { 4, 4, 8 };
        public int Damage{ get { return DAMAGE[level]; } }

        byte colour;

        public byte Colour{ get { return colour; } }

        sbyte level;
        bool active;

        public bool Active{ get { return active; } }

        bool hit;

        public bool Hit{ get { return hit; } }

        const int HITTIMEMAX = 15;
        int hitTime;

        const int BLOCKWIDTH = 4;
        const int BLOCKHEIGHT = 8;

        const int IBLOCKWIDTH = 16;
        const int IBLOCKHEIGHT = 16;

        Vector2 posadji = new Vector2(IBLOCKWIDTH / 2, IBLOCKHEIGHT / 2);


        /// <summary>
        /// Constructs a bullet with default values
        /// </summary>
        public Bullet()
        {
        }

        /// <summary>
        /// Constructs a Bullet with new values. Exceptions retain defaults.
        /// </summary>
        public Bullet(sbyte weplevel, byte wepcolour, Vector2 mousePos, Vector2 playerPos, Texture2D bullettex, Texture2D hittex, byte dual)
        {
            level = weplevel;
            colour = wepcolour;
            texProj = bullettex;
            texHit = hittex;
            active = true;

            int mouseX = (int)mousePos.X;
            int mouseY = (int)mousePos.Y;
            int playerX = (int)playerPos.X;
            int playerY = (int)playerPos.Y;


            if (weplevel != -1)
            {
                switch (weplevel)
                {
                    case 0:
                        posadj = new Vector2(BLOCKWIDTH / 2, BLOCKHEIGHT / 2);

                        pos.X = playerX;
                        pos.Y = playerY - 64;
                        break;
                    case 1:
                        posadj = new Vector2(BLOCKWIDTH / 2, BLOCKHEIGHT / 2);

                        if (dual == 0)
                        {
                            //left bullet
                            pos.X = playerX - 42;
                            pos.Y = playerY - 64;
                        }
                        if (dual == 1)
                        {
                            //right bullet

                            pos.X = playerX + 42;
                            pos.Y = playerY - 64;
                        }

                        break;
                    case 2:
                        posadj = new Vector2(BLOCKWIDTH, BLOCKHEIGHT);

                        if (dual == 0)
                        {
                            //left bullet
                            pos.X = playerX - 42;
                            pos.Y = playerY - 64;
                        }
                        if (dual == 1)
                        {
                            //right bullet
                            pos.X = playerX + 42;
                            pos.Y = playerY - 64;
                        }
                        break;
                }
            }
        }

        public void Move()
        {
            if (active && !hit)
            {
                pos.Y -= SPEED[level];
            }

            if (pos.Y < 0)
            {
                active = false;
            }
        }

        public void Draw(SpriteBatch sprbat)
        {
            if (active == true && level != -1)
            {
                if (level != 2)
                {
                    if (!hit)
                    {
                        sprbat.Draw(texProj, new Rectangle((int)(pos.X - posadj.X), (int)(pos.Y - posadj.Y), BLOCKWIDTH, BLOCKHEIGHT), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, BLOCKHEIGHT), Color.White, 0, origin, 0, 0.01f);
                    }

                    else
                    {
                        hitTime--;

                        if (hitTime > (HITTIMEMAX / 3 * 2))
                        {
                            sprbat.Draw(texHit, new Rectangle((int)(pos.X - posadji.X), (int)(pos.Y - posadji.Y), IBLOCKWIDTH, IBLOCKHEIGHT), new Rectangle(colour * IBLOCKWIDTH, 0, IBLOCKWIDTH, IBLOCKHEIGHT), Color.White, 0, origin, 0, 0.01f);
                        }

                        else if (hitTime > (HITTIMEMAX / 3))
                        {
                            sprbat.Draw(texHit, new Rectangle((int)(pos.X - posadji.X), (int)(pos.Y - posadji.Y), IBLOCKWIDTH, IBLOCKHEIGHT), new Rectangle(colour * IBLOCKWIDTH, IBLOCKHEIGHT, IBLOCKWIDTH, IBLOCKHEIGHT), Color.White, 0, origin, 0, 0.01f);
                        }

                        else if (hitTime > 0)
                        {
                            sprbat.Draw(texHit, new Rectangle((int)(pos.X - posadji.X), (int)(pos.Y - posadji.Y), IBLOCKWIDTH, IBLOCKHEIGHT), new Rectangle(colour * IBLOCKWIDTH, IBLOCKHEIGHT * 2, IBLOCKWIDTH, IBLOCKHEIGHT), Color.White, 0, origin, 0, 0.01f);
                        }

                        else if (hitTime <= 0)
                        {
                            active = false;
                        }
                        
                    }
                }
                else if (level == 2)
                {
                    if (!hit)
                    {
                        sprbat.Draw(texProj, new Rectangle((int)(pos.X - posadj.X), (int)(pos.Y - posadj.Y), BLOCKWIDTH * 2, BLOCKHEIGHT * 2), new Rectangle(colour * BLOCKWIDTH * 2, 0, BLOCKWIDTH * 2, BLOCKHEIGHT * 2), Color.White, 0, origin, 0, 0.01f);
                    }

                    else
                    {
                        hitTime--;

                        if (hitTime > (HITTIMEMAX / 3 * 2))
                        {
                            sprbat.Draw(texHit, new Rectangle((int)(pos.X - posadji.X * 2), (int)(pos.Y - posadji.Y * 2), IBLOCKWIDTH * 2, IBLOCKHEIGHT * 2), new Rectangle(colour * IBLOCKWIDTH, 0, IBLOCKWIDTH, IBLOCKHEIGHT), Color.White, 0, origin, 0, 0.01f);
                        }

                        else if (hitTime > (HITTIMEMAX / 3))
                        {
                            sprbat.Draw(texHit, new Rectangle((int)(pos.X - posadji.X * 2), (int)(pos.Y - posadji.Y * 2), IBLOCKWIDTH * 2, IBLOCKHEIGHT * 2), new Rectangle(colour * IBLOCKWIDTH, IBLOCKHEIGHT, IBLOCKWIDTH, IBLOCKHEIGHT), Color.White, 0, origin, 0, 0.01f);
                        }

                        else if (hitTime > 0)
                        {
                            sprbat.Draw(texHit, new Rectangle((int)(pos.X - posadji.X * 2), (int)(pos.Y - posadji.Y * 2), IBLOCKWIDTH * 2, IBLOCKHEIGHT * 2), new Rectangle(colour * IBLOCKWIDTH, IBLOCKHEIGHT * 2, IBLOCKWIDTH, IBLOCKHEIGHT), Color.White, 0, origin, 0, 0.01f);
                        }

                        else if (hitTime <= 0)
                        {
                            active = false;
                        }

                    }
                }
            }
        }

        public Rectangle GetRect()
        {
            if (level == 2)
            {

                return new Rectangle((int)(pos.X - posadj.X), (int)(pos.Y - posadj.Y), BLOCKWIDTH * 2, BLOCKHEIGHT * 2);
            }

            else
            {
                return new Rectangle((int)(pos.X - posadj.X), (int)(pos.Y - posadj.Y), BLOCKWIDTH, BLOCKHEIGHT);
            }
        }

        public void HitObject()
        {
            if (!hit)
            {
                hit = true;
                hitTime = HITTIMEMAX;
            }
        }

    }
}
