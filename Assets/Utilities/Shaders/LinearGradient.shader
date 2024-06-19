// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/LinearGradient" {
	Properties{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_ColorFirst("First Color", Color) = (1,1,1,1)
		_ColorSecond("Second Color", Color) = (1,1,1,1)
		_Length("Length", Range(0.1, 10)) = 1
		_Speed("Speed", Range(0, 5)) = 1
		_xMultiply("xMultiply", Range(-1.0, 1.0)) = 0
		_yMultiply("yMultiply", Range(-1.0, 1.0)) = 0
	}

		SubShader{
			Tags {"Queue" = "Transparent"  "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			LOD 100

			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha

			Pass {
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D  _MainTex;
			fixed4 _ColorFirst;
			fixed4 _ColorSecond;

			float _Speed;
			float _Length;
			float _xMultiply;
			float _yMultiply;

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
				float nest = _Time.y * _Speed + i.texcoord.y * _Length * _yMultiply + i.texcoord.x * _Length * _xMultiply;
				int remainder = fmod(floor(nest), 2);
				float time = remainder == 1 ? 1 - frac(nest) : frac(nest);
				fixed4 c = lerp(_ColorFirst, _ColorSecond, time);
				float4 currentPixelColor = tex2D(_MainTex, i.texcoord.xy);
				c.a = currentPixelColor.a;
				return c;
			}
			ENDCG
			}
		}
}