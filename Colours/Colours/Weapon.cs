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
    abstract class Weapon
    {
        Texture2D texWep, texEff, texHair;
        Vector2 origin = new Vector2(0, 0);
        protected bool active;

        public const int BLOCKWIDTH = 128;
        public const int BLOCKHEIGHT = 64;

        protected sbyte level;//weapon level/unlocked status

        public sbyte Level { get { return level; } }

        protected const sbyte MINLEVEL = -1;
        protected const byte MAXLEVEL = 2;

        protected bool auto;
        
        public virtual bool Auto { get { return auto; } }

        protected byte number;
        public virtual byte Number { get { return number; } }


        Vector2 posadj = new Vector2(BLOCKWIDTH / 2, BLOCKHEIGHT / 2);
        Vector2 poswep = new Vector2(0, BLOCKHEIGHT);

        public bool Active { get { return active; } set { active = value; } }

        public Weapon()
        {
            level = 0;
            active = false;
            texWep = null;
        }

        /// <summary>
        /// Constructs a weapon with default values.
        /// </summary>
        public Weapon(UI ui, Texture2D weaponTexture)
        {
            level = -1;
            active = false;
            texWep = weaponTexture;
        }

        /// <summary>
        /// Constructs a weapon with default values.
        /// </summary>
        public Weapon(UI ui, Texture2D weaponTexture, Texture2D hairTexture)
        {
            level = -1;
            active = false;
            texWep = weaponTexture;
            texHair = hairTexture;
        }

        /// <summary>
        /// Constructs a weapon with default values and an Effect Texture.
        /// Use if effect is to be applied to weapon rather than to projectile.
        /// </summary>
        /// <param name="weaponTexture">Texture for Weapon</param>
        /// <param name="effectTexture">Texture for Effect</param>
        public Weapon(UI ui, Texture2D weaponTexture, Texture2D hairTexture, Texture2D effectTexture)
        {
            level = 0;
            active = false;
            texWep = weaponTexture;
            texEff = effectTexture;
            texHair = hairTexture;
        }

        /// <summary>
        /// Changes Weapon's level
        /// </summary>
        /// <param name="mode">0 = Decrement, 1 = Increment, 2 = Cycle, 3 = Assign</param>
        /// <param name="newlevel">-1 = Locked, 0/1/2 = Levels</param>
        public void ChangeLevel(byte mode, sbyte newlevel)
        {
            
            if (mode == 0 && level > MINLEVEL)
            {
                level--;
            }

            if (mode == 1 && level < MAXLEVEL)
            {
                level++;
            }

            if (mode == 2)
            {
                if (level < MAXLEVEL)
                {
                    level++;
                }

                else if (level == MAXLEVEL)
                {
                    level = MINLEVEL;
                }
            }

            if (mode == 3 && newlevel >= MINLEVEL && newlevel <= MAXLEVEL)
            {
                level = newlevel;
            }
        }

        public void Draw(SpriteBatch sprbat, Vector2 playerPos, byte colour, Vector2 mousePos)
        {
            if (active)
            {
                sprbat.Draw(texWep, (playerPos - posadj - poswep), new Rectangle(colour * BLOCKWIDTH, level * BLOCKHEIGHT, BLOCKWIDTH, BLOCKHEIGHT), Color.White, 0, origin, 1, 0, 0.001f);
            }
        }

        public abstract void DrawUI(Vector2 mousePos, Vector2 playerPos, byte colour);
        public abstract bool Fire();
        public abstract void Cool();
        public abstract void InstaCool();
    }
}
