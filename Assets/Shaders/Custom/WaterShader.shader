Shader "Unlit/WaveShader"
{
    Properties
    {
       // _MainTex ("Texture", 2D) = "white" {}
        _amplitude("Amplitude", Range(0,5)) = 1
        _length("Length", Range(0,5)) = 2
        _speed("Speed", Range(0,5)) = 1
        _color("Color", Color) = (1.,1.,1.,1)
        _offset("Offset", Range(-100,100)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            //sampler2D _MainTex;
            //float4 _MainTex_ST;
            float _amplitude;
            float _length;
            float _speed;
            fixed4 _color;
            float _offset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_M, v.vertex);
                o.vertex.y = sin(_Time.y * _speed + o.vertex.x / _length) * _amplitude + _offset;
                o.vertex = mul(UNITY_MATRIX_V, o.vertex);
                o.vertex = mul(UNITY_MATRIX_P, o.vertex);
                o.color = _color;
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return _color;
            }
            ENDCG
        }
    }
}
