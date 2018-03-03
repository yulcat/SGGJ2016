// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/Gummy" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_ShadowColor ("Shadow Color", Color) = (1,1,1,1)
		_RimPower ("Rim Power", float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		// Blend SrcAlpha OneMinusSrcAlpha
		// ZWrite On
		// ZTest Less
		
		Stencil {
            Ref 1
            Comp Always
            Pass Replace
        }
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard 

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		fixed4 _Color;
		fixed4 _ShadowColor;
		float _RimPower;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			half rimResult = pow (rim, _RimPower);
			o.Albedo = _Color * (1 - rimResult) + _ShadowColor * rimResult;
			// Metallic and smoothness come from slider variables
			o.Smoothness = 1 - rimResult;
			// o.Alpha = rimResult * 0.2 + 0.8;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
