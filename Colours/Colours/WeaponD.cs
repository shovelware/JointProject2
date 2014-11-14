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
    class WeaponD : Weapon
    {
        private readonly int[] DAMAGEMIN = new int[3] { 50, 100, 100 };
        private readonly int[] DAMAGEMAX = new int[3] { 100, 200, 250 };
        private readonly int[] PIERCE = new int[3] { 1, 2, 3 };

        private readonly int[] COOLMAX = new int[3] { 30, 25, 20 };
        private readonly int[] CHARGEMAX = new int[3] { 200, 600, 1000 };
        private readonly int[] CHARGEGAIN = new int[3] { 1, 2, 5 };

        //DAMAGE OUTPUT: 100, 600, 1000

        const byte NUMBER = 4;
        const bool AUTO = false;

        public override byte Number
        {
            get { return NUMBER; }
        }

        public override bool Auto
        {
            get { return AUTO; }
        }

        int shots;//

        int cooltime;
        bool charging;
        int charge;
        bool discharged;
        int damage;


        bool fired;

        UI hud;

        public WeaponD(UI ui, Texture2D weaponTexture, Texture2D hairTexture)
            : base(ui, weaponTexture, hairTexture)
        {
            hud = ui;
            if (level != -1)
            {
                charge = CHARGEMAX[level];
            }
        }

        public override bool Fire()
        {

            if (active && !discharged && level != -1)
            {

                fired = false;

                if (charge >= DAMAGEMIN[level])
                {
                    if (charge < DAMAGEMAX[level])
                    {
                        damage = charge;
                    }

                    if (charge > DAMAGEMAX[level])
                    {
                        damage = DAMAGEMAX[level];
                    }
                        charge -= damage;
                        cooltime = COOLMAX[level];
                        fired = true;
                        shots++;//
                }
            }

            return fired;
        }

        public override void Cool()
        {
            if (level != -1)
            {
                if (charge < DAMAGEMIN[level])
                {
                    discharged = true;
                }

                else if (charge > DAMAGEMIN[level])
                {
                    discharged = false;
                }

                if (charge < CHARGEMAX[level])//recharging
                {

                    if (cooltime > 0)
                    {
                        cooltime--;
                    }
                    
                    else if (cooltime <= 0)
                    {
                        charging = true;
                        charge += CHARGEGAIN[level];
                    }
                }


                else if (charge >= CHARGEMAX[level])//charged
                {
                    discharged = false;
                    charging = false;
                    charge = CHARGEMAX[level];//reset overfill on delevel
                    shots = 0;//
                }
            }
        }

        public override void InstaCool()
        {
            discharged = false;
            charging = false;
            charge = CHARGEMAX[level];
        }

        public override void DrawUI(Vector2 mousePos, Vector2 playerPos, byte colour)
        {
            if (level != -1)
            {
                hud.DrawWeaponStatus(1, charge, CHARGEMAX[level], discharged, charging, Color.Red, Color.Orange);

                hud.DrawBool(16, 850, fired);

                hud.DrawInt(128, 800, shots);
                hud.DrawInt(128, 825, (shots * DAMAGEMIN[level]));
                hud.DrawInt(128, 850, (shots * DAMAGEMAX[level]));
            }
        }
    }
}
