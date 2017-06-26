Shader "Custom/Fog" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_FogColor("Fog Color", Color) = (1, 1, 1, 1)		
		_FogStart("Fog Start", Float) = 0
		_FogEnd("Fog End", Float) = 0

		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows finalcolor:mycolor vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			half fog;
		};

		fixed4 _Color;
		fixed4 _FogColor;
		float _FogStart;
		float _FogEnd;

		half _Glossiness;
		half _Metallic;

		void vert(inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);
			float4 pos = mul(unity_ObjectToWorld, v.vertex);
			data.fog = saturate((_FogStart - v.vertex.y) / (_FogStart - _FogEnd));
		}

		void mycolor(Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
			fixed3 fogColor = _FogColor.rgb;
			fixed3 tintColor = _Color.rgb;

			color.rgb = lerp(color.rgb * tintColor, fogColor, IN.fog);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{			
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
