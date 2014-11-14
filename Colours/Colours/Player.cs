using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Colours
{
    class Player
    {
        //Scoring
        int score;

        public int Score { get { return score; } set { score = value; } }

        string name;

        public string Name { get { return name; } set { name = value; } }

        UI hud;
        Texture2D playerTex;

        const int BLOCKWIDTH = 128;
        const int BLOCKHEIGHT = 64;

        //position and scale
        Vector2 pos = new Vector2(320, 800);
        Vector2 posadj = new Vector2(BLOCKWIDTH / 2, BLOCKHEIGHT / 2);
        Vector2 poswep = new Vector2(0, BLOCKHEIGHT);
        Vector2 origin = new Vector2(0, 0);

        public Vector2 Pos { get { return pos; } }

        //movement
        int speed;
        
        //combat
        bool alive;

        public bool Alive { get { return alive; } }

        bool active;
        
        public bool Active { get { return active; } }


        const int HEALTHMAX = 128;
        int health;

        int xp;
        int[] xpneeded = new int[] { 256, 512, 512 };

        const byte WEAPONSMAX = 4;

        const byte WHT = 0, RED = 1, GRN = 2, BLU = 3, CYN = 4, MGN = 5, YLO = 6, BLK = 7;
        byte colour;

        public byte Colour { get { return colour; } }

        Weapon[] weapons;
        Weapon currentWeapon;

        public Weapon CurrentWeapon { get { return currentWeapon; } }


        /// <summary>
        /// Constructs a player with default values.
        /// </summary>
        public Player(Texture2D playerTexture, Weapon[] weps, UI hud1)
        {
            alive = true;
            active = true;
            name = "Oshino";
            playerTex = playerTexture;
            colour = 0;
            weapons = weps;
            hud = hud1;

            health = HEALTHMAX;
            speed = 4;
        }

        public Player(int scores, string names)
        {
            name = names;
            score = scores;
        }

        public bool Fire()
        {
            return (currentWeapon.Fire());
        }


        public bool Refire()
        {
            if (currentWeapon.Auto == true)
            {
                return (currentWeapon.Fire());
            }
            else return false;
        }

        /// <summary>
        /// Changes current Weapon's level
        /// </summary>
        /// <param name="mode">0 = Decrement, 1 = Increment, 2 = Cycle, 3 = Assign</param>
        /// <param name="newlevel">-1 = Locked, 0/1/2 = Levels</param>
        public void ChangeWepLevel (byte mode, sbyte newlevel)
        {
            currentWeapon.ChangeLevel(mode, newlevel);
        }

        public void Move(int target, int mult)
        {
            if (target < (int)pos.X)
            {
                if ((int)pos.X - target < speed)
                {
                    pos.X = target;
                }

                else
                {
                    pos.X = (int)pos.X - (speed * mult);
                }
            }

            else if (target > (int)pos.X)
            {
                if (target - (int)pos.X  < speed)
                {
                    pos.X = target;
                }

                else
                {
                    pos.X = (int)pos.X + (speed * mult);
                }
            }
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

        public void ChangeColour(byte newcol)
        {
            if (colour != newcol && newcol <= BLK)
            colour = newcol;
        }
        
        /// <summary>
        /// Changes current Weapon for the new weapon.
        /// </summary>
        /// <param name="mode">0 = Decrement, 1 = Increment, 2 = Cycle, 3 = Assign</param>
        /// <param name="newWep">0 = None, 1 = Atatat, 2 = Boom, 3 = Coil, 4 = Depleted</param>
        public void ChangeWeapon(byte mode, byte newNum)
        {
            if (mode == 0)
            {
                byte prev;
                //find next unlocked
                for (prev = 0; prev <= WEAPONSMAX; prev++)
                {
                    if (weapons[prev].Level != -1)
                    {
                        break;
                    }
                }


                //deactivate others
                for (byte i = 0; i <= WEAPONSMAX; i++)
                {
                    weapons[i].Active = false;
                }

                //assign next unlocked active
                currentWeapon = weapons[prev];
                weapons[prev].Active = true;
            }

            if (mode == 1)
            {
                byte next;
                //find next unlocked
                for (next = currentWeapon.Number; next <= WEAPONSMAX; next++)
                {
                    if (weapons[next].Level != -1 && next != currentWeapon.Number)
                    {
                        break;
                    }
                }

                //deactivate others
                for (byte i = 0; i <= WEAPONSMAX; i++)
                {
                    weapons[i].Active = false;
                }

                //assign next unlocked active
                currentWeapon = weapons[next];
                weapons[next].Active = true;
            }

            if (mode == 2)
            {
                //get active
                //find next unlocked
                //deactivate others
                //assign next unlocked active
                //if next unlocked >max start again
            }

            if (mode == 3)
            {
                if (newNum >= 0 && newNum <= WEAPONSMAX && weapons[newNum].Level != -1)
                {
                    for (byte i = 0; i <= WEAPONSMAX; i++)
                    {
                        weapons[i].Active = false;
                    }
                    currentWeapon = weapons[newNum];
                    currentWeapon.Active = true;
                }
            }
        }

        public Rectangle GetRect()
        {
            return new Rectangle((int)(pos.X - posadj.X), (int)(pos.Y - posadj.Y), BLOCKWIDTH, BLOCKHEIGHT);
        }

        public void GainXP(int gain)
        {
            xp += gain;

            if (currentWeapon.Level == 0 || currentWeapon.Level == 1)
            {
                if (xp > xpneeded[currentWeapon.Level] && currentWeapon.Level !=2)
                {
                    currentWeapon.ChangeLevel(1, 1);
                }
            }
        }

        public void Hit(int damage, byte hitcolour)
        {
            if (colour == BLK)
            {
            }

            else if (colour == WHT)
            {
            }

            else if (colour == hitcolour + 3 || colour == hitcolour - 3)
            {
                Hurt(damage * 2);
            }

            else
            {
                Hurt(damage);
            }
        }

        public void Hurt(int damage)
        {
            health -= damage;
        }

        public bool CheckHealth()
        {
            if (health <= 0)
            {
                Death();
                return true;
            }

            else
            {
                return false;
            }
        }

        public void Death()
        {
            alive = false;
            active = false;
        }

        public void ChangeScore(int change, bool sub)
        {
            if (sub)
            {
                int i = change;
                while (i > 0)
                {
                    i--;
                    score -= 1;
                }
            }

            else
            {
                int i = 0;
                while (i < change)
                {
                    i++;
                    score += 1;
                }
            }
        }

        public void Reset()
        {
            health = HEALTHMAX;
            xp = 0;
            weapons[1].ChangeLevel(3, 0);
            score = 0;
            colour = 1;
            active = true;
            alive = true;
        }

        public void Draw(SpriteBatch sprbat, Vector2 mousePos)
        {
            if (active)
            {
                if (alive)
                {
                    hud.DrawBar(new Vector2(320, 860), health, HEALTHMAX, colour);
                    if (currentWeapon.Level != -1)
                    {
                        hud.DrawWeaponXP(xp, xpneeded[currentWeapon.Level]);
                    }
                    if (currentWeapon.Active != false)
                    {
                        currentWeapon.DrawUI(mousePos, pos, colour);
                        currentWeapon.Draw(sprbat, pos, colour, mousePos);
                    }
                    sprbat.Draw(playerTex, (pos - posadj), new Rectangle(colour * BLOCKWIDTH, 0, BLOCKWIDTH, BLOCKHEIGHT), Color.White, 0, origin, 1, 0, 0.001f);
                    hud.DrawString(new Vector2(400, 858), name);
                    hud.DrawInt(new Vector2(200, 858), score);
                }
            }
        }

    }
}