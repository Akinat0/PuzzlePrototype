Shader "WaveBlendUnlitShader"
{
	Properties
	{
		_Frequency ("Frequency", Float) = 1
		_Speed ("Speed", Float) = 1
		_HAmplitude ("Horizontal amplitude", Range(0, 1)) = 1
		_VAmplitude ("Vectical amplitude", Range(0, 1)) = 1

		[Space(20)]
		[MaterialToggle] Mask ("Mask", Int) = 0
		_MaskTex ("Mask" , 2D) = "white" {}

		[Space(20)]
		[MaterialToggle] Fade ("Fade", Int) = 0
		_FadeSize ("Fade size", Float) = 1
		_FadeOffset ("Fade offset", Float) = 1

		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#pragma multi_compile _ CLIP
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#pragma multi_compile _ MASK_ON
			#pragma multi_compile _ FADE_ON

			#include "UnitySprites.cginc"

			struct vertData
			{
				float4 vertex : POSITION;
				float4 color  : COLOR;
				float2 uv     : TEXCOORD0;
			};

			struct fragData
			{
				float4 vertex : SV_POSITION;
				fixed4 color  : COLOR;
				float2 uv     : TEXCOORD0;
			};

			sampler2D _MaskTex;

			float _Frequency;
			float _Speed;
			float _HAmplitude;
			float _VAmplitude;
			float _FadeSize;
			float _FadeOffset;

			fragData vert(vertData IN)
			{
				fragData OUT;
				
				UNITY_SETUP_INSTANCE_ID (IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;
				OUT.color = IN.color * _RendererColor;
				
				return OUT;
			}
			
			fixed4 frag(fragData IN) : SV_Target
            {
                half2 offset = sin(IN.uv.yx * _Frequency + _Time.y * _Speed) * half2(_HAmplitude, _VAmplitude);                
                
                #if MASK_ON
				half2 uv = lerp(IN.uv, IN.uv + offset, tex2D(_MaskTex, IN.uv).a);
				#else
				half2 uv = IN.uv + offset;
				#endif
				
				fixed4 color = SampleSpriteTexture(uv) * IN.color;
                
                #if FADE_ON
                IN.uv -= 0.5;
                color.a *= 1 - clamp(dot(IN.uv, IN.uv) * _FadeSize - _FadeOffset, 0, 1);
                #endif
                
                return color;
            }
			
			ENDCG
		}
	}
}