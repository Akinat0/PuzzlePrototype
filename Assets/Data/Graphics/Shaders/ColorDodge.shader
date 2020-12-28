Shader "PhotoshopBlends"
{
	Properties
	{
		_Source ("Upper Layer", 2D) = "white" {}
		_Destination ("Lower Layer", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}

	Subshader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vertex_shader
			#pragma fragment pixel_shader

			#pragma target 3.0
			#include <UnityShaderUtilities.cginc>

			struct custom_type
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0; 
			};
			
			sampler2D _Destination, _Source;
			float3 _Color; 

			float3 colorDodge( float3 s, float3 d )
			{
				return d / (1.0 - s);
			}
			
			custom_type vertex_shader (float4 vertex:POSITION, float2 uv:TEXCOORD0)
			{
				custom_type vs;
				vs.vertex = UnityObjectToClipPos(vertex);
				vs.uv = uv;
				return vs;
			}

			float4 pixel_shader (custom_type ps) : SV_TARGET
			{
				float2 uv = ps.uv.xy;
				float3 s = tex2D(_Source, uv).xyz * _Color;
				float3 d = tex2D(_Destination, uv).xyz;
				float3 c = colorDodge(s,d);
				return float4(c,1.0);			
			}
			ENDCG
		}
	}
}