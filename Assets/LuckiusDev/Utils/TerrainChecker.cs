using UnityEngine;

namespace LuckiusDev.Utils
{
    public static class TerrainChecker
    {
        private static float[] GetTextureMix(Vector3 position, Terrain terrain) {
            Vector3 terrainPosition = terrain.transform.position;
            TerrainData terrainData = terrain.terrainData;
            int mapX = (int) ( ( position.x - terrainPosition.x ) / terrainData.size.x * terrainData.alphamapWidth );
            int mapZ = (int) ( ( position.z - terrainPosition.z ) / terrainData.size.z * terrainData.alphamapHeight );
            float[,,] splatMapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

            float[] cellMix = new float[splatMapData.GetUpperBound(2) + 1];
            for (int i = 0; i < cellMix.Length; i++) {
                cellMix[i] = splatMapData[0, 0, i];
            }
            return cellMix;
        }

        public static string GetLayerName( Vector3 position, Terrain terrain ) {
            float[] cellMix = GetTextureMix(position, terrain);
            
            float strongest = 0f;
            int maxIndex = 0;
            for (int i = 0; i < cellMix.Length; i++) {
                if (cellMix[i] > strongest) {
                    strongest = cellMix[i];
                    maxIndex = i;
                }
            }

            return terrain.terrainData.terrainLayers[maxIndex].name;
        }
    }
}
