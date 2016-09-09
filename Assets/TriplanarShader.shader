// Custom Triplanar Shader heavily based on http://www.martinpalko.com/triplanar-mapping/
Shader "Custom/TriplanarShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_TextureScale("Texture Scale",float) = 1
		_TriplanarBlendSharpness("Blend Sharpness",float) = 1
		_TexTop("Albedo Top (RGB)", 2D) = "white" {}
		_TexMid("Albedo Middle (RGB)", 2D) = "white" {}
		_TexBot("Albedo Bottom (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _TexTop;
		sampler2D _TexMid;
		sampler2D _TexBot;

		struct Input {
			float3 worldPos;
			float3 worldNormal;
		};

		half _Glossiness;
		half _Metallic;
		float _TextureScale;
		float _TriplanarBlendSharpness;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			
			// Find our UVs for each axis based on world position of the fragment.
			half2 yUV = IN.worldPos.xz / _TextureScale;
			half2 xUV = IN.worldPos.zy / _TextureScale;
			half2 zUV = IN.worldPos.xy / _TextureScale;

			// Now do texture samples from our diffuse map with each of the 3 UV set's we've just made.
			half3 topDiff = tex2D(_TexTop, yUV);
			half3 botDiff = tex2D(_TexBot, yUV);
			half3 midDiffX = tex2D(_TexMid, xUV);
			half3 midDiffZ = tex2D(_TexMid, zUV);

			// Get the absolute value of the world normal.
			// Put the blend weights to the power of BlendSharpness, the higher the value, 
			// the sharper the transition between the planar maps will be.
			half3 blendWeights = pow(abs(IN.worldNormal), _TriplanarBlendSharpness);

			// Divide our blend mask by the sum of it's components, this will make x+y+z=1
			blendWeights = blendWeights / (blendWeights.x + blendWeights.y + blendWeights.z);

			// Finally, blend together all three samples based on the blend mask.
			o.Albedo = midDiffX * blendWeights.x
				+ topDiff * blendWeights.y * clamp(IN.worldNormal.y, 0.0f, 1.0f)
				+ botDiff * blendWeights.y * clamp(-IN.worldNormal.y, 0.0f, 1.0f)
				+ midDiffZ * blendWeights.z;
			
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
