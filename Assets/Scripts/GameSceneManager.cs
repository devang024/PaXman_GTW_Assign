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

        int seedIndex = 0;
        FloorTile[] _Seeds = new FloorTile[2];
        foreach (var item in TentativeTileList[0].Neighbours)
        {
            if (item.TileTypeGetSet == FloorTile.TileType.Space)
            {
                _Seeds[seedIndex] = item;
                seedIndex++;
                //Debug.Log("Count:" + item.fillThis(FloorTile.TileType.Counting));
            }
        }

        seedIndex = -1;
        if (_Seeds[0].fillThis(FloorTile.TileType.Counting) < _Seeds[1].fillThis(FloorTile.TileType.Counting))
            seedIndex = 0;
        else if (_Seeds[0].fillThis(FloorTile.TileType.Counting) > _Seeds[1].fillThis(FloorTile.TileType.Counting))
            seedIndex = 1;

        resettingGridAfterCounting();

        /*In case, if both area is same, we dont do anything*/
        if (seedIndex!=-1)
        _Seeds[seedIndex].fillThis(FloorTile.TileType.Concrete);
        

        TentativeTileList.Clear();
    }
}
