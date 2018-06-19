using Terraria;
using Terraria.ModLoader;

namespace DBZMOD.Namek
{
    public class Namekbg : ModSurfaceBgStyle
    {
        public override bool ChooseBgStyle()
        {
            return !Main.gameMenu && Main.player[Main.myPlayer].GetModPlayer<MyPlayer>(mod).ZoneCustomBiome; 
        }

        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                    {
                        fades[i] = 1f;
                    }
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                    {
                        fades[i] = 0f;
                    }
                }
            }
        }

        public override int ChooseFarTexture()
        {
            return mod.GetBackgroundSlot("Backgrounds/namekbg");    //this is the surface biome far bg
        }


        public override int ChooseMiddleTexture()
        {
            return mod.GetBackgroundSlot("Backgrounds/namekbg");      //this is the surface biome middle gackground
        }


        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return mod.GetBackgroundSlot("Backgrounds/namekbg");      //this is the surface biome close gackground
        }
    }
}