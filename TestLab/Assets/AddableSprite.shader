Shader "2D/AddableSprite"
 {  
     Properties
     {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _AddColor ("Added Color", Color) = (0,0,0,1)
        _ReadyRate ("Shadowed sprite rate", float) = 1
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
             #pragma multi_compile DUMMY PIXELSNAP_ON
  
             sampler2D _MainTex;
             fixed4 _AddColor;
             half _ReadyRate;
 
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
             };
  
             Fragment vert(Vertex v)
             {
                 Fragment o;
     
                 o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                 o.uv_MainTex = v.uv_MainTex;
                 o.color = v.color;
     
                 return o;
             }
                                                     
             float4 frag(Fragment IN) : COLOR
             {
                 float4 o;
                 int isShadowed = step(IN.uv_MainTex.y,_ReadyRate);
                 fixed4 normal = tex2D (_MainTex, IN.uv_MainTex);
                 normal.rgb = normal.rgb * (!_ReadyRate * 0.3 + 0.7);
                 fixed shadowColor = (max(max(normal.r,normal.g),normal.b) + min(min(normal.r,normal.g),normal.b))/2 * 0.7;
                 fixed4 shadow = fixed4(shadowColor,shadowColor,shadowColor,normal.a);
                 o = normal + (shadow - normal) * isShadowed;
                 o.rgb = o.rgb * IN.color + _AddColor;
                     
                 return o;
             }
 
             ENDCG
         }
     }
 }