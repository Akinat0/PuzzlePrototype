Shader "UI/TutorialFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Aspect ("Aspect", Float) = 1
        _HoleSize ("Hole Size", Range(0, 1)) = 0.01
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
        
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 uv       : TEXCOORD0;
                float4 color    : COLOR;
            };

            sampler2D _MainTex;
 
            int _HolesLength;
            float _Aspect;
            float _HoleSize;
            float4 _Holes[5];

            v2f vert (appdata v)
            {
                v2f o;
                
                o.vertex   = UnityObjectToClipPos(v.vertex);
                o.uv       = v.uv;
                o.color    = v.color;

                return o;
            }

            void Discard(float2 uv)
            {
                for (int i = 0; i < _HolesLength; i++)
                {
                    if (uv.x >= _Holes[i].x && uv.x <= _Holes[i].z && uv.y >= _Holes[i].y && uv.y <= _Holes[i].w)
                        discard;
                }
            }

            float GetSqrDistance(float2 uv, float4 rect)
            {
                float width = rect.z - rect.x;
                float height = rect.w - rect.y;
                
                float2 center = float2(rect.x + width/2, rect.y + height/2);

                float dx = max(abs(uv.x - center.x) - width/2, 0);
                float dy = max(abs(uv.y - center.y) - height/2, 0) / _Aspect;

                return dx * dx + dy * dy;
            }
            
            float GetClosestHoleDistance(float2 uv)
            {
                float closestDist = GetSqrDistance(uv, _Holes[0]);
                
                for (int i = 1; i < _HolesLength; i++)
                    closestDist = min(GetSqrDistance(uv, _Holes[i]), closestDist);

                return closestDist;
            }

            float4 GetFadeColor(float distance, float4 color)
            {
                float a = smoothstep(distance, 0, _HoleSize * _HoleSize);
                color.a *= a;
                return color;
            }
            
            fixed4 frag(v2f IN) : SV_Target
            {
                float4 color = tex2D(_MainTex, IN.uv) * IN.color;

                Discard(IN.uv);

                float distance = GetClosestHoleDistance(IN.uv);
                
                color = GetFadeColor(distance, color);
                
                return color;
            }
            
            ENDCG
        }
    }
}
