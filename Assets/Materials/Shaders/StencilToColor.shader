Shader "Unlit/StencilToColor"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
            Stencil {
                Ref 1
                Comp Equal
            }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (float4 vertex : POSITION)
			{
				o.vertex = UnityObjectToClipPos(vertex);
				return o;
			}
			
			fixed4 frag () : SV_Target
			{
				return fixed4(1,1,1,1);
			}
			ENDCG
		}
	}
}
