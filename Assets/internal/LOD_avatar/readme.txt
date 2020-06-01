Pedestrian pack 0.8

-animation:
	categorzied animation controllers, each one animation
	(you can drag an drop them on each pedestrian)
-avatarMeshesLOD:
	pedestrian meshes with four LODs
-materials:
	materials categorized in folders for each pedestrian
-prefabs:
	pedestrian prefabs with animator, LOD group and applied LOD group manager group
-scripts:
	AvatarLodController
		for each pedestrian 
	AvatarLodManager
		manages the active LODs in proportion to camera distance
-test:
	categorzied animations in FBXs 
-textures:
	all textures

-LODwalk.controller [old controller for pedestrians]
-ManagerGroup.prefab [gameobj with attached AvatarLodManager, required in scene if you want to manage the LODs]
-testScene [scene with animation test]