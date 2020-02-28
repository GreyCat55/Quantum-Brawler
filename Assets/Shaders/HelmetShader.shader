Shader "Unlit/HelmetShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue" = "Transparent+1"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			
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
				//fixed4 worldVertex : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//o.worldVertex = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 energyColor = fixed4(0, 1, (71.0 / 255.0), 1);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
			/*
				fixed4 bright = fixed4(1, 0, 0, 1.0);
				fixed4 dark = fixed4(0, 0, 1.0, 1.0);
				return lerp(dark,bright,fmod(abs(i.worldVertex.x), 1.0));
				*/
			
			if (dot(col, energyColor) > 1.5) {
				col.r += sin(i.vertex.x);
				col.g += cos(i.vertex.y);
				col.b += tan(i.vertex.z);
			}
			
			return col;
			}

			ENDCG
		}
	}
}
