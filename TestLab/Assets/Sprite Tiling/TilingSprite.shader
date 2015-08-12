Shader "2D/TilingSprite"
 {  
     Properties
     {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        [MaterialToggle] _FixedWidth ("Fixed Width", Float) = 0
        [MaterialToggle] _FixedHeight ("Fixed Height", Float) = 0
     }
     SubShader
     {
         Tags 
         { 
             "RenderType" = "Transparent" 
             "Queue" = "Transparent" 
         }
 
         Pass
         {
             ZWrite Off
             Blend SrcAlpha OneMinusSrcAlpha 
  
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
  
             sampler2D _MainTex;
             float _FixedWidth;
             float _FixedHeight;
 
             struct Vertex
             {
                 float4 vertex : POSITION;
                 float2 uv_MainTex : TEXCOORD0;
                 float4 color : COLOR;
             };
     
             struct Fragment
             {
                 float4 vertex : POSITION;
                 float2 uv_MainTex : TEXCOORD0;
                 fixed4 color : COLOR;
                 float2 uv_mult : TEXCOORD1;
             };
  
             Fragment vert(Vertex v)
             {
                 Fragment o;
     
                 o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                 o.uv_MainTex = v.uv_MainTex;
                 o.color = v.color;
                 o.uv_mult = float2(UNITY_MATRIX_MV[0][0],UNITY_MATRIX_MV[1][1]);
                 o.uv_mult = o.uv_mult + ((o.uv_mult/UNITY_MATRIX_MV[0][0]) - o.uv_mult) * _FixedWidth;
                 o.uv_mult = o.uv_mult + ((o.uv_mult/UNITY_MATRIX_MV[1][1]) - o.uv_mult) * _FixedHeight;
                 o.uv_mult = o.uv_mult + (float2(1,1) - o.uv_mult) * _FixedWidth * _FixedHeight;
                 return o;
             }
                                                     
             float4 frag(Fragment IN) : COLOR
             {
                 float2 uv;
                 uv.x = frac(IN.uv_MainTex.x * IN.uv_mult.x);
                 uv.y = frac(IN.uv_MainTex.y * IN.uv_mult.y);
                 float4 o = tex2D (_MainTex, uv) * IN.color;
                 return o;
             }
 
             ENDCG
         }
     }
 }