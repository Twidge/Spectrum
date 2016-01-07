Shader "Custom/ConditionalShader"
{
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_UseWhite("Use White", float) = 0.0
		_Proximity("Proximity", Range(0,4)) = 0.0
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

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		uniform float _UseWhite;
		uniform float _Proximity;

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			float vis = 0.6;
			float adjust = 0.15;
			float variance = 0.06;
			float speed = 100;
			
			fixed4 c = _Color;
			o.Albedo = c.rgb;
			
			if(_UseWhite != 1.0f && _Proximity > 0.0f)
			{
				if((sin(IN.worldPos.x * _Time.x) * cos(IN.worldPos.z + IN.worldPos.x)) + (cos(IN.worldPos.y * IN.worldPos.z * _Time.x) * sin(IN.worldPos.z - (IN.worldPos.y * IN.worldPos.x * _Time.x))) + 2 < _Proximity)
				{
					o.Albedo = (1,1,1,1);
					o.Albedo.r = o.Albedo.r - (c.g * vis) - (c.b * vis) - variance * cos(sin((IN.worldPos.x + IN.worldPos.y))) - variance * sin(cos((IN.worldPos.y)));
					o.Albedo.g = o.Albedo.g - (c.r * vis) - (c.b * vis) - variance * sin(cos((IN.worldPos.y + IN.worldPos.x))) - variance * cos(cos((IN.worldPos.x)));
					o.Albedo.b = o.Albedo.b - (c.r * vis) - (c.g * vis) - variance * cos(cos((IN.worldPos.x))) - variance * sin(sin((IN.worldPos.x + IN.worldPos.y)));
				}
			}
			
			if(_UseWhite == 1.0f)
			{
				o.Albedo = (1,1,1,1);
				o.Albedo.r = o.Albedo.r - (c.g * vis) - (c.b * vis) - (adjust * cos(_Time.x * speed)) - variance * cos(sin((IN.worldPos.x + IN.worldPos.y + speed * _Time.x))) - variance * sin(cos((IN.worldPos.y)));
				o.Albedo.g = o.Albedo.g - (c.r * vis) - (c.b * vis) - (adjust * cos(_Time.x * speed)) - variance * sin(cos((IN.worldPos.y + IN.worldPos.x + speed * _Time.x))) - variance * cos(cos((IN.worldPos.x)));
				o.Albedo.b = o.Albedo.b - (c.r * vis) - (c.g * vis) - (adjust * cos(_Time.x * speed)) - variance * cos(cos((IN.worldPos.x + speed * _Time.x))) - variance * sin(sin((IN.worldPos.x + IN.worldPos.y)));
			}
			
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}