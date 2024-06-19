// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SphereGradient" {
	Properties{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_ColorFirst("First Color", Color) = (1,1,1,1)
		_ColorSecond("Second Color", Color) = (1,1,1,1)
		_Length("Length", Range(0.1, 10)) = 1
		_Speed("Speed", Range(0, 5)) = 1
		_xPos("xPos", Range(0, 1)) = 0.5
		_yPos("yPos", Range(0, 1)) = 0.5
	}

		SubShader{
			Tags {"Queue" = "Background"  "IgnoreProjector" = "True"}
			LOD 100

			ZWrite On

			Pass {
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _ColorFirst;
			fixed4 _ColorSecond;
			float _Speed;
			float _Length;
			float _xPos;
			float _yPos;

			struct v2f {
				float4 pos : SV_POSITION;
				float4 texcoord : TEXCOORD0;
			};

			v2f vert(appdata_full v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : COLOR {
				float dist = distance(i.texcoord, float2(_xPos, _yPos));
				dist += _Time.y * _Speed;
				dist *= _Length;
				int remainder = fmod(floor(dist), 2);
				float time = remainder == 1 ? 1 - frac(dist) : frac(dist);
				fixed4 c = lerp(_ColorFirst, _ColorSecond, time);
				c.a = 1;
				return c;
			}

			ENDCG
			}
		}
}