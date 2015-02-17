Shader "DeferredShading/CombineEmission" {

Properties {
    g_intensity ("Intensity", Vector) = (1.0, 1.0, 1.0, 1.0)
}
SubShader {
    Tags { "RenderType"="Opaque" }
    ZTest Always
    ZWrite Off
    Cull Back

CGINCLUDE
#include "Compat.cginc"
sampler2D g_emission_buffer;
float4 g_intensity;

struct ia_out
{
    float4 vertex : POSITION;
};

struct vs_out
{
    float4 vertex : SV_POSITION;
    float4 screen_pos : TEXCOORD0;
};

struct ps_out
{
    float4 color : COLOR0;
};


vs_out vert(ia_out io)
{
    vs_out o;
    o.vertex = io.vertex;
    o.screen_pos = io.vertex;
    return o;
}

ps_out frag(vs_out vo)
{
    float2 coord = (vo.screen_pos.xy / vo.screen_pos.w + 1.0) * 0.5;
    #if UNITY_UV_STARTS_AT_TOP
        coord.y = 1.0-coord.y;
    #endif
    ps_out po = { tex2D(g_emission_buffer, coord)*g_intensity };
    return po;
}
ENDCG

    Pass {
        Blend One One

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma target 3.0
        #ifdef SHADER_API_OPENGL 
            #pragma glsl
        #endif
        ENDCG
    }
}

}
