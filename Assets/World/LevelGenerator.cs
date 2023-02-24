using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generator
{
    public class LevelGenerator : MonoBehaviour
    {
        #region Struct of terrain data
        //maybe can be switch to scriptable object
        [System.Serializable]
        private struct TerrainSettings
        {               
            public TileBase tileOfChunk;
            public bool isWalkable;
            public int minSize;
            public int maxSize;
        }
        private struct ChunkData
        {
            public TerrainSettings terrainSettings;
            public Vector2Int size;
        }
        #endregion
        #region Variables
        [SerializeField] private Spawner spawner;        
        [SerializeField] private Tilemap collisionTilemap;
        [SerializeField] private Tilemap backgroundTilemap;
        [SerializeField] private int startChunkLenght = 20;
        [SerializeField] private int minTileHeight = 5;
        [SerializeField] private int maxTileHeight = 15;
        [SerializeField] private int maxTileHeightChange = 2;
        [SerializeField] private TerrainSettings[] chunksForThisMap;
        private int heightOfMap = 30;
        [SerializeField] private Vector2Int mapSize;
        private int mapLenght = 0;
        //first chunk of terrain must be walkable
        private bool mustBeWalkable = true;
        #endregion
        private void Awake()
        {
            Debug.Log("awake creating");
            List<ChunkData> mapChunks = GenerateMapFrontChunksData();
            //generate background tiles
            GenerateBackgroundTiles(mapChunks);
            
            
            
            
            //generate platforms

            //generate props
            GenerateProps();
            //update array to tilemap
            UpdateTilesToTilemap(mapChunks);
            
        }

        private void GenerateProps()
        {
            
        }

        private List<ChunkData> GenerateMapFrontChunksData()
        {
            Debug.Log("awake creating");
            List<ChunkData> returnChunkData = new();
            int previousHeight = 0;
            while (mapLenght < mapSize.x)
            {
                ChunkData chunk = new();            
                TerrainSettings terrainSettings = GetRandomTerrainSettings();
                int chunkWidth = GetRandomWidth(terrainSettings);
                chunkWidth = Mathf.Min(mapSize.x - chunkWidth, chunkWidth);
                int chunkHeight = GetRandomHeight(previousHeight);
                chunk.size = new Vector2Int(chunkWidth, chunkHeight);
                chunk.terrainSettings = terrainSettings;
                returnChunkData.Add(chunk);
                Debug.Log(chunk.size);
                mapLenght += chunkWidth;
                previousHeight = chunkHeight;
            }

            return returnChunkData;
        }
        private int GetRandomWidth(TerrainSettings terrainSettings)
        {
            return mapLenght == 0 
                    ? startChunkLenght 
                    : Random.Range(terrainSettings.minSize, terrainSettings.maxSize + 1);
        }
        private void GenerateBackgroundTiles(List<ChunkData> mapChunks)
        {
            //go through map pick walkable terrain
            //if next or previous chunkdata is not walkable and bigger than this walkable terrain, 
            //set it to its height
            for (int i = 0; i < mapChunks.Count; i++)
            {
                if(mapChunks[i].terrainSettings.isWalkable)
                {
                    //previous chunk
                    int previousIndex = i - 1;
                    if(previousIndex >= 0 && !mapChunks[previousIndex].terrainSettings.isWalkable)
                    {
                        ChunkData chunkData = mapChunks[previousIndex];
                        chunkData.size.y = Mathf.Min(mapChunks[previousIndex].size.y, mapChunks[i].size.y);
                        mapChunks[previousIndex] = chunkData;
                    }
                    //next chunk
                    int nextIndex = i + 1;
                    if(nextIndex < mapChunks.Count && !mapChunks[nextIndex].terrainSettings.isWalkable)
                    {
                        ChunkData chunkData = mapChunks[nextIndex];
                        chunkData.size.y = Mathf.Min(mapChunks[nextIndex].size.y, mapChunks[i].size.y);
                        mapChunks[nextIndex] = chunkData;
                    }
                }
            }

        }
        private void UpdateTilesToTilemap(List<ChunkData> mapChunks)
        {
            int lenght = 0;
            //all chunk data to tileset
            foreach (var item in mapChunks)
            {
                int xSize = item.size.x;
                int ySize = item.size.y;

                for (int x = 0; x < xSize; x++)
                {
                    for (int y = 0; y < ySize; y++)
                    {
                        Debug.Log("saving " + item.terrainSettings.tileOfChunk + " to the " + collisionTilemap + " at position " + new Vector3Int(lenght, y));
                        collisionTilemap.SetTile(new Vector3Int(lenght, y), item.terrainSettings.tileOfChunk) ;
                    }
                    lenght++;
                }
            }
            collisionTilemap.RefreshAllTiles();
        }
        private TerrainSettings GetRandomTerrainSettings()
        {
            
            List<TerrainSettings> chunksToSelect = new();
            //get random terrain
            foreach (var chunk in chunksForThisMap)
            {
                //mustBeWalkable is for starting purpose and for gap...etc...
                if(!mustBeWalkable) //not needet to be walkable, get all terrains
                {
                    chunksToSelect.Add(chunk);
                    continue;
                }
                //previous part is not walkable and this part must be
                if(chunk.isWalkable)
                {
                    chunksToSelect.Add(chunk);
                }
            }
            //choose random terrain
            int randomChunkIndex = Random.Range(0, chunksToSelect.Count);
            var v = chunksToSelect[randomChunkIndex];
            //must be next terrain walkable?
            mustBeWalkable = !v.isWalkable;
            return v;
        }
        private int GetRandomHeight(int previousHeight)
        {
            //randomize new height
            //if gap, then use previous
            int newRndHeight = Random.Range(previousHeight - maxTileHeightChange, 
                            previousHeight + maxTileHeightChange + 1);
            newRndHeight = Mathf.Clamp(newRndHeight, minTileHeight, heightOfMap - 1);
            
            return newRndHeight;
        }
             
    }
}

