Shader "SwordMaster/Rim Bumped Specular"
{
	Properties 
	{
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Main Tex", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Specular ("Specular Color", Color) = (1, 1, 1, 1)
		_Gloss ("Gloss", Range(8.0, 256)) = 20
        _RimColor("RimColor", Color) = (0,1,0,1)  
        _RimPower("RimPower", Range(0.0, 1.0)) = 0.5 
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}

		Pass 
		{
		 
			Tags { "LightMode"="ForwardBase" }
		
			CGPROGRAM
			
			#pragma multi_compile_fwdbase	
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			fixed4 _Specular;
			float _Gloss;

			fixed4 _RimColor;  
            float _RimPower; 
			
			struct a2v 
			{
				fixed4 vertex : POSITION;
				fixed3 normal : NORMAL;
				fixed4 tangent : TANGENT;
				fixed4 texcoord : TEXCOORD0;
			};
			
			struct v2f 
			{
				fixed4 pos : SV_POSITION;

				fixed4 worldPos :TEXCOORD0;

				fixed4 uv : TEXCOORD1;

				fixed3 tangentSpaceNormal :TEXCOORD2;

				fixed3 tangentSpaceLightDirection:TEXCOORD3;

				fixed3 tangentSpaceViewDirection:TEXCOORD4;

				SHADOW_COORDS(5)
			};
			
			v2f vert(a2v v) 
			{
			 	v2f o;

			 	o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld,v.vertex);

			 	o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
			 	o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;

				fixed3 normalizedVertexNormal = normalize(v.normal);

				fixed3 normalizedVertexTangent = normalize(v.tangent.xyz);

				fixed3 normalizedVertexBitangent = normalize(cross(normalizedVertexNormal,normalizedVertexTangent) * v.tangent.w);

				float3x3 objectToTangentMatrix = float3x3(normalizedVertexTangent.xyz,normalizedVertexBitangent,normalizedVertexNormal);

				o.tangentSpaceNormal = mul(objectToTangentMatrix,v.normal.xyz).xyz;

				o.tangentSpaceLightDirection = mul(objectToTangentMatrix,ObjSpaceLightDir(v.vertex)).xyz;

				o.tangentSpaceViewDirection = mul(objectToTangentMatrix,ObjSpaceViewDir(v.vertex)).xyz;
  				
  				TRANSFER_SHADOW(o);
			 	
			 	return o;
			}
			
			fixed4 frag(v2f i) : SV_Target 
			{
				fixed3 albedo = tex2D(_MainTex,i.uv.xy).rgb * _Color.rgb;

				fixed3 ambientColor = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

				fixed4 normalTextureTexel = tex2D(_BumpMap,i.uv.zw);
				
				fixed3 tangentSpaceNormalDirection = UnpackNormal(normalTextureTexel);
				
				fixed3 tangentSpaceLightPosition = normalize(i.tangentSpaceLightDirection);

				fixed3 diffuseColor = _LightColor0.rgb * albedo * saturate(dot(tangentSpaceNormalDirection,tangentSpaceLightPosition));

				fixed3 tangentSpaceViewDirection = normalize(i.tangentSpaceViewDirection);

				fixed3 tangentSpaceHalfDirection = normalize(tangentSpaceLightPosition + tangentSpaceViewDirection);

				fixed3 specularColor = _LightColor0.rgb * _Specular.rgb*pow(saturate(dot(tangentSpaceNormalDirection,tangentSpaceHalfDirection)),_Gloss);

				fixed3 worldPos = i.worldPos;

				UNITY_LIGHT_ATTENUATION(atten, i, worldPos);
 
 				fixed3 tangentSpaceNormal = normalize(i.tangentSpaceNormal);

                float rim = 1 - max(0, dot(tangentSpaceViewDirection, tangentSpaceNormal));  

				fixed4 rimColor;

				if(_RimPower != 0.0)
				{
					rimColor = _RimColor * pow(rim, 1/_RimPower); 
				}
				else
				{
					rimColor = fixed4(0.0,0.0,0.0,0.0);
				}
               

				fixed3 color = ambientColor + (diffuseColor + specularColor)*atten + rimColor.rgb;

				fixed4 resultColor = fixed4(color,1);

				return resultColor;
			}
			
			ENDCG
		}
		
		Pass
		{

			Tags { "LightMode" = "ForwardAdd" }

			Blend One One

			CGPROGRAM

			#pragma multi_compile_fwdadd_fullshadows	

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			fixed4 _Specular;
			float _Gloss;

			fixed4 _RimColor;
			float _RimPower;

			struct a2v
			{
				fixed4 vertex : POSITION;
				fixed3 normal : NORMAL;
				fixed4 tangent : TANGENT;
				fixed4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				fixed4 pos : SV_POSITION;

				fixed4 worldPos : TEXCOORD0;

				fixed4 uv : TEXCOORD1;

				fixed3 tangentSpaceNormal : TEXCOORD2;

				fixed3 tangentSpaceLightDirection : TEXCOORD3;

				fixed3 tangentSpaceViewDirection : TEXCOORD4;

				SHADOW_COORDS(5)
			};

			v2f vert(a2v v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld,v.vertex);

				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;

				fixed3 normalizedVertexNormal = normalize(v.normal);

				fixed3 normalizedVertexTangent = normalize(v.tangent.xyz);

				fixed3 normalizedVertexBitangent = normalize(cross(normalizedVertexNormal,normalizedVertexTangent) * v.tangent.w);

				float3x3 objectToTangentMatrix = float3x3(normalizedVertexTangent.xyz,normalizedVertexBitangent,normalizedVertexNormal);

				o.tangentSpaceNormal = mul(objectToTangentMatrix,v.normal.xyz).xyz;

				o.tangentSpaceLightDirection = mul(objectToTangentMatrix,ObjSpaceLightDir(v.vertex)).xyz;

				o.tangentSpaceViewDirection = mul(objectToTangentMatrix,ObjSpaceViewDir(v.vertex)).xyz;

				TRANSFER_SHADOW(o);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed3 albedo = tex2D(_MainTex,i.uv.xy).rgb * _Color.rgb;

				fixed4 normalTextureTexel = tex2D(_BumpMap,i.uv.zw);

				fixed3 tangentSpaceNormalDirection = UnpackNormal(normalTextureTexel);

				fixed3 tangentSpaceLightPosition = normalize(i.tangentSpaceLightDirection);

				fixed3 diffuseColor = _LightColor0.rgb * albedo * saturate(dot(tangentSpaceNormalDirection,tangentSpaceLightPosition));

				fixed3 tangentSpaceViewDirection = normalize(i.tangentSpaceViewDirection);

				fixed3 tangentSpaceHalfDirection = normalize(tangentSpaceLightPosition + tangentSpaceViewDirection);

				fixed3 specularColor = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(tangentSpaceNormalDirection,tangentSpaceHalfDirection)),_Gloss);

				fixed3 worldPos = i.worldPos;

				UNITY_LIGHT_ATTENUATION(atten, i, worldPos);

				fixed3 tangentSpaceNormal = normalize(i.tangentSpaceNormal);

				float rim = 1 - max(0, dot(tangentSpaceViewDirection, tangentSpaceNormal));

				fixed4 rimColor;

				if (_RimPower != 0.0)
				{
					rimColor = _RimColor * pow(rim, 1 / _RimPower);
				}
				else
				{
					rimColor = fixed4(0.0,0.0,0.0,0.0);
				}


				fixed3 color = (diffuseColor + specularColor) * atten + rimColor.rgb;

				fixed4 resultColor = fixed4(color,1);

				return resultColor;
			}

			ENDCG
		}

		Pass
		{
			Tags{"LightMode" = "ShadowCaster"}

			CGPROGRAM

			#pragma vertex vert

			#pragma fragment frag

			#pragma multi_compile_shadowcaster

			#include "UnityCG.cginc"

			struct v2f
			{
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v)
			{
				v2f o;

				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);

				return o;
			}

			float4 frag(v2f i):SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i);
			}

			ENDCG
		}

	} 
	
}