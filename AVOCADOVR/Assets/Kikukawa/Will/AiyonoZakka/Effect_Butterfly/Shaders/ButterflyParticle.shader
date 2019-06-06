// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-3954-OUT,voffset-2311-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32031,y:32727,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_VertexColor,id:4376,x:31616,y:32733,varname:node_4376,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:3951,x:31635,y:33011,prsc:2,pt:False;n:type:ShaderForge.SFN_Time,id:8874,x:31883,y:33243,varname:node_8874,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8415,x:32061,y:32987,varname:node_8415,prsc:2|A-4376-G,B-3951-OUT,C-9601-OUT,D-3086-OUT;n:type:ShaderForge.SFN_Multiply,id:7092,x:32155,y:33353,varname:node_7092,prsc:2|A-8874-T,B-3215-OUT,C-3026-OUT;n:type:ShaderForge.SFN_Multiply,id:3954,x:32309,y:32760,varname:node_3954,prsc:2|A-7241-RGB,B-4376-RGB,C-5292-OUT;n:type:ShaderForge.SFN_Multiply,id:2311,x:32398,y:33111,varname:node_2311,prsc:2|A-8415-OUT,B-5231-OUT;n:type:ShaderForge.SFN_Sin,id:5231,x:32332,y:33353,varname:node_5231,prsc:2|IN-7092-OUT;n:type:ShaderForge.SFN_Vector1,id:5292,x:32192,y:32883,varname:node_5292,prsc:2,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:9601,x:31801,y:33084,ptovrint:False,ptlb:Flapping,ptin:_Flapping,varname:node_9601,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:3026,x:31902,y:33698,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_3026,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:6520,x:31738,y:33427,varname:node_6520,prsc:2|A-8152-Y,B-9099-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:8152,x:31526,y:33388,varname:node_8152,prsc:2;n:type:ShaderForge.SFN_Vector1,id:7213,x:31806,y:33578,varname:node_7213,prsc:2,v1:10;n:type:ShaderForge.SFN_Add,id:3215,x:31902,y:33427,varname:node_3215,prsc:2|A-6520-OUT,B-7213-OUT;n:type:ShaderForge.SFN_Vector1,id:3086,x:31801,y:33146,varname:node_3086,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Vector1,id:9099,x:31526,y:33519,varname:node_9099,prsc:2,v1:0.1;proporder:7241-9601-3026;pass:END;sub:END;*/

Shader "Aiyonozakka/ButterflyParticle" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _Flapping ("Flapping", Float ) = 1
        _Speed ("Speed", Float ) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float _Flapping;
            uniform float _Speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_8874 = _Time;
                v.vertex.xyz += ((o.vertexColor.g*v.normal*_Flapping*0.1)*sin((node_8874.g*((mul(unity_ObjectToWorld, v.vertex).g*0.1)+10.0)*_Speed)));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 emissive = (_Color.rgb*i.vertexColor.rgb*2.0);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
