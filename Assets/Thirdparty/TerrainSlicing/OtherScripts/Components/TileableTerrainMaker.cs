//Terrain Slicing & Dynamic Loading Kit copyright © 2012 Kyle Gillen. All rights reserved. Redistribution is not allowed.
namespace TerrainSlicingKit
{
    using UnityEngine;
    using PlayModeUtilities;

    [AddComponentMenu("Terrain Slicing Kit/Tileable Terrain Maker")]
    public class TileableTerrainMaker : MonoBehaviour
    {
        [System.NonSerialized]
        public AlphamapBackupController backupAlphaMap = null;


        public int effectedRegionWidth = 1;
        public int columns = 1;
        public int rows = 1;

        public Terrain[] terrains = new Terrain[1];

        public bool showFoldout = true;
        public bool onlyTileInner = false;
        public bool onlyTileOuter = false;
        public bool emptyLocations = false;
        public bool smoothEdges = true;

        //This script cannot be used in game, so we'll just destroy it if 
        //it's still attached when the game starts.
        //It will only be destroyed while the game is running; once in editor mode it will reappear.
        private void Awake()
        {
            Destroy(this);
        }
    }
}