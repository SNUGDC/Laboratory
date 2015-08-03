Shader "Custom/billboardPlane" {
	Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        pass{
        Cull Off
        //ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        // color of light source (from "Lighting.cginc")
 
        // User-specified properties
        sampler2D _MainTex;
        struct vertexInput {
        	float4 vertex : POSITION;
        	float4 texcoord : TEXCOORD0;
        };
        struct vertexOutput {
        	float4 pos : SV_POSITION;
        	float4 tex : TEXCOORD1;
        };
        vertexOutput vert(vertexInput v)
        {
            vertexOutput o;
            float4 ori=mul(UNITY_MATRIX_MV,float4(0,0,0,1));
            float4 vt=v.vertex;
            //vt.y=vt.z;
            float2 r1=float2(_Object2World[0][0],_Object2World[0][2]);
            float2 r2=float2(_Object2World[2][0],_Object2World[2][2]);
            float2 vt0=vt.x*r1;
            vt0+=vt.z*r2;
	        vt.xy=-vt0;
            vt.z=0;
            vt.xyz+=ori.xyz;//result is vt.z==ori.z ,so the distance to camera keeped ,and screen size keeped
            o.pos=mul(UNITY_MATRIX_P,vt);
            o.tex=v.texcoord;
            return o;
        }
        float4 frag(vertexOutput input):COLOR
        {
            return tex2D(_MainTex, float2(input.tex.xy));

        }
        ENDCG
        }//endpass
    }
}
