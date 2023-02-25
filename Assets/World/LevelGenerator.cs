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
        
        
        private struct ChunkData
        {
            public TileLevelSO TileSO;
            public RectInt rect;
        }
        #endregion
        #region Variables
        [SerializeField] private Spawner spawner;        
        [SerializeField] private Tilemap collisionTilemap;
        [SerializeField] private Tilemap backgroundTilemap;
        [SerializeField] private int startChunkLenght = 20;
        [SerializeField] private int minTileHeight = 2;
        [SerializeField] private int maxTileHeight = 8;
        [SerializeField] private int maxTileHeightChange = 2;
        [SerializeField] private TileLevelSO[] chunksForThisMap;
        [SerializeField] private Vector2Int mapSize;
        [SerializeField] GameObject propsGO;
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
            GenerateProps(mapChunks);
            //update array to tilemap
            UpdateTilesToTilemap(mapChunks);
            
        }

        private void GenerateProps(List<ChunkData> mapChunks)
        {
            foreach (var chunk in mapChunks)
            {
                float distance = 0;
                if(chunk.TileSO.isWalkable)
                {
                    do
                    {
                        Sprite propsSprite = chunk.TileSO.propsSprites[Random.Range(0, chunk.TileSO.propsSprites.Length)];                        
                        float halfSpriteWidth = propsSprite.rect.width / propsSprite.pixelsPerUnit / 2;
                        distance += Random.Range(halfSpriteWidth, halfSpriteWidth * 4);
                        if(distance > chunk.rect.width - halfSpriteWidth)
                        {
                            continue;
                        }
                        Vector3 position = new Vector3(chunk.rect.xMin + distance, chunk.rect.yMax, 0);
                        GameObject actualProps = Instantiate(propsGO, position, Quaternion.identity);
                        actualProps.GetComponent<SpriteRenderer>().sprite = propsSprite;
                    } while (distance < chunk.rect.width);

                }
            }
        }

        private List<ChunkData> GenerateMapFrontChunksData()
        {
            //Debug.Log("awake creating");
            List<ChunkData> returnChunkData = new();
            int previousHeight = 0;
            while (mapLenght < mapSize.x)
            {
                ChunkData chunk = new();            
                TileLevelSO terrainSettings = GetRandomTerrainSettings();
                int chunkWidth = GetRandomWidth(terrainSettings);
                chunkWidth = Mathf.Min(mapSize.x - chunkWidth, chunkWidth);
                int chunkHeight = GetRandomHeight(previousHeight);
                chunk.rect = new RectInt(mapLenght, 0, chunkWidth, chunkHeight);
                chunk.TileSO = terrainSettings;
                returnChunkData.Add(chunk);
                //Debug.Log(chunk.rect);
                mapLenght += chunkWidth;
                previousHeight = chunkHeight;
            }

            return returnChunkData;
        }
        private int GetRandomWidth(TileLevelSO terrainSettings)
        {
            return mapLenght == 0 
                    ? startChunkLenght 
                    : Random.Range(terrainSettings.minLenght, terrainSettings.maxHeight + 1);
        }
        private void GenerateBackgroundTiles(List<ChunkData> mapChunks)
        {
            //go through map pick walkable terrain
            //if next or previous chunkdata is not walkable and bigger than this walkable terrain, 
            //set it to its height
            for (int i = 0; i < mapChunks.Count; i++)
            {
                if(!mapChunks[i].TileSO.isWalkable)
                {
                    //previous chunk
                    int previousIndex = i - 1;
                    if(previousIndex >= 0 && mapChunks[previousIndex].TileSO.isWalkable)
                    {
                        ChunkData chunkData = mapChunks[i];
                        //Debug.Log("chunk data from list " + chunkData.rect);
                        //Debug.Log(mapChunks[i].rect.y + " old new y");
                        chunkData.rect.height = Mathf.Min(mapChunks[previousIndex].rect.height, mapChunks[i].rect.height);
                        mapChunks[i] = chunkData;
                        //Debug.Log(mapChunks[i].rect.y + " actual new y, previous chunk Y is " + mapChunks[previousIndex].rect.y);
                    }
                    //next chunk
                    int nextIndex = i + 1;
                    if(nextIndex < mapChunks.Count && mapChunks[nextIndex].TileSO.isWalkable)
                    {
                        ChunkData chunkData = mapChunks[i];
                        //Debug.Log(mapChunks[i].rect.y + "old new y");
                        chunkData.rect.height = Mathf.Min(mapChunks[nextIndex].rect.height, mapChunks[i].rect.height);
                        mapChunks[i] = chunkData;
                        //Debug.Log(mapChunks[i].rect.y + "actual new y, next chuhnk Y is "+ mapChunks[nextIndex].rect.y);
                    }
                }
            }

        }
        private void UpdateTilesToTilemap(List<ChunkData> mapChunks)
        {            
            //all chunk data to tileset
            foreach (var item in mapChunks)
            {
                

                for (int x = item.rect.xMin; x < item.rect.xMax; x++)
                {
                    for (int y = item.rect.yMin; y < item.rect.yMax; y++)
                    {
                        //Debug.Log("saving " + item.TileSO.tileRule + " to the " + collisionTilemap + " at position " + new Vector3Int(x, y));
                        collisionTilemap.SetTile(new Vector3Int(x, y), item.TileSO.tileRule) ;
                    }
                    
                }
            }
            collisionTilemap.RefreshAllTiles();
        }
        private TileLevelSO GetRandomTerrainSettings()
        {
            
            List<TileLevelSO> chunksToSelect = new();
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
            newRndHeight = Mathf.Clamp(newRndHeight, minTileHeight, maxTileHeight);
            
            return newRndHeight;
        }
             
    }
}

