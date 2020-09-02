Shader "Blur/BoxBlur"
{
	Properties
	{
	    _MainTex ("MainTex", 2D) = "white" {}
	}

	SubShader
	{
		Cull Off
		Lighting Off
		ZWrite Off
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appData
			{
				float4 vertex : POSITION;			
				float2 uv     : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv     : TEXCOORD0;
			};

			v2f vert(appData IN)
			{
				v2f OUT;
				
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;
				
				return OUT;
			}
			
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			
			float4 box(sampler2D tex, float2 uv, float4 texelSize)
			{
			    float4 color;
			    
			    color = tex2D(tex, uv + float2(-texelSize.x, -texelSize.y)) + 
			            tex2D(tex, uv + float2(0, -texelSize.y)) +
			            tex2D(tex, uv + float2(texelSize.x, -texelSize.y)) +
			            tex2D(tex, uv + float2(-texelSize.x, 0)) +
			            tex2D(tex, uv + float2(0, 0)) +
			            tex2D(tex, uv + float2(texelSize.x, 0)) +
			            tex2D(tex, uv + float2(-texelSize.x, texelSize.y)) +
			            tex2D(tex, uv + float2(0, texelSize.y)) +
			            tex2D(tex, uv + float2(texelSize.x, texelSize.y));
			            
                color = color / 9;			            
			    return color;    
			}
			
			fixed4 frag(v2f IN) : SV_Target
            {
                float4 color = box(_MainTex, IN.uv, _MainTex_TexelSize);
                return color;
            }
			
			ENDCG
		}
	}
}