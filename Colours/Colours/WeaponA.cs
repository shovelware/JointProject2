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
    class WeaponA : Weapon
    {
        private readonly int[] HEATGAIN = new int[3]{ 2, 2, 4 };
        private readonly int[] HEATMAX = new int[3]{ 50, 50, 100 };
        private readonly int[] COOLDOWN = new int[3] { 1, 1, 2 };
        private readonly int[] COOLSPEED = new int[3] { 2, 2, 5 };
        private readonly int[] OVERHEATMAX = new int[3] { 60, 45, 30};
        private readonly int[] REFIREMAX = new int[3] { 20, 15, 15 };

        //DAMAGE OUTPUT: 130, 260, 520
        const byte NUMBER = 1;
        const bool AUTO = true;

        public override byte Number
        {
            get { return NUMBER; }
        }

        public override bool Auto
        {
            get { return AUTO; }
        }

        int refiretime;
        bool refire;
        int shots;//

        int cooling;
        int heat;
        int overheat;
        bool overheated;
        bool recovering;

        bool fired;
        Texture2D texhair;
        UI hud;

        public WeaponA(UI ui, Texture2D weaponTexture, Texture2D hairTexture)
            : base(ui, weaponTexture)
        {
            hud = ui;
            texhair = hairTexture;
        }

        public override bool Fire()
        {
            fired = false;

            if (active && !overheated && !recovering && !refire && level != -1)
            {
                if (heat < HEATMAX[level])
                {
                    heat += HEATGAIN[level];
                    fired = true;
                    refiretime = REFIREMAX[level];
                    refire = true;
                    shots++;//
                }

                if (heat > HEATMAX[level])
                {
                    fired = false;
                    overheated = true;
                    heat = HEATMAX[level];
                    overheat = OVERHEATMAX[level];
                }

                if (heat == 0)
                {
                    shots = 0;//
                }

            }

            return fired;
        }

        public override void Cool()
        {
            if (level != -1)
            {
                if (refire)//refiring
                {
                    refiretime--;

                    if (refiretime == 0)
                    {
                        refire = false;
                    }
                }

                else if (heat > 0 && overheat == 0 && !refire)//cooling
                {
                    cooling--;

                    if (heat >= COOLDOWN[level] && cooling <= 0)
                    {
                        heat -= COOLDOWN[level];
                        cooling = COOLSPEED[level];
                    }

                    if (heat < COOLDOWN[level])
                    {
                        heat -= heat;
                    }

                    if (heat == 0 && recovering == true)
                    {
                        recovering = false;
                        shots = 0;//
                    }
                }

                else if (overheated == true && overheat > 0)//overheating
                {
                    overheat--;

                    if (overheat == 0)
                    {
                        overheated = false;
                        recovering = true;
                    }
                }
            }
        }

        public override void InstaCool()
        {
            heat = 0;
            overheated = false;
            recovering = false;
        }

        public override void DrawUI(Vector2 mousePos, Vector2 playerPos, byte colour)
        {
            if (level != -1)
            {
                //hud.DrawBar(new Vector2(playerPos.X, playerPos.Y + 32), heat, HEATMAX[level], overheated, recovering, colour);
                hud.DrawBar(new Vector2(320, 840), heat, HEATMAX[level], overheated, recovering, colour);
                hud.DrawWeaponHairs(NUMBER, level, colour, texhair, mousePos, playerPos);

                //hud.DrawBool(16, 850, fired);
                //hud.DrawInt(128, 800, shots);
            }
        }
    }
}
