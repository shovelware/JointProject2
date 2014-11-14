using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Colours
{
    class UI
    {
        SpriteBatch sprbat;
        SpriteFont font;
        Texture2D centre, hor, ver;
        Texture2D[] colours = new Texture2D[9];
        Vector2 origin = new Vector2(0, 0);


        public UI(SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D[] content)
        {
            sprbat = spriteBatch;
            font = spriteFont;
            centre = content[0];
            hor = content[1];
            ver = content[2];

            colours [0] = content[3];
            colours [1] = content[4];
            colours [2] = content[5];
            colours [3] = content[6];
            colours [4] = content[7];
            colours [5] = content[8];
            colours [6] = content[9];
            colours [7] = content[10];
            colours[8] = content[11];
        }

        public void DrawHairs(Vector2 mousePos, Vector2 playerPos)
        {
            int mouseX = (int)mousePos.X;
            int mouseY = (int)mousePos.Y;
            int playerX = (int)playerPos.X;
            int playerY = (int)playerPos.Y;

            sprbat.Draw(ver, new Rectangle(mouseX - 1, 0, 2, playerY), new Rectangle(0, 0, 2, 900), Color.White, 0, origin, 0, 0.01f);

            if (mouseY < 800)
            {
                sprbat.Draw(hor, new Rectangle(0, mouseY - 1, 650, 2), new Rectangle(0, 0, 650, 2), Color.White, 0, origin, 0, 0.01f);
            }
            sprbat.Draw(hor, new Rectangle(0, playerY - 1, 650, 2), new Rectangle(0, 0, 650, 2), Color.White, 0, origin, 0, 0.01f);
            sprbat.Draw(centre, new Rectangle(mouseX - 4, mouseY - 4, 8, 8), new Rectangle(0, 0, 8, 8), Color.White, 0, origin, 0, 0);
        }

        public void DrawHairCentre(Vector2 mousePos, Vector2 playerPos)
        {
            int mouseX = (int)mousePos.X;
            int mouseY = (int)mousePos.Y;
            int playerX = (int)playerPos.X;
            int playerY = (int)playerPos.Y;

            sprbat.Draw(ver, new Rectangle(mouseX - 1, 0, 2, playerY), new Rectangle(0, 0, 2, 900), Color.White, 0, origin, 0, 0.01f);
            sprbat.Draw(hor, new Rectangle(0, mouseY - 1, 650, 2), new Rectangle(0, 0, 650, 2), Color.White, 0, origin, 0, 0.01f);
            sprbat.Draw(centre, new Rectangle(mouseX - 4, mouseY - 4, 8, 8), new Rectangle(0, 0, 8, 8), Color.White, 0, origin, 0, 0);
        }

        public void DrawWeaponHairs(byte weapon, sbyte level, byte colour, Texture2D texhair, Vector2 mousePos, Vector2 playerPos)
        {

            int mouseX = (int)mousePos.X;
            int mouseY = (int)mousePos.Y;
            int playerX = (int)playerPos.X;
            int playerY = (int)playerPos.Y;

            const int BLOCKWIDTH = 16;
            //const int BLOCKHEIGHT = 896;

            switch (weapon)
            {
                case 1:
                    {
                        switch (level)
                        {
                            case 0:
                                sprbat.Draw(texhair, new Rectangle((int)playerX - 8, 0, 16, (int)playerY), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, (int)playerY), Color.White, 0, origin, 0, 0.01f);
                                break;
                            case 1:
                                sprbat.Draw(texhair, new Rectangle((int)playerX - 50, 0, 16, (int)playerY), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, (int)playerY), Color.White, 0, origin, 0, 0.01f);
                                sprbat.Draw(texhair, new Rectangle((int)playerX + 34, 0, 16, (int)playerY), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, (int)playerY), Color.White, 0, origin, 0, 0.01f);
                                break;
                            case 2:
                                sprbat.Draw(texhair, new Rectangle((int)playerX - 58, 0, 32, (int)playerY), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, (int)playerY), Color.White, 0, origin, 0, 0.01f);
                                sprbat.Draw(texhair, new Rectangle((int)playerX + 26, 0, 32, (int)playerY), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, (int)playerY), Color.White, 0, origin, 0, 0.01f);
                                break;
                        }
                        break;
                    }
            }
        }

        public void DrawWeaponHairs(byte weapon, sbyte level, byte colour, Texture2D texhair, Texture2D teximpact, Vector2 mousePos, Vector2 playerPos)
        {
            int mouseX = (int)mousePos.X;
            int mouseY = (int)mousePos.Y;
            int playerX = (int)playerPos.X;
            int playerY = (int)playerPos.Y;

            const int BLOCKWIDTH = 16;
            //const int BLOCKHEIGHT = 896;
            const int PLAYER = 128;

            int[] IBLOCKWIDTH = new int[3]  {32, 64, 128};
            int[] IBLOCKHEIGHT = new int[3] {32, 64, 128};
            int[] MINRANGE = new int[3] { 64, 128, 256 };

            int minrange = 864 - PLAYER - MINRANGE[level];

            switch (weapon)
            {
                case 2:
                    {
                        switch (level)
                        {
                            case 0:
                                if (mouseY < minrange)
                                {
                                    sprbat.Draw(texhair, new Rectangle((int)playerX - BLOCKWIDTH / 2, (int)mouseY, BLOCKWIDTH, (int)(playerY - mouseY)), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, (int)(playerY - mouseY)), Color.White, 0, origin, 0, 0.01f);
                                    sprbat.Draw(teximpact, new Rectangle((int)playerX - IBLOCKWIDTH[level] / 2, (int)mouseY - IBLOCKHEIGHT[level] / 2, IBLOCKWIDTH[level], IBLOCKHEIGHT[level]), new Rectangle(colour * IBLOCKWIDTH[level], 0, IBLOCKWIDTH[level], IBLOCKHEIGHT[level]), Color.White, 0, origin, 0, 0.01f);
                                }
                                else
                                {
                                    sprbat.Draw(texhair, new Rectangle((int)playerX - BLOCKWIDTH / 2, minrange, BLOCKWIDTH, (int)(playerY - minrange)), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, (int)(playerY - minrange)), Color.White, 0, origin, 0, 0.01f);
                                    sprbat.Draw(teximpact, new Rectangle((int)playerX - IBLOCKWIDTH[level] / 2, minrange - IBLOCKHEIGHT[level] / 2, IBLOCKWIDTH[level], IBLOCKHEIGHT[level]), new Rectangle(colour * IBLOCKWIDTH[level], 0, IBLOCKWIDTH[level], IBLOCKHEIGHT[level]), Color.White, 0, origin, 0, 0.01f);
                                }
                                break;
                            case 1:
                                if (mouseY < minrange)
                                {
                                    sprbat.Draw(texhair, new Rectangle((int)playerX - BLOCKWIDTH, (int)mouseY, BLOCKWIDTH * 2, (int)(playerY - mouseY)), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, (int)(playerY - mouseY)), Color.White, 0, origin, 0, 0.01f);
                                    sprbat.Draw(teximpact, new Rectangle((int)playerX - IBLOCKWIDTH[level] / 2, (int)mouseY - IBLOCKHEIGHT[level] / 2, IBLOCKWIDTH[level], IBLOCKHEIGHT[level]), new Rectangle(colour * IBLOCKWIDTH[level], 0, IBLOCKWIDTH[level], IBLOCKHEIGHT[level]), Color.White, 0, origin, 0, 0.01f);
                                }
                                else
                                {
                                    sprbat.Draw(texhair, new Rectangle((int)playerX - BLOCKWIDTH, (int)minrange, BLOCKWIDTH * 2, (int)(playerY - minrange)), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, (int)(playerY - minrange)), Color.White, 0, origin, 0, 0.01f);
                                    sprbat.Draw(teximpact, new Rectangle((int)playerX - IBLOCKWIDTH[level] / 2, minrange - IBLOCKHEIGHT[level] / 2, IBLOCKWIDTH[level], IBLOCKHEIGHT[level]), new Rectangle(colour * IBLOCKWIDTH[level], 0, IBLOCKWIDTH[level], IBLOCKHEIGHT[level]), Color.White, 0, origin, 0, 0.01f);
                                }
                                break;
                            case 2:
                                if (mouseY < minrange)
                                {
                                    sprbat.Draw(texhair, new Rectangle((int)playerX - BLOCKWIDTH * 2, (int)mouseY, BLOCKWIDTH * 4, (int)(playerY - mouseY)), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, (int)(playerY - mouseY)), Color.White, 0, origin, 0, 0.01f);
                                    sprbat.Draw(teximpact, new Rectangle((int)playerX - IBLOCKWIDTH[level] / 2, (int)mouseY - IBLOCKHEIGHT[level] / 2, IBLOCKWIDTH[level], IBLOCKHEIGHT[level]), new Rectangle(colour * IBLOCKWIDTH[level], 0, IBLOCKWIDTH[level], IBLOCKHEIGHT[level]), Color.White, 0, origin, 0, 0.01f);
                                }
                                else
                                {
                                    sprbat.Draw(texhair, new Rectangle((int)playerX - BLOCKWIDTH * 2, (int)minrange, BLOCKWIDTH * 4, (int)(playerY - minrange)), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, (int)(playerY - minrange)), Color.White, 0, origin, 0, 0.01f);
                                    sprbat.Draw(teximpact, new Rectangle((int)playerX - IBLOCKWIDTH[level] / 2, minrange - IBLOCKHEIGHT[level] / 2, IBLOCKWIDTH[level], IBLOCKHEIGHT[level]), new Rectangle(colour * IBLOCKWIDTH[level], 0, IBLOCKWIDTH[level], IBLOCKHEIGHT[level]), Color.White, 0, origin, 0, 0.01f);
                                }
                                break;
                        }
                        break;
                    }



            }
        }

        public void DrawWeaponStatus(byte pos, int value, int maxvalue, bool status1, bool status2, Color color1, Color color2)
        {
            sprbat.DrawString(font, "" + maxvalue, new Vector2(16, 800), Color.Black);

            if (status1)
            {
                sprbat.DrawString(font, "" + value, new Vector2(16, 825), color1);
            }

            else if (status2)
            {
                sprbat.DrawString(font, "" + value, new Vector2(16, 825), color2);
            }

            else
            {
                sprbat.DrawString(font, "" + value, new Vector2(16, 825), Color.Black);
            }
        }

        public void DrawBar(Vector2 pos, int value, int maxvalue, byte colour)
        {
            int posx = (int)(pos.X - maxvalue / 2 - 2);
            int posy = (int)pos.Y;

            sprbat.Draw(colours[8], new Rectangle(posx, posy, maxvalue + 4, 2), Color.White);
            sprbat.Draw(colours[8], new Rectangle(posx, posy + 10, maxvalue + 4, 2), Color.White);

            sprbat.Draw(colours[8], new Rectangle(posx, posy + 1, 2, 10), Color.White);
            sprbat.Draw(colours[8], new Rectangle(posx + 2 + maxvalue, posy + 1, 2, 10), Color.White);

            sprbat.Draw(colours[colour], new Rectangle(posx + 2, posy + 2, value, 8), Color.White);
        }

        public void DrawBar(Vector2 pos, int value, int maxvalue, bool status1, bool status2, byte colour)
        {
            int posx = (int)(pos.X - maxvalue / 2 - 2);
            int posy = (int)pos.Y;

            sprbat.Draw(colours[8], new Rectangle(posx, posy, maxvalue + 4, 2), Color.White);
            sprbat.Draw(colours[8], new Rectangle(posx, posy + 10, maxvalue + 4, 2), Color.White);

            sprbat.Draw(colours[8], new Rectangle(posx, posy + 1, 2, 10), Color.White);
            sprbat.Draw(colours[8], new Rectangle(posx + 2 + maxvalue, posy + 1, 2, 10), Color.White);

            if (status1)
            {
                sprbat.Draw(colours[7], new Rectangle(posx + 2, posy + 2, value, 8), Color.White);
            }

            else if (status2)
            {
                sprbat.Draw(colours[0], new Rectangle(posx + 2, posy + 2, value, 8), Color.White);
            }

            else
            {
                sprbat.Draw(colours[colour], new Rectangle(posx + 2, posy + 2, value, 8), Color.White);
            }
        }

        public void DrawWeaponXP(int value, int maxvalue)
        {
            int posx = 320 - maxvalue / 2 - 2;
            int posy = 880;

            sprbat.Draw(colours[8], new Rectangle(posx, posy, maxvalue + 4, 2), Color.White);
            sprbat.Draw(colours[8], new Rectangle(posx, posy + 10, maxvalue + 4, 2), Color.White);

            sprbat.Draw(colours[8], new Rectangle(posx, posy + 1, 2, 10), Color.White);
            sprbat.Draw(colours[8], new Rectangle(posx + 2 + maxvalue, posy + 1, 2, 10), Color.White);

            if (value >= maxvalue)
            {
                sprbat.Draw(colours[0], new Rectangle(posx + 2, posy + 2, maxvalue, 8), Color.White);
            }

            else
            {
                sprbat.Draw(colours[7], new Rectangle(posx + 2, posy + 2, value, 8), Color.White);
            }
        }

        public void DrawMsg(Vector2 pos, string text, Color color)
        {
            sprbat.Draw(colours[7], new Rectangle((int)pos.X - 64, (int)pos.Y - 32, 128, 64), Color.White);
            sprbat.Draw(colours[8], new Rectangle((int)pos.X - 62, (int)pos.Y - 30, 124, 60), Color.White);
            sprbat.DrawString(font, text, new Vector2(pos.X - 40, pos.Y - 20), color);

        }

        public void DrawEnemyHealth(Enemy e)
        {
            int posx = (int)e.Pos.X - e.HealthMax / 2 - 2;
            int posy = (int)(e.Pos.Y - e.Posadj.Y * e.Size - 16);

            sprbat.Draw(colours[8], new Rectangle(posx, posy, e.HealthMax + 4, 2), Color.White);
            sprbat.Draw(colours[8], new Rectangle(posx, posy + 10, e.HealthMax + 4, 2), Color.White);

            sprbat.Draw(colours[8], new Rectangle(posx, posy + 1, 2, 10), Color.White);
            sprbat.Draw(colours[8], new Rectangle(posx + 2 + e.HealthMax, posy + 1, 2, 10), Color.White);

            sprbat.Draw(colours[e.Colour], new Rectangle(posx + 2, posy + 2, e.Health, 8), Color.White);
        }

        public void DrawBool(int xPos, int yPos, bool value)
        {
            sprbat.DrawString(font, "" + value, new Vector2(xPos, yPos), Color.Black);
        }

        public void DrawByte(int xPos, int yPos, byte value)
        {
            sprbat.DrawString(font, "" + value, new Vector2(xPos, yPos), Color.Black);
        }

        public void DrawInt(int xPos, int yPos, int value)
        {
            sprbat.DrawString(font, "" + value, new Vector2(xPos, yPos), Color.Black);
        }

        public void DrawInt(Vector2 pos, int value)
        {
            sprbat.DrawString(font, "" + value, pos, Color.Black);
        }

        public void DrawString(int xPos, int yPos, string value)
        {
            sprbat.DrawString(font, "" + value, new Vector2(xPos, yPos), Color.Black);
        }

        public void DrawString(Vector2 pos, string value)
        {
            sprbat.DrawString(font, "" + value, pos, Color.Black);
        }

        public void DrawString(int xPos, int yPos, string value, Color color)
        {
            sprbat.DrawString(font, "" + value, new Vector2(xPos, yPos), color);
        }
        
        public void DrawVector2(int xPos, int yPos, Vector2 value)
        {
            sprbat.DrawString(font, "" + value, new Vector2(xPos, yPos), Color.Black);
        }

        public void DrawRectangle(int xPos, int yPos, Rectangle value)
        {
            sprbat.DrawString(font, "" + value, new Vector2(xPos, yPos), Color.Black);
        }
    }
}

