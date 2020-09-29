using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public enum GameState
    {
        Initializing,
        GameStarted,
        GamePaused,
        GameFinished
    }

    public GameState gameState = GameState.Initializing;

    public FloorMaker floorMaker;

    public const string TILE_TAG = "FloorTile";

    List<FloorTile> TentativeTileList = new List<FloorTile>();

    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addToTentativeList(FloorTile _tileScript)
    {
        if (_tileScript != null)
        {
            TentativeTileList.Add(_tileScript);
        }
    }

    void resettingGridAfterCounting()
    {
        /**Resetting the counting tileType to default space tileType*/
        foreach (var item in floorMaker.Grid)
        {
            if (item.Value.TileTypeGetSet == FloorTile.TileType.Counting)
            {
                item.Value.TileTypeGetSet = FloorTile.TileType.Space;
            }
        }
    }

    public void makeConcreteTiles()
    {
        foreach( var item in TentativeTileList )
        {
            item.TileTypeGetSet = FloorTile.TileType.Concrete;
        }


        
        FloorTile _seed=null;
        if (TentativeTileList.Count > 0)
        {
            _seed = getSeed(0);
            if (_seed == null)
            {
                resettingGridAfterCounting();
                _seed = getSeed(TentativeTileList.Count - 1);
            }
        }

        resettingGridAfterCounting();

        /*In case, if both area is same, we dont do anything*/
        if (_seed != null)
        {
            _seed.fillThis(FloorTile.TileType.Concrete);
        }
        

        TentativeTileList.Clear();
    }

    FloorTile getSeed(int index)
    {
        int lastTileCounter = floorMaker.Rows * floorMaker.Columns;
        FloorTile _seed=null;
        int seedNeighbours = 0;
        foreach (var item in TentativeTileList[index].Neighbours)
        {
            if (item.TileTypeGetSet == FloorTile.TileType.Space)
            {
                seedNeighbours++;
                int currentTileCounter = item.fillThis(FloorTile.TileType.Counting);
                if (lastTileCounter > currentTileCounter)
                {
                    _seed = item;
                    lastTileCounter = currentTileCounter;
                }
                //Debug.Log("Count:" + item.fillThis(FloorTile.TileType.Counting));
            }
        }
        if (seedNeighbours <= 1)
        {
            _seed = null;
        }
        return _seed;
    }
}
