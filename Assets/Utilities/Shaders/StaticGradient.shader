// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/StaticGradient" {
	Properties{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_ColorFirst("First Color", Color) = (1,1,1,1)
		_ColorSecond("Second Color", Color) = (1,1,1,1)
	}

		SubShader{
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			LOD 100

			 ZWrite On
			 Blend SrcAlpha OneMinusSrcAlpha

			Pass {
			CGPROGRAM
			#pragma vertex vert alpha
			#pragma fragment frag alpha
			#include "UnityCG.cginc"

			sampler2D  _MainTex;
			fixed4 _ColorFirst;
			fixed4 _ColorSecond;

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
				fixed4 c = lerp(_ColorFirst, _ColorSecond, i.texcoord.x);
				float4 currentPixelColor = tex2D(_MainTex, i.texcoord.xy);
				c.a = currentPixelColor.a;
				return c;
			}
			ENDCG
			}
		}
}