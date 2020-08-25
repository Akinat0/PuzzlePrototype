Shader "Custom/FrostShader"
{
	Properties
	{
		_BlendTex ("Image", 2D) = "" {}
		
	    _BlendAmount ("BlendAmount", Float) = 0
	    _EdgeSharpness ("EdgeSharpness", Float) = 1 // >= 1
	    _SeeThroughness ("SeeThroughness", Float) = 0.2
	    
	    _Intensity ("Intensity", Float) = 15
	    _Vignette ("Vignette", Range(0, 1)) = 0.25
	}
	
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f
	{
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};
	
	sampler2D _BlendTex;
	
	float _BlendAmount;
	float _EdgeSharpness;
	float _SeeThroughness;
	float _Distortion;
	float _Intensity;
	float _Vignette;
		
	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	} 
	
	float4 getVignette(float2 uv){
	
	    uv *= 1 - uv.yx;
	    float vig = uv.x * uv.y * _Intensity;
	    vig = pow(vig, _Vignette);
	    return vig;
	}
	
	half4 frag(v2f i) : COLOR
	{ 
	    float vig = getVignette(i.uv);
	    
		float4 blendColor = tex2D(_BlendTex, i.uv);
		blendColor.a *= (1 - vig);

		float4 overlayColor = blendColor;
		overlayColor.rgb = (blendColor.rgb + 0.5) * (blendColor.rgb + 0.5); //double overlay
	
        blendColor = lerp(blendColor, overlayColor, _SeeThroughness);

		return blendColor;
	}

	ENDCG 
	
	Subshader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			
			Blend SrcAlpha One
			
			Fog
			{
				Mode off
			}

			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest 
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}

	Fallback off	
} 