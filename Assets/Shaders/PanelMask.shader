Shader "Unlit/PanelMask"
{
     Properties{
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _MaskTex("Mask", 2D) = "white" {}
    }
        SubShader{
            Tags { "RenderType" = "Opaque" "Queue" = "Transparent"}
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma surface surf Standard keepalpha
            #pragma target 3.0

            sampler2D _MainTex;
            sampler2D _MaskTex;

            struct Input {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutputStandard o) {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                fixed4 m = tex2D(_MaskTex, IN.uv_MainTex);
                c.a = m.r;
                o.Albedo = c.rgb*0.3;
                o.Alpha = c.a;
            }
            ENDCG
    }
        FallBack "Diffuse"
}
