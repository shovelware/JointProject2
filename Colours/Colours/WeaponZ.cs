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
    class WeaponZ : Weapon
    {
        UI hud;

        public WeaponZ(UI ui, Texture2D weaponTexture)
            : base(ui, weaponTexture)
        {
            hud = ui;
        }

        public override bool Fire()
        {
            return false;
        }

        public override void Cool()
        {
        }


        public override void InstaCool()
        {
        }


        public override void DrawUI(Vector2 mousePos, Vector2 playerPos, byte colour)
        {
        }
    }
}
