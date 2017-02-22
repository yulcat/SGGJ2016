Shader "Unlit/UVApplier"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_UVTex("UV Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags 
         { 
             "RenderType" = "Transparent" 
             "Queue" = "Transparent" 
         }

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _UVTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 uvTexData = tex2D(_UVTex, i.uv);
				fixed2 uv = fixed2(uvTexData.x, uvTexData.y);
				fixed4 mainTexColor = tex2D(_MainTex, uv);
				mainTexColor.rgb = mainTexColor.rgb * uvTexData.z;
				if(uvTexData.b < 0.5)
				{
					mainTexColor.a = 0;
				}
				// apply fog
				return mainTexColor;
			}
			ENDCG
		}
	}
}
