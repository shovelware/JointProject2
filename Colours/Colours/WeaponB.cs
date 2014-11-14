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
    class WeaponB : Weapon
    {
        private readonly int[] DAMAGE = new int[3]{ 64, 128, 256 };
        private readonly int[] RADIUS = new int[3] { 32, 64, 128 };
        private readonly int[] FALLOFF = new int[3] { 32, 32, 32 };
        private readonly int[] MINRANGE = new int[3] { 64, 128, 256 };


        private readonly int[] MAGAZINEMAX = new int[3]{ 1, 1, 1 };
        private readonly int[] RELOADMAX = new int[3] { 120, 60, 30 };
        private readonly int[] REFIREMAX = new int[3] { 60, 60, 60 };

        //DAMAGE OUTPUT: 50, 200, 600

        const byte NUMBER = 2;
        const bool AUTO = false;

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

        int magazine;
        int reloadtime;
        bool reloading;
        bool empty;

        bool fired;

        Texture2D texhair;
        Texture2D[] texhairi;

        UI hud;

        public WeaponB(UI ui, Texture2D weaponTexture, Texture2D hairTexture, Texture2D[] hairiTexture)
                    : base(ui, weaponTexture, hairTexture)
                {
                    hud = ui;
                    texhair = hairTexture;
                    texhairi = hairiTexture;
                }

        public override bool Fire()
        {

            if (active && !empty && !refire && level != -1)
            {

                fired = false;

                if (magazine > 0)
                {
                    magazine--;
                    fired = true;
                    refiretime = REFIREMAX[level];
                    refire = true;
                    reloadtime = RELOADMAX[level];
                    shots++;
                }

                if (magazine <= 0)
                {
                    fired = false;
                    empty = true;
                }

            }

            return fired;
        }

        public override void Cool()
        {            
            if (level != -1)//refire
            {
                if (refire)
                {
                    refiretime--;

                    if (refiretime == 0)
                    {
                        refire = false;
                    }
                }

                else if (magazine < MAGAZINEMAX[level] && !refire)//reloading
                {
                    reloading = true;
                    reloadtime--;

                    if (reloadtime <= 0)
                    {
                        magazine++;
                        reloadtime = RELOADMAX[level];
                    }

                }

                else if (magazine >= MAGAZINEMAX[level])//reloaded
                {
                    empty = false;
                    reloading = false;
                    magazine = MAGAZINEMAX[level];//reset overfill on delevel
                    shots = 0;//
                }
            }
        }


        public override void InstaCool()
        {
            empty = false;
            reloading = false;
            magazine = MAGAZINEMAX[level];
        }


        public override void DrawUI(Vector2 mousePos, Vector2 playerPos, byte colour)
        {
            if (level != -1)
            {
                hud.DrawWeaponStatus(1, magazine, MAGAZINEMAX[level], empty, reloading, Color.Red, Color.Orange);
                hud.DrawWeaponHairs(NUMBER, level, colour, texhair, texhairi[level], mousePos, playerPos);


                hud.DrawBool(16, 850, fired);

                hud.DrawInt(128, 800, shots);
                hud.DrawInt(128, 825, (shots * DAMAGE[level]));


            }
        }
    }
}
