Shader "2D/CRT-like"
 {  
     Properties
     {
        _MainTex ("Sprite Texture", 2D) = "white" {}
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
                 int xInt = floor(x);
                 int yInt = floor(y);
                 float offset = 0.33;
                 float4 c0 = tex2D (_MainTex, float2((xInt+1+offset)/_width, (yInt+offset)/_height));
                 float4 c4 = tex2D (_MainTex, float2((xInt-1+offset)/_width, (yInt+offset)/_height));
                 float4 c = tex2D (_MainTex, float2((xInt+offset)/_width, (yInt+offset)/_height));

				 float4 co = c0 * saturate(1-abs(1.5-frac(x))) + c * saturate(1-abs(0.5-frac(x))) + c4 * saturate(1-abs(-0.5-frac(x)));
				 float brightness = (max(max(co.r,co.g),co.b) + min(min(co.r,co.g),co.b))/2;
				 float yb = abs(frac(y)-0.5)*(2-brightness);
				 o.rgb = co.rgb * (1-yb * yb);
				 o.a = co.a;
                 return o;
             }
 
             ENDCG
         }
     }
 }