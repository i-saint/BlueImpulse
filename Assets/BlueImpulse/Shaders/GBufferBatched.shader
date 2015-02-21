Shader "BlueImpulse/GBufferBatched" {

Properties {
    _BaseColor ("BaseColor", Vector) = (0.15, 0.15, 0.2, 5.0)
    _GlowColor ("GlowColor", Vector) = (0.0, 0.0, 0.0, 0.0)
    g_line_color ("LineColor", Vector) = (0.45, 0.4, 2.0, 0.0)
    _Gloss ("Gloss", Float) = 1.0
}
SubShader {
    Tags { "RenderType"="Opaque" "Queue"="Geometry" }

CGINCLUDE
#include "UnityCG.cginc"

#define WITHOUT_COMMON_VERT_FRAG
#include "../../DeferredShading/Shaders/Compat.cginc"
#include "../../DeferredShading/Shaders/DS.cginc"
#include "../../DeferredShading/Shaders/DSGBuffer.cginc"
#include "../../BatchRenderer/Shaders/math.cginc"
#include "Glowline.cginc"

#if (defined(SHADER_API_D3D11) || defined(SHADER_API_PSSL)) && !defined(ALWAYS_USE_TEXTURE_DATA_SOURCE)
    #define WITH_STRUCTURED_BUFFER
#endif


int     g_num_instances;
float4  g_scale;
int     g_batch_begin;

int     GetInstanceID(float2 i) { return i.x + g_batch_begin; }
int     GetNumInstances()       { return g_num_instances; }
float3  GetBaseScale()          { return g_scale.xyz; }
int     GetBatchBegin()         { return g_batch_begin; }


#ifdef WITH_STRUCTURED_BUFFER

struct InstanceData
{
    float3 translation;
    float4 rotation;
    float scale;
    float time;
    float id;
};
StructuredBuffer<InstanceData> g_instance_buffer;

float3 GetInstanceTranslation(int i){ return g_instance_buffer[i].translation; }
float4 GetInstanceRotation(int i)   { return g_instance_buffer[i].rotation; }
float  GetInstanceScale(int i)      { return g_instance_buffer[i].scale; }
float  GetInstanceTime(int i)       { return g_instance_buffer[i].time; }
float  GetObjectID(int i)           { return g_instance_buffer[i].id; }

#else // WITH_STRUCTURED_BUFFER

float3 GetInstanceTranslation(int i){ return 0.0; }
float4 GetInstanceRotation(int i)   { return 0.0; }
float3 GetInstanceScale(int i)      { return 0.0; }
float  GetInstanceTime(int i)       { return 0.0; }
float  GetObjectID(int i)         { return 0.0; }

#endif // WITH_STRUCTURED_BUFFER

void ApplyInstanceTransform(float2 id, inout float4 vertex, inout float3 normal)
{
    int instance_id = GetBatchBegin() + id.x;
    if(instance_id >= GetNumInstances()) {
        vertex.xyz *= 0.0;
        return;
    }

    vertex.xyz *= GetBaseScale();
    vertex.xyz *= GetInstanceScale(instance_id);
    {
        float3x3 rot = quaternion_to_matrix33(GetInstanceRotation(instance_id));
        vertex.xyz = mul(rot, vertex.xyz);
        normal.xyz = mul(rot, normal.xyz);
        //tangent.xyz = mul(rot, tangent.xyz);
    }
    vertex.xyz += GetInstanceTranslation(instance_id);
}


struct my_vs_out
{
    float4 vertex : SV_POSITION;
    float4 screen_pos : TEXCOORD0;
    float4 position : TEXCOORD1;
    float3 normal : TEXCOORD2;
    float3 local_position : TEXCOORD3;
    float3 local_normal : TEXCOORD4;
    float4 instance_pos : TEXCOORD5;
    float4 params : TEXCOORD6;
};

float4 g_line_color;
float4 g_sphere;

my_vs_out vert(appdata_full v)
{
    int iid = GetInstanceID(v.texcoord1.xy);

    my_vs_out o;
    o.local_position = v.vertex.xyz * GetInstanceScale(iid) + hash(GetObjectID(iid))*10.0;
    o.local_normal = v.normal.xyz;

    ApplyInstanceTransform(v.texcoord1.xy, v.vertex, v.normal);

    float4 vmvp = mul(UNITY_MATRIX_VP, v.vertex);
    o.vertex = vmvp;
    o.screen_pos = vmvp;
    o.position = v.vertex;
    o.normal = v.normal.xyz;
    o.instance_pos = float4(GetInstanceTranslation(iid), iid);
    o.params = float4(GetObjectID(iid), GetInstanceTime(iid), 0.0, 0.0);
    return o;
}

float4 frag_depth(my_vs_out i) : SV_TARGET
{
    return 0.0;
}

ps_out frag(my_vs_out i)
{
    float objid = i.params.x;
    float objtime = i.params.y;

    float4 glow = _GlowColor;

    float2 dg = boxcell((i.local_position.xyz)*0.1, i.local_normal.xyz);

    float pc = 1.0-clamp(1.0 - max(min(dg.x, 2.0)-1.0, 0.0)*2.0, 0.0, 1.0);
    float d = -length(i.position.xyz)*0.15 - dg.y*0.5;
    float vg = max(0.0, frac(1.0-d*0.75-_Time.y*0.25)*3.0-2.0) * pc;
    glow += g_line_color * vg * 1.5;

    float extrude = dg.y*4.0 - 6.0 + dg.x*0.5;
    float3 sphere_pos = g_sphere.xyz;
    float sphere_radius = g_sphere.w + extrude;
    float3 s_normal = normalize(_WorldSpaceCameraPos.xyz - i.position.xyz);
    float3 pos_rel = i.position.xyz - sphere_pos;
    float s_dist = abs(dot(pos_rel, s_normal));
    float3 pos_proj = i.position.xyz - s_dist*s_normal;
    float dist_proj = length(pos_proj-sphere_pos);
    if(dist_proj>sphere_radius) {
        discard;
    }

    ps_out o;
    float len = length(pos_rel);
    if(len<sphere_radius) {
        o.normal = float4(i.normal.xyz, _Gloss);
        o.position = float4(i.position.xyz, i.screen_pos.z);
    }
    else {
        float s_dist2 = length(pos_proj-sphere_pos);
        float s_dist3 = sqrt(sphere_radius*sphere_radius - s_dist2*s_dist2);
        float3 ps = pos_proj + s_normal * s_dist3;

        float3 dir = normalize(ps-sphere_pos);
        float3 pos = sphere_pos+dir*sphere_radius;
        float4 spos = mul(UNITY_MATRIX_VP, float4(pos,1.0));
        o.normal = float4(dir, _Gloss);
        o.position = float4(pos, spos.z);
    }
    glow.rgb += float3(0.2, 0.2, 0.7) * max(1.0 - abs(dist_proj-sphere_radius)*0.5, 0.0)*5.0;


    o.color = _BaseColor;
    o.glow = glow;
    return o;
}
    ENDCG
    
    Pass {
        Name "DepthPrePass"
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1" }
        Cull Back
        ZWrite On
        ZTest Less
        ColorMask 0

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma target 3.0
        #ifdef SHADER_API_OPENGL 
            #pragma glsl
        #endif
        ENDCG
    }

    Pass {
        Name "GBuffer"
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        Cull Back
        ZWrite Off
        ZTest Equal

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
