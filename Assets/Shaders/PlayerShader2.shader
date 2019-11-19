Shader "Custom/PlayerShader2" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
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

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		
		half4 _In0;
		half4 _Out0;
		half4 _In1;
		half4 _Out1;
		half4 _In2;
		half4 _Out2;

		half4 frag (v2f i) : SV_Target
		{
			half4 col = tex2D(_MainTex, i.uv);

			if (all(col.rgb == _In0.rgb))
				return _Out0
			if (all(col.rgb == _In1.rgb))
				return _Out1
			if (all(col.rgb == _In2.rgb))
				return _Out2
			
			return col
		}

		Material _mat;

		void OnEnable() {
			Shader shader = Shader.find("Hidden/PaletteSwap")

			if (_mat == null) {
				_mat = new Material(shader);
			}
		}

		void OnRender(RenderTexture src, RenderTexture dst) {
			_mat.SetColor("_In0", In0)
			_mat.SetColor("_Out0", Out0)
			_mat.SetColor("_In0", In1)
			_mat.SetColor("_Out0", Out1)
			_mat.SetColor("_In0", In2)
			_mat.SetColor("_Out0", Out2)

			Graphics.Blit(src, dst, _mat)
		}

		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
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
