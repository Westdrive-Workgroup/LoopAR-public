// EasyRoads3D v3 Shader
// creates hole in terrain

Shader "EasyRoads3D/Misc/Terrain Mask" {
	SubShader{
		Tags{"Queue" = "Geometry+10"}
		ColorMask 0
		ZWrite On
		Pass{}
	}
}
