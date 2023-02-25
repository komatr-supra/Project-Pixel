/*
This is script for generating random platform level.
Generating rects with tile data (ChunkData contains TileLevelSO and RectInt).
So i dont need check every grid position. 
Props and also enemy is from TileLevelSO and callculated for each chunk.
Just between chunks -> can be handled transition(between tiles), slopes.
At the end are chunks set to the right tilemap.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectPixel.Generator
{
    public class LevelGenerator : MonoBehaviour
    {
        #region Struct of terrain data
        private struct ChunkData
        {
            public TileLevelSO TileSO;
            public RectInt rect;
        }
        #endregion
        #region Variables   
        [Header("Platforms")]        
        [Tooltip("Lenght of starting platform, player will start in the middle.")]
        [Range(10, 50)]
        [SerializeField] private int startPlatformLenght = 20;
        [Range(0, 100)]
        [Tooltip("Minimal tile height")]
        [SerializeField] private int minTileHeight = 2;
        [Range(0, 100)]
        [Tooltip("Maximal tile height")]
        [SerializeField] private int maxTileHeight = 8;
        [Range(0, 100)]
        [Tooltip("Maximal height change between platforms")]
        [SerializeField] private int maxTileHeightChange = 2;
        [Space]
        [Header("Map")]
        [Tooltip("X is horizontal size of the map, Y is the vertical size of map")]
        [SerializeField] private Vector2Int mapSize;
        [Tooltip("Tile setup Scriptable Object for this map.")]
        [SerializeField] private TileLevelSO[] tileSO;
        [Space]
        [Header("System - SETUP!")]
        [SerializeField] GameObject propsGO;
        [SerializeField] private Tilemap collisionTilemap;
        [SerializeField] private Tilemap backgroundTilemap;
        private int mapLenght = 0;
        //first terrain chunk must be walkable
        private bool mustBeWalkable = true;
        #endregion
        private void Awake()
        {
            //generate main data
            List<ChunkData> mapChunks = GenerateMapFrontChunksData();
            //update background data(here is just adjust height -> dont overlap main terrain)
            UpdateBackgroundData(mapChunks);
            //generate platforms

            //generate visual props
            GenerateProps(mapChunks);
            //update array to tilemap
            UpdateTilesToTilemap(mapChunks);            
        }

        private void GenerateProps(List<ChunkData> mapChunks)
        {
            //go through chunks
            foreach (var chunk in mapChunks)
            {
                float distance = 0;
                //props willbe added to the walkable terrain
                if(chunk.TileSO.isWalkable)
                {                    
                    do
                    {
                        //random sprite
                        Sprite propsSprite = chunk.TileSO.propsSprites[Random.Range(0, chunk.TileSO.propsSprites.Length)];
                        //get half width of sprite                        
                        float halfSpriteWidth = propsSprite.rect.width / propsSprite.pixelsPerUnit / 2;
                        //get random X position
                        distance += Random.Range(halfSpriteWidth, halfSpriteWidth * 4);
                        //this position is too long -> return
                        if(distance > chunk.rect.width - halfSpriteWidth)continue;   
                        //get position for props                     
                        Vector3 position = new Vector3(chunk.rect.xMin + distance, chunk.rect.yMax, 0);
                        //create and set props game object
                        GameObject actualProps = Instantiate(propsGO, position, Quaternion.identity, transform);
                        var v = actualProps.GetComponent<SpriteRenderer>();                        
                        v.sprite = propsSprite;
                        //just a bit variation
                        v.flipX = Random.Range(0f, 1f) > 0.5f ? true : false;
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
                    ? startPlatformLenght 
                    : Random.Range(terrainSettings.minLenght, terrainSettings.maxHeight + 1);
        }
        private void UpdateBackgroundData(List<ChunkData> mapChunks)
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
                        var tilemap = item.TileSO.isWalkable ? collisionTilemap : backgroundTilemap;
                        //Debug.Log("saving " + item.TileSO.tileRule + " to the " + collisionTilemap + " at position " + new Vector3Int(x, y));
                        tilemap.SetTile(new Vector3Int(x, y), item.TileSO.tileRule) ;
                    }
                    
                }
            }
            collisionTilemap.RefreshAllTiles();
        }
        private TileLevelSO GetRandomTerrainSettings()
        {
            
            List<TileLevelSO> chunksToSelect = new();
            //get random terrain
            foreach (var chunk in tileSO)
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

