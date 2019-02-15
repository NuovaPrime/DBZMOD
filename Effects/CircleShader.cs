using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DBZMOD.Effects
{
    public class CircleShader : ShaderData
    {
        public CircleShader(Ref<Effect> shader, string passName) : base(shader, passName) { }
        public void ApplyShader(int radius)
        {
            Shader.Parameters["radius"].SetValue(radius);
            Apply();
        }
    }
}
