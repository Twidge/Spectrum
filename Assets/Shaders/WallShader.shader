Shader "Custom/WallShader"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	
	SubShader
	{
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

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			float variance = 0.12;
			float speed = 25;
			
			IN.worldPos = IN.worldPos / 10;
		
			// Albedo comes from a texture tinted by color
			
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Albedo.r = o.Albedo.r + variance * cos(sin((IN.worldPos.x + IN.worldPos.y + speed * _Time.x))) - variance * sin(cos((IN.worldPos.y)));
			o.Albedo.g = o.Albedo.g + variance * sin(cos((IN.worldPos.y + IN.worldPos.x + speed * _Time.x))) - variance * cos(cos((IN.worldPos.x)));
			o.Albedo.b = o.Albedo.b + variance * cos(cos((IN.worldPos.x + speed * _Time.x))) - variance * sin(sin((IN.worldPos.x + IN.worldPos.y)));
			
			// Metallic and smoothness come from slider variables
			
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
