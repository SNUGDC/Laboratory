Shader "2D/Eagle"
 {  
     Properties
     {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _AddColor ("Added Color", Color) = (0,0,0,1)
        _width ("Sprite width", Int) = 64
        _height ("Sprite height", Int) = 128
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
             #pragma multi_compile DUMMY PIXELSNAP_ON
  
             sampler2D _MainTex;
             fixed4 _AddColor;
             float _width;
             float _height;
 
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
                 float x = (IN.uv.x * _width);
                 float y = (IN.uv.y * _height);
                 half xInt = floor(x);
                 half yInt = floor(y);
                 half xDirc = floor(frac(x)*2)*2-1;
                 half yDirc = floor(frac(y)*2)*2-1;
                 float offset = 0.3;
                 float4 c1 = tex2D (_MainTex, float2((xInt+xDirc+offset)/_width, (yInt+yDirc+offset)/_height));
                 float4 c2 = tex2D (_MainTex, float2((xInt+xDirc+offset)/_width, (yInt+offset)/_height));
                 float4 c3 = tex2D (_MainTex, float2((xInt+offset)/_width, (yInt+yDirc+offset)/_height));
                 float4 c = tex2D (_MainTex, float2((xInt+offset)/_width, (yInt+offset)/_height));
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