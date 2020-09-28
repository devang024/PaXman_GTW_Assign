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

    

    public const string TILE_TAG = "FloorTile";

    List<FloorTile> TentativeTileList = new List<FloorTile>();

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
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

    public void makeConcreteTiles()
    {
        foreach( var item in TentativeTileList )
        {
            item.TileTypeGetSet = FloorTile.TileType.Concrete;
        }
        TentativeTileList.Clear();
    }
}
