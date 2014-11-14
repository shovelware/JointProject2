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
    class WeaponC : Weapon
    {
        private readonly int[] DAMAGE = new int[3] { 2, 4, 6 };
        private readonly int[] RADIUS = new int[3] { 4, 8, 16 };

        private readonly int[] COOLGAIN = new int[3] { 2, 3, 4 };
        private readonly int[] COOLTIMEMAX = new int[3] { 30, 25, 20 };
        private readonly int[] POWERMAX = new int[3] { 120, 180, 240 };

        //DAMAGE OUTPUT: 120, 240, 360

        const byte NUMBER = 3;
        const bool AUTO = true;

        public override byte Number
        {
            get { return NUMBER; }
        }

        public override bool Auto
        {
            get { return AUTO; }
        }

        int power;

        int shots;//

        int cooltime;
        bool charging;
        bool empty;

        bool fired;

        UI hud;

        public WeaponC(UI ui, Texture2D weaponTexture, Texture2D hairTexture)
            : base(ui, weaponTexture, hairTexture)
        {
            hud = ui;
        }

        public override bool Fire()
        {

            if (active && !empty && level != -1)
            {

                fired = false;

                if (power > 0)
                {
                    power -= COOLGAIN[level];
                    fired = true;
                    cooltime = COOLTIMEMAX[level];
                    shots++;
                }

                if (power <= 0)
                {
                    fired = false;
                    empty = true;
                }

            }

            return fired;
        }

        public override void Cool()
        {
            
            if (level != -1)
            {
                if (power < POWERMAX[level])//recharging
                {
                    if (cooltime > 0)
                    {
                        cooltime--;
                    }

                    if (cooltime <= 0)
                    {
                        charging = true;
                        power++;
                    }
                }

                else if (power >= POWERMAX[level])//charged
                {
                    empty = false;
                    charging = false;
                    power = POWERMAX[level];//reset overfill on delevel
                    shots = 0;//
                }
            }
        }

        public override void InstaCool()
        {
            empty = false;
            charging = false;
            power = POWERMAX[level];
        }

        public override void DrawUI(Vector2 mousePos, Vector2 playerPos, byte colour)
        {
            if (level != -1)
            {
                hud.DrawWeaponStatus(1, power, POWERMAX[level], empty, charging, Color.Red, Color.Orange);


                hud.DrawBool(16, 850, fired);

                hud.DrawInt(128, 800, shots);
                hud.DrawInt(128, 825, (shots * DAMAGE[level]));
            }
        }
    }
}
