using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    public GameObject PrefabEnemy;
    public GameObject InstructionPanelUI;
    public GameObject GameOverPanelUI;

    public Text Lives, Progress;
    public const bool LOGGING = false;
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
    public const string ENEMY_TAG = "Enemy";
    public const string LIVES_KEY = "LIVES";

    public const string PAUSE_EVENT = "GamePausedEvent";

    List<FloorTile> TentativeTileList = new List<FloorTile>();
    int lives = 3;

    public GameObject World;
    //public List<EnemyScript> Enemies;
    public GameObject Player;
    public void StartGame()
    {
        //Enemies = new List<EnemyScript>();
        gameState = GameState.GameStarted;
        InstructionPanelUI.SetActive(false);
        Lives.text = "Lives : "+lives.ToString();
        checkGameEnd();
        


        World.BroadcastMessage("StartGame");
    }

    private void Awake()
    {
        lives = PlayerPrefs.GetInt(LIVES_KEY, -1);
        if ( lives < 3 )
        {
            PlayerPrefs.SetInt(LIVES_KEY,3);
            lives = 3;
            Lives.text = "Lives : " + lives;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 60;
        GameObject _obj;
        for (int index = 0; index < 3; index++)
        {
            _obj = Instantiate(PrefabEnemy, World.transform) as GameObject;
            if (_obj != null)
            {
                EnemyScript _enemyScript = _obj.GetComponent<EnemyScript>();
                if (_enemyScript != null)
                {
                    _enemyScript.enemyType = EnemyScript.EnemyType.Slow;
                    float _x = Random.Range(2, floorMaker.Rows - 2);
                    float _y = Random.Range(2, floorMaker.Columns - 2);
                    _obj.transform.position = new Vector3(_x, _y, _obj.transform.position.z);
                    //Enemies.Add(_enemyScript);
                }
            }
        }
        _obj = Instantiate(PrefabEnemy, World.transform) as GameObject;
        if (_obj != null)
        {
            EnemyScript _enemyScript = _obj.GetComponent<EnemyScript>();
            if (_enemyScript != null)
            {
                _enemyScript.enemyType = EnemyScript.EnemyType.Fast;
                _obj.GetComponent<SpriteRenderer>().color = Color.yellow;
                float _x = Random.Range(2, floorMaker.Rows - 2);
                float _y = Random.Range(2, floorMaker.Columns - 2);
                _obj.transform.position = new Vector3(_x, _y, _obj.transform.position.z);
                //Enemies.Add(_enemyScript);
            }
        }
    }



    [Range(0f,4f)]
    public float timeScale = 1.0f;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timeScale;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if ( gameState == GameState.GameStarted )
            {
                gameState = GameState.GamePaused;
                World.BroadcastMessage("GamePausedEvent", false);
            }
            else if ( gameState == GameState.GamePaused )
            {
                gameState = GameState.GameStarted;
                World.BroadcastMessage("GamePausedEvent", true);
            }
        }
    }

    public void addToTentativeList(FloorTile _tileScript)
    {
        if ( _tileScript != null )
        {
            TentativeTileList.Add(_tileScript);
        }
    }

    public void RestartTurn()
    {
        if (gameState == GameState.GameStarted)
        StartCoroutine(RestartCoRoutine());
    }

    IEnumerator RestartCoRoutine()
    {
        gameState = GameState.GamePaused;
        World.BroadcastMessage(PAUSE_EVENT,false);
        yield return new WaitForSeconds(3.0f);

        resettingGridForRestart();
        Player.transform.position = new Vector3(0.5f, 0.5f, Player.transform.position.z);
        Player.transform.rotation = Quaternion.identity;
        lives = lives - 1;
        if ( lives < 0 )
        {
            GameOverPanelUI.SetActive(true);
        }
        else
        {
            Lives.text = "Lives : " + lives;
            PlayerPrefs.SetInt(LIVES_KEY, lives);
            gameState = GameState.GameStarted;
            World.BroadcastMessage(PAUSE_EVENT, true);
        }
        
        


        yield return null;
    }

    void resettingGridForRestart()
    {
        TentativeTileList.Clear();
        /**Resetting the counting tileType to default space tileType*/
        foreach (var item in floorMaker.Grid)
        {
            if (item.Value.TileTypeGetSet == FloorTile.TileType.Tentative)
            {
                item.Value.TileTypeGetSet = FloorTile.TileType.Space;
            }
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

    /* in the following method...
     */
    public void makeConcreteTiles()
    {
        int counter = 0;
        FloorTile _seed = null;
        foreach ( var item in TentativeTileList )
        {
            item.TileTypeGetSet = FloorTile.TileType.Concrete;
            if (_seed == null)
            {
                resettingGridAfterCounting();
                _seed = getSeed(counter);
            }
            counter++;
        }

        resettingGridAfterCounting();

        /*In case, if both area is same, we dont do anything*/
        if (_seed != null)
        {
            _seed.fillThis(FloorTile.TileType.Concrete);
        }
        

        TentativeTileList.Clear();
        checkGameEnd();

    }

    void checkGameEnd()
    {
        int total = floorMaker.Rows * floorMaker.Columns;
        int concreteCounter = 0;
        foreach( var item in floorMaker.Grid )
        {
            if ( item.Value.TileTypeGetSet == FloorTile.TileType.Concrete )
            {
                concreteCounter++;
            }
        }

        float progress = ((float)concreteCounter) / ((float)total);
        Progress.text = "Progress : "+(Mathf.FloorToInt(progress * 100f)+ "/" + "80%");

        if (progress >= 0.8f )
        {
            gameState = GameState.GameFinished;
            Debug.LogError("GameFinished");
        }
        else Debug.Log("Counter:"+concreteCounter);
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
