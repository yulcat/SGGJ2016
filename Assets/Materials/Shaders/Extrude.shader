Shader "Example/Normal Extrusion" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
	  _Normal ("Normal", 2D) = "white" {}
      _Amount ("Extrusion Amount", Range(-1,1)) = 0.5
	  _Color ("Back Color", Color) = (0,0,0,1)
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
      struct Input {
          float2 uv_MainTex;
      };
      float _Amount;
      void vert (inout appdata_full v) {
          v.vertex.xyz += v.normal * _Amount;
      }
      sampler2D _MainTex;
	  sampler2D _Normal;
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
		  o.Normal = UnpackNormal (tex2D (_Normal, IN.uv_MainTex));
      }
      ENDCG
	  Pass
        {
			Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "UnityCG.cginc"
			float _Amount;
            
            float4 vert (appdata_base i) : SV_POSITION
            {
                return UnityObjectToClipPos(i.vertex + i.normal * _Amount);
            }

            fixed4 _Color;

            fixed4 frag () : SV_Target
            {
                return _Color; // just return it
            }
            ENDCG
        }
    } 
    Fallback "Diffuse"
  }