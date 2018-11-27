using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
