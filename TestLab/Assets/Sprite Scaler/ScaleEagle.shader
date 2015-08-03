Shader "2D/Eagle"
 {  
     Properties
     {
        _MainTex ("Sprite Texture", 2D) = "white" {}
     }
     SubShader
     {
         Tags 
         { 
             "RenderType" = "Opaque" 
             "Queue" = "Transparent+1" 
         }
 
         Pass
         {
             ZWrite Off
             Blend SrcAlpha OneMinusSrcAlpha 
  
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
  
             sampler2D _MainTex;
             float4 _MainTex_TexelSize;
 
             struct Vertex
             {
                 float4 vertex : POSITION;
                 float2 uv : TEXCOORD0;
                 float4 color : COLOR;
             };
     
             struct Fragment
             {
                 float4 vertex : POSITION;
                 float2 uv : TEXCOORD0;
                 fixed4 color : COLOR;
             };
             
             half isNotEqual(float4 in1, float4 in2)
             {
                float4 dist = in1 - in2;
                half isEqual = dot(dist,dist);
                return isEqual;
             }
  
             Fragment vert(Vertex v)
             {
                 Fragment o;
     
                 o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                 o.uv = v.uv;
                 o.color = v.color;
                 return o;
             }
                                                     
             float4 frag(Fragment IN) : COLOR
             {
                 float4 o = float4(0,0,0,1);
                 float x = (IN.uv.x /_MainTex_TexelSize.x);
                 float y = (IN.uv.y /_MainTex_TexelSize.y);
                 half xInt = floor(x);
                 half yInt = floor(y);
                 half xDirc = floor(frac(x)*2)*2-1;
                 half yDirc = floor(frac(y)*2)*2-1;
                 float offset = 0.3;
                 float4 c1 = tex2D (_MainTex, float2(
                 	(xInt+xDirc+offset)*_MainTex_TexelSize.x, (yInt+yDirc+offset)*_MainTex_TexelSize.y));
                 float4 c2 = tex2D (_MainTex, float2(
                 	(xInt+xDirc+offset)*_MainTex_TexelSize.x, (yInt+offset)*_MainTex_TexelSize.y));
                 float4 c3 = tex2D (_MainTex, float2(
                 	(xInt+offset)*_MainTex_TexelSize.x, (yInt+yDirc+offset)*_MainTex_TexelSize.y));
                 float4 c = tex2D (_MainTex, float2(
                 	(xInt+offset)*_MainTex_TexelSize.x, (yInt+offset)*_MainTex_TexelSize.y));
                 if (isNotEqual(c1,c3) || isNotEqual(c1,c2)){
                    o = c;
                 }else{
                    o = c1;
                 }
                 return o;
             }
 
             ENDCG
         }
     }
 }