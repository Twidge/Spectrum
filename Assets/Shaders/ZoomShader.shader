Shader "Custom/ZoomShader"
{
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_ZoomMode("Zoom Mode", float) = 0.0
		_ZoomSpeed("Zoom Speed", Range(0,100)) = 0.0
	}
	SubShader {
		Tags
		{
			"Queue" = "Geometry"
		}
		
		LOD 200
		
		Blend One One
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		uniform float _ZoomMode;
		uniform float _ZoomSpeed;

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = _Color;
			
			if(_ZoomMode == 1.0f)
			{
				c = tex2D (_MainTex, float2(IN.uv_MainTex.x - (_ZoomSpeed * _Time.x), IN.uv_MainTex.y));
			}
			
			o.Albedo = c.rgb;
			
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}