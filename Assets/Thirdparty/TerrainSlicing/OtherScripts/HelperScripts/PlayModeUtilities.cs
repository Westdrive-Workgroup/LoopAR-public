namespace TerrainSlicingKit
{
	namespace PlayModeUtilities
	{
		using UnityEngine;
		
		//Implemented here to avoid 
		public class AlphamapBackupController
		{
			private float[,][,,] backupAlphaMap;
			private TerrainData[,] terrainData;
			private int rows, columns;
			
			public AlphamapBackupController(TerrainData[,] terrainToBackup)
			{
				rows = terrainToBackup.GetLength(0);
				columns = terrainToBackup.GetLength(1);
				terrainData = terrainToBackup;
				BackupAlphaMaps();
			}
			
			private void BackupAlphaMaps()
			{				
				int width = terrainData[0, 0].alphamapWidth;
				int height = terrainData[0, 0].alphamapHeight;
				int layers = terrainData[0, 0].alphamapLayers;
				
				backupAlphaMap = new float[rows, columns][,,];
				
				for(int row = 0; row < rows; row++)
				{
					for(int col = 0; col < columns; col++)
					{
						float[,,] alphaMap = terrainData[row, col].GetAlphamaps(0, 0, width, height);
						float[,,] temp = new float[height, width, layers];
						for(int innerRow = 0; innerRow < height; innerRow++)
						{
							for(int innerCol = 0; innerCol < width; innerCol++)
							{
								for(int i = 0; i < layers; i++)
									temp[innerRow, innerCol, i] = alphaMap[innerRow, innerCol, i];
							}
						}
						backupAlphaMap[row, col] = temp;
					}
				}
			}
			
			public void RestoreAlphaMaps()
			{
				if(backupAlphaMap == null)
				{
					#if UNITY_EDITOR
					UnityEditor.EditorUtility.DisplayDialog("Error", "No backup alphampa exists!", "OK");
					#endif
					return;
				}
					
				for(int row = 0; row < rows; row++)
				{
					for(int col = 0; col < columns; col++)
						terrainData[row, col].SetAlphamaps(0, 0, backupAlphaMap[row,col]);
				}
				
				this.backupAlphaMap = null;
				this.terrainData = null;
			}
		}
	}
}
