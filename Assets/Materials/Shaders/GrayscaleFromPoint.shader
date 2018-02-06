Shader "Effects/GrayscaleFromPoint"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_GrayscaleCenter ("Center", Vector) = (0.5, 0.5, 0, 0)
		_GrayscaleRadius ("Radius", float) = 0.3
		_GrayscaleLerp ("Lerp", float) = 200
		_Pow("Contrast", float) = 2
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			
			float2 _GrayscaleCenter;
			float _GrayscaleRadius;
			float _GrayscaleLerp;
			float _Pow;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				half3 lum = Luminance(col.rgb);
				float width = _MainTex_TexelSize.z / _MainTex_TexelSize.w;
				float dx = (i.uv.x - _GrayscaleCenter.x) * width;
				float dy = (i.uv.y - _GrayscaleCenter.y);
				float dist = dx * dx + dy * dy;
				return lerp(col, saturate(pow(fixed4(lum,1), _Pow)), saturate(-(dist-_GrayscaleRadius) * _GrayscaleLerp));
			}
			ENDCG
		}
	}
}
