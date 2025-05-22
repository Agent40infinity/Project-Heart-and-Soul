Shader "Unit/Fx_BillBoard"
{
Properties
{
[Enum(UnityEngine.Rendering.BlendMode)]_Src("Src", Float) = 1
[Enum(UnityEngine.Rendering.BlendMode)]_Dst("Dst", Float) = 1
_MainTex("MainTex", 2D) = "white" {}
}

SubShader
{
	Tags { "RenderType"="Opaque" "Queue"="Transparent" "PreviewType"="Plane" }
LOD 100

	CGINCLUDE
	#pragma target 3.0
	ENDCG
	Blend [_Src] [_Dst], SrcAlpha OneMinusSrcAlpha
	AlphaToMask Off
	Cull Off
	ColorMask RGBA
	ZWrite Off
	ZTest LEqual
	Stencil
	{
		Ref 255
		CompFront Always
		PassFront Keep
		FailFront Keep
		ZFailFront Keep
		CompBack Always
		PassBack Keep
		FailBack Keep
		ZFailBack Keep
	}
	
	
	Pass
	{
		Name "Unlit"
		Tags { "LightMode"="ForwardBase" }
		CGPROGRAM

	
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_instancing
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float4 color : COLOR;
			float3 ase_normal : NORMAL;
			float4 ase_tangent : TANGENT;
			float4 ase_texcoord : TEXCOORD0;
		};
		
		struct v2f
		{
			float4 vertex : SV_POSITION;
			#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
			float3 worldPos : TEXCOORD0;
			#endif
			float4 ase_texcoord1 : TEXCOORD1;
			float4 ase_color : COLOR;
		};

		uniform float _Src;
		uniform float _Dst;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;

		v2f vert ( appdata v )
		{
			v2f o;
			//Calculate new billboard vertex position and normal;
			float3 upCamVec = normalize ( UNITY_MATRIX_V._m10_m11_m12 );
			float3 forwardCamVec = -normalize ( UNITY_MATRIX_V._m20_m21_m22 );
			float3 rightCamVec = normalize( UNITY_MATRIX_V._m00_m01_m02 );
			float4x4 rotationCamMatrix = float4x4( rightCamVec, 0, upCamVec, 0, forwardCamVec, 0, 0, 0, 0, 1 );
			v.ase_normal = normalize( mul( float4( v.ase_normal , 0 ), rotationCamMatrix )).xyz;
			v.ase_tangent.xyz = normalize( mul( float4( v.ase_tangent.xyz , 0 ), rotationCamMatrix )).xyz;
			//This unfortunately must be made to take non-uniform scaling into account;
			//Transform to world coords, apply rotation and transform back to local;
			v.vertex = mul( v.vertex , unity_ObjectToWorld );
			v.vertex = mul( v.vertex , rotationCamMatrix );
			v.vertex = mul( v.vertex , unity_WorldToObject );
			o.ase_texcoord1.xy = v.ase_texcoord.xy;
			o.ase_color = v.color;			
			//setting value to unused interpolator channels and avoid initialization warnings
			o.ase_texcoord1.zw = 0;
			o.vertex = UnityObjectToClipPos(v.vertex);

			#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			#endif
			return o;
		}
		
		fixed4 frag (v2f i ) : SV_Target
		{
			float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 finalColor = tex2D( _MainTex, uv_MainTex);
			return finalColor;
		}
		ENDCG
	}
}
}