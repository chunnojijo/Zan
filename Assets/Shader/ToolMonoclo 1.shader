Shader "Custom/ToolMonoclo1"
{
    Properties
    {

		_Color("Color", Color) = (1,1,1,1)
		_Color1("Color1", Color) = (1,1,1,1)
		_Color2("Color2", Color) = (1,1,1,1)
		_Color3("Color3", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_BlackValue1("BlackValue1",Range(0,1))=0.5
		_BlackValue2("BlackValue2",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf ToonRamp

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _Color1;
        fixed4 _Color2;
        fixed4 _Color3;
		float a;
		float _BlackValue1;
		float _BlackValue2;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)


		fixed4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten) 
		{
			half d = dot(s.Normal,lightDir);
			fixed3 ramp;
			if (d < _BlackValue1) 
			{
				ramp = _Color1;
			}else if (_BlackValue1 <=d && d<_BlackValue2) {
				ramp = _Color2;
			}
			else {
				ramp = _Color3;
			}

			fixed4 c;
			c.rgb = ramp;
			c.a = 0;
			return c;

		}
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
