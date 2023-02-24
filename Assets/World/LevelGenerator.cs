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
        private struct ChunkSettings
        {               
            public TileBase tileOfChunk;
            public bool isWalkable;
            public int minSize;
            public int maxSize;
        }
        #endregion
        #region Variables
        [SerializeField] private Spawner spawner;        
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private int requestedMapLenght = 100;
        [SerializeField] private int startChunkLenght = 20;
        [SerializeField] private int minTileHeight = 5;
        [SerializeField] private int maxTileHeight = 15;
        [SerializeField] private int maxTileHeightChange = 2;
        [SerializeField] private ChunkSettings[] chunksForThisMap;
        [SerializeField] private ChunkSettings backTile;
        private int heightOfMap = 30;
        private ChunkSettings[,] mapArray;
        private int mapLenght = 0;
        //first chunk of terrain must be walkable
        private bool mustBeWalkable = true;
        #endregion
        private void Awake()
        {
            mapArray = new ChunkSettings[requestedMapLenght, heightOfMap];
            while (mapLenght <= requestedMapLenght)
            {
                //select starting tile, must be walkable
                ChunkSettings chunk = GetRandomChunk();
                Debug.Log("new chunk is " + chunk.tileOfChunk);
                GenerateChunkOfMap(chunk);
            }
            //TODO
            //generate props
            
            UpdateTilesToTilemap();
        }

        private void UpdateTilesToTilemap()
        {
            //lenght of mapArray in both dimension (X and Y)
            int xSize = mapArray.GetLength(0);
            int ySize = mapArray.GetLength(1);

            //set all tiles with data from mapArray            
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    var v = mapArray[x, y].tileOfChunk;
                    if(v == null) continue;
                    tilemap.SetTile(new Vector3Int(x,y,0), v);
                }
            }

            tilemap.RefreshAllTiles();

            int xPos = startChunkLenght / 2;
            int yPos  = 0;
            for (int i = 0; i < heightOfMap; i++)
            {
                if(mapArray[xPos, i].tileOfChunk == null)
                {
                    yPos = i;
                    break;
                }
            }
            Vector3 startPos = new Vector3(xPos, yPos, 0);
            spawner.SpawnPlayer(startPos);
        }

        private ChunkSettings GetRandomChunk()
        {
            
            List<ChunkSettings> chunksToSelect = new();
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

        private void GenerateChunkOfMap(ChunkSettings chunk)
        {
            //size of actual chunk
            int size = mapLenght == 0 ? startChunkLenght : Random.Range(chunk.minSize, chunk.maxSize + 1);

            //height of previous chunk
            int previousHeight = GetPreviousChunkHeight();
            //randomize new height
            int newRndHeight = Random.Range(previousHeight - maxTileHeightChange, previousHeight + maxTileHeightChange + 1);
            newRndHeight = Mathf.Clamp(newRndHeight, minTileHeight, heightOfMap - 1);
            Debug.Log("new height of chunk is " + newRndHeight);
            //create chunk of map
            for (int xx = 0; xx < size; xx++) //lenght
            {
                //real X position of generated tile
                int xPos = mapLenght + xx;
                if (xPos >= requestedMapLenght)
                {
                    Debug.Log("x size of array is larger than map");
                    break;
                }
                
                var tile = chunk.tileOfChunk == null ? backTile : chunk;
                int height = chunk.tileOfChunk == null ? previousHeight : newRndHeight; 
                //TODO
                //if previous chunk was null and new height is bigger than previous height, then clear previous line of background tile
                for (int yy = 0; yy < height; yy++) //fill all height with input data (from GetRandomChunk() method)
                {
                    mapArray[xPos, yy] = tile;
                    Debug.Log("chunk added to position " + xPos + ", " + yy + " is: " + mapArray[xPos, yy].tileOfChunk);
                }                
            }
            //add actual chunk size to the current map lenght
            mapLenght += size;
        }

        private int GetPreviousChunkHeight()
        {
            //no previous position -> take random
            if(mapLenght == 0) return Random.Range(minTileHeight, maxTileHeight);
            //go through y
            for (int yPos = 0; yPos < heightOfMap; yPos++)
            {
                //get 1st empty cell
                var v = mapArray[mapLenght - 1, yPos].tileOfChunk;
                if(v == null)
                {
                    return yPos;
                }
            }
            //just safety check -> this newer happend
            Debug.LogError("previous height of map cannot be found!");
            return Random.Range(minTileHeight, maxTileHeight);
        }        
    }
}

