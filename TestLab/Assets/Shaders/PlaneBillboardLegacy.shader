Shader "Custom/billboardLegacy" {
	Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _CutRange("Cut Standard", Float) = 0.0
        _Brightness("Brightness", Float) = 1.0
    }
    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        pass{
        Tags { "LightMode" = "ForwardBase" } 
        Cull Off
        //ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        uniform float4 _LightColor0; 
        // color of light source (from "Lighting.cginc")
 
        // User-specified properties
        uniform sampler2D _BumpMap;	
        uniform float4 _BumpMap_ST;
        uniform float _Brightness; 
        uniform float _CutRange;
 
        sampler2D _MainTex;
        
        struct vertexInput {
        	float4 vertex : POSITION;
        	float4 texcoord : TEXCOORD0;
        	float3 normal : NORMAL;
        	float4 tangent : TANGENT;
        };
        struct vertexOutput {
        	float4 pos : SV_POSITION;
        	float4 posWorld : TEXCOORD0;
        	// position of the vertex (and fragment) in world space 
        	float4 tex : TEXCOORD1;
        	float3 tangentWorld : TEXCOORD2;  
        	float3 normalWorld : TEXCOORD3;
            float3 binormalWorld : TEXCOORD4;
//            float3x3 camRotation : TEXCOORD5;
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
            
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // unity_Scale.w is unnecessary 
            o.tangentWorld = normalize(
               mul(modelMatrix, float4(v.tangent.xyz, 0.0)).xyz);
            o.normalWorld = normalize(
               mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
            o.binormalWorld = normalize(
               cross(o.normalWorld, o.tangentWorld) 
               * v.tangent.w); // tangent.w is specific to Unity
 
            o.posWorld = mul(modelMatrix, v.vertex);
 
            o.tex=v.texcoord;
            return o;
        }
        float4 frag(vertexOutput input):COLOR
        {
            float4 encodedNormal = tex2D(_BumpMap, 
               _BumpMap_ST.xy * input.tex.xy + _BumpMap_ST.zw);
            float3 localCoords = float3(2.0 * encodedNormal.a - 1.0, 
                2.0 * encodedNormal.g - 1.0, 0.0);
            localCoords.z = sqrt(1.0 - dot(localCoords, localCoords));
               // approximation without sqrt:  localCoords.z = 
               // 1.0 - 0.5 * dot(localCoords, localCoords);
 
            float3x3 local2WorldTranspose = float3x3(
               input.tangentWorld, 
               input.binormalWorld, 
               input.normalWorld);
            float3 normalDirection = 
               normalize(mul(localCoords, local2WorldTranspose));
 
            float3 viewDirection = normalize(
               _WorldSpaceCameraPos - input.posWorld.xyz);
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(_WorldSpaceLightPos0.xyz);
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = 
                  _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
            
            float3 cam = mul(float4(_WorldSpaceCameraPos,0),_World2Object);
	        cam.y = 0;
	        cam = normalize(cam);

            float3x3 camRotation = float3x3(
				float3(-cam.z,0.0,cam.x),
				float3(0.0, 1.0, 0.0),
				float3(-cam.x, 0.0,-cam.z));
            //lightDirection = mul(camRotation,lightDirection);
 
            float4 ambientLighting = 
               float4(UNITY_LIGHTMODEL_AMBIENT.xyz * tex2D(_MainTex, float2(input.tex.xy)).xyz,tex2D(_MainTex, float2(input.tex.xy)).w);
 
           	float brightness = min(_CutRange, dot(normalDirection, lightDirection));
            brightness += _CutRange;
            brightness = brightness * brightness * brightness * brightness * brightness * brightness;

            float4 diffuseReflection = 
               attenuation * _LightColor0 * tex2D(_MainTex, float2(input.tex.xy))
               * brightness;
 
            return float4(ambientLighting + diffuseReflection);

        }
        ENDCG
        }//endpass
        Pass {      
        Tags { "LightMode" = "ForwardAdd" } 
        // pass for additional light sources
        Blend One One // additive blending 
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        uniform float4 _LightColor0; 
        // color of light source (from "Lighting.cginc")
 
        // User-specified properties
        uniform sampler2D _BumpMap;	
        uniform float4 _BumpMap_ST;
        uniform float _Brightness; 
        uniform float _CutRange;
        uniform float _CutConst = 1.0-_CutRange;
 
        sampler2D _MainTex;
                
        struct vertexInput {
        	float4 vertex : POSITION;
        	float4 texcoord : TEXCOORD0;
        	float3 normal : NORMAL;
        	float4 tangent : TANGENT;
        };
        struct vertexOutput {
        	float4 pos : SV_POSITION;
        	float4 posWorld : TEXCOORD0;
        	// position of the vertex (and fragment) in world space 
        	float4 tex : TEXCOORD1;
        	float3 tangentWorld : TEXCOORD2;  
        	float3 normalWorld : TEXCOORD3;
            float3 binormalWorld : TEXCOORD4;
            float3 camRotation : TEXCOORD5;
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
            
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // unity_Scale.w is unnecessary 
            o.tangentWorld = normalize(
               mul(modelMatrix, float4(v.tangent.xyz, 0.0)).xyz);
            o.normalWorld = normalize(
               mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
            o.binormalWorld = normalize(
               cross(o.normalWorld, o.tangentWorld) 
               * v.tangent.w); // tangent.w is specific to Unity
 
            o.posWorld = mul(modelMatrix, v.vertex);

 
            o.tex=v.texcoord;
            return o;
        }
        float4 frag(vertexOutput input):COLOR
        {
            float4 encodedNormal = tex2D(_BumpMap, 
               _BumpMap_ST.xy * input.tex.xy + _BumpMap_ST.zw);
            float3 localCoords = float3(2.0 * encodedNormal.a - 1.0, 
                2.0 * encodedNormal.g - 1.0, 0.0);
            localCoords.z = sqrt(1.0 - dot(localCoords, localCoords));
               // approximation without sqrt:  localCoords.z = 
               // 1.0 - 0.5 * dot(localCoords, localCoords);
 
            float3x3 local2WorldTranspose = float3x3(
               input.tangentWorld, 
               input.binormalWorld, 
               input.normalWorld);
            float3 normalDirection = 
               normalize(mul(localCoords, local2WorldTranspose));
 
            float3 viewDirection = normalize(
               _WorldSpaceCameraPos - input.posWorld.xyz);
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(_WorldSpaceLightPos0.xyz);
            } 
            else // point or spot light
            {
               float3 vertexToLightSource = 
                  _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
            
            float3 cam = mul(float4(_WorldSpaceCameraPos,0),_World2Object);
	        cam.y = 0;
	        cam = normalize(cam);
	        
            float3x3 camRotation = float3x3(
				float3(-cam.z,0.0,cam.x),
				float3(0.0, 1.0, 0.0),
				float3(-cam.x, 0.0,-cam.z));
            //lightDirection = mul(camRotation,lightDirection);
            
            float brightness = min(_CutRange, dot(normalDirection, lightDirection));
            brightness += _CutRange;
            brightness = brightness * brightness * brightness * brightness * brightness * brightness;
 			
            float4 diffuseReflection = 
               attenuation * _LightColor0 * tex2D(_MainTex, float2(input.tex.xy))
               * brightness;
 
            return diffuseReflection;

        }
        ENDCG
        }//endpass
    }
}
