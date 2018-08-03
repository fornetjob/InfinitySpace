Shader "Noise/CalculateCell"
{
	Properties
	{
		Seed("Seed", Int) = 0
		Width("Width", Range(1,1000)) = 100
		PosX("PosX", Int) = 0
		PosY("PosY", Int) = 0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Assets/Game/Shaders/Tools.cginc"

			int Width;
			int Seed;
			int PosX;
			int PosY;

			fixed4 frag(v2f_img i) : COLOR
			{
				int id = abs(floor(i.pos.x) + floor(i.pos.y) * Width);

				int rating = CalcRating(Seed, id, PosX, PosY);

				return fixed4(rating % 256 / 256.0, floor(rating / 256) / 256.0, 0, 1);
			}
			ENDCG
		}
	}
}