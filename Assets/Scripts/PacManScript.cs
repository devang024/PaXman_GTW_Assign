using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManScript : MonoBehaviour
{
    public const float MAX_SWIPE_TIME = 1.25f;

    // Factor of the screen width that we consider a swipe
    // 0.17 works well for portrait mode 16:9 phone
    public const float MIN_SWIPE_DISTANCE = 0.1f;

    public static bool swipedRight = false;
    public static bool swipedLeft = false;
    public static bool swipedUp = false;
    public static bool swipedDown = false;

    public bool debugWithArrowKeys = true;

    Vector2 startPos;
    float startTime;

    [SerializeField]
    GameSceneManager manager;
    public float speed;

    Vector2 _direction = new Vector2();

    Vector2 _tempPositionVector = new Vector2();
    Vector2 XLimits, YLimits;

    bool onTheFloor = true;


    float _rows;// = FindObjectOfType<FloorMaker>().Rows;
    float _columns;// = FindObjectOfType<FloorMaker>().Columns;
    // Start is called before the first frame update
    void Start()
    {
        _rows = FindObjectOfType<FloorMaker>().Rows;
        _columns = FindObjectOfType<FloorMaker>().Columns;
        XLimits = new Vector2(0.5f,_rows-0.5f);
        YLimits = new Vector2(0.5f,_columns-0.5f);
    }

    void StopMe()
    {        
        _direction = Vector2.zero;
        this.transform.position = new Vector2(Mathf.Floor(this.transform.position.x) + 0.5f, Mathf.Floor(this.transform.position.y) + 0.5f);
        
    }

    public void StartGame()
    {
        gameRunning = true;
    }

    void MoveMe( )
    {        
        _tempPositionVector = this.transform.position;
        float xchange = _direction.x * speed * Time.deltaTime;
        if (_tempPositionVector.x + xchange >= XLimits.x && _tempPositionVector.x + xchange <= XLimits.y)
        {
            
            _tempPositionVector.x += xchange;
            //if(_tempPositionVector.x>1)
            //_tempPositionVector.x = Mathf.Ceil(_tempPositionVector.x)+0.5f;
        }
        else
        {
            if (_tempPositionVector.x + xchange < XLimits.x)
            {
                _tempPositionVector.x = 0.5f;                
            }
            else if (_tempPositionVector.x + xchange > XLimits.y)
            {
                _tempPositionVector.x = _rows - 0.5f;                
            }
        }

        float yChange = _direction.y * speed * Time.deltaTime;
        if (_tempPositionVector.y + yChange >= YLimits.x && _tempPositionVector.y + yChange <= YLimits.y)
        {
            _tempPositionVector.y += yChange;
            //if (_tempPositionVector.y > 1)
            //    _tempPositionVector.y = Mathf.Ceil(_tempPositionVector.y) + 0.5f;
        }
        else
        {
            if (_tempPositionVector.y + yChange < YLimits.x)
            {
                _tempPositionVector.y = 0.5f;                
            }
            else if (_tempPositionVector.y + yChange > YLimits.y)
            {
                _tempPositionVector.y = _columns - 0.5f;                
            }
        }
        
        this.transform.position = _tempPositionVector;
    }

    bool gameRunning = false;

    public void GamePausedEvent(bool _value)
    {
        gameRunning = _value;
    }

    void upDirection()
    {
        _direction.x = 0f;
        _direction.y = 1f;
        this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        this.transform.position = new Vector2(manager.floorMaker.Grid[_gIndex].transform.position.x + 0.5f,
            manager.floorMaker.Grid[_gIndex].transform.position.y + 0.5f);
    }

    void downDirection()
    {
        _direction.x = 0f;
        _direction.y = -1f;
        this.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
        this.transform.position = new Vector2(manager.floorMaker.Grid[_gIndex].transform.position.x + 0.5f,
            manager.floorMaker.Grid[_gIndex].transform.position.y + 0.5f);
    }

    void leftDirection()
    {
        _direction.x = -1f;
        _direction.y = 0f;
        this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        this.transform.position = new Vector2(manager.floorMaker.Grid[_gIndex].transform.position.x + 0.5f,
            manager.floorMaker.Grid[_gIndex].transform.position.y + 0.5f);
    }

    void rightDirection()
    {
        _direction.x = 1f;
        _direction.y = 0f;
        this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        this.transform.position = new Vector2(manager.floorMaker.Grid[_gIndex].transform.position.x + 0.5f,
            manager.floorMaker.Grid[_gIndex].transform.position.y + 0.5f);
    }

    int Xindex, Yindex;
    FloorMaker.GridIndex _gIndex;
    private void FixedUpdate()
    {
        if (gameRunning)
        {
            swipedRight = false;
            swipedLeft = false;
            swipedUp = false;
            swipedDown = false;
            Xindex = Mathf.FloorToInt(this.transform.position.x);
            Yindex = Mathf.FloorToInt(this.transform.position.y);
            _gIndex = new FloorMaker.GridIndex(Xindex, Yindex);

#if UNITY_EDITOR || UNITY_STANDALONE

            if (Input.GetKeyDown(KeyCode.UpArrow))swipedUp = true;

            if (Input.GetKeyDown(KeyCode.DownArrow))swipedDown = true;

            if (Input.GetKeyDown(KeyCode.LeftArrow))swipedLeft = true;

            if (Input.GetKeyDown(KeyCode.RightArrow))swipedRight = true;

#elif (UNITY_ANDROID || UNITY_IOS )
{
        if(Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began)
			{
				startPos = new Vector2(t.position.x/(float)Screen.width, t.position.y/(float)Screen.width);
				startTime = Time.time;
			}
			if(t.phase == TouchPhase.Ended)
			{
				if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
					return;

				Vector2 endPos = new Vector2(t.position.x/(float)Screen.width, t.position.y/(float)Screen.width);

				Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

				if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
					return;

				if (Mathf.Abs (swipe.x) > Mathf.Abs (swipe.y)) { // Horizontal swipe
					if (swipe.x > 0) {
						swipedRight = true;
					}
					else {
						swipedLeft = true;
					}
				}
				else { // Vertical swipe
					if (swipe.y > 0) {
						swipedUp = true;
					}
					else {
						swipedDown = true;
					}
				}
			}
		}
   }
        
#endif
            if (swipedRight) if (_direction.x == 0f || onTheFloor) rightDirection();
            if (swipedLeft) if (_direction.x == 0f || onTheFloor) leftDirection();
            if (swipedUp) if (_direction.y == 0f || onTheFloor) upDirection();
            if (swipedDown) if (_direction.y == 0f || onTheFloor) downDirection();

            MoveMe();

            /*Following is tile changing logic block*/
            {
                if (manager.floorMaker.Grid[_gIndex].TileTypeGetSet == FloorTile.TileType.Concrete)
                {
                    if (!onTheFloor)
                    {
                        StopMe();
                        manager.makeConcreteTiles();
                    }
                    onTheFloor = true;
                }
                else
                {
                    onTheFloor = false;
                    if (manager.floorMaker.Grid[_gIndex].TileTypeGetSet == FloorTile.TileType.Space)
                    {
                        manager.floorMaker.Grid[_gIndex].TileTypeGetSet = FloorTile.TileType.Tentative;
                        manager.addToTentativeList(manager.floorMaker.Grid[_gIndex]);
                    }

                }
            }
        }


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameSceneManager.LOGGING)
        Debug.Log("Collided Gameobject name:" + collision.gameObject.name);

        if ( collision.gameObject.tag == GameSceneManager.TILE_TAG )
        {
            FloorTile _tile = collision.gameObject.GetComponent<FloorTile>();
            if ( _tile.TileTypeGetSet == FloorTile.TileType.Tentative)
            {
                manager.RestartTurn();
            }
        }
        else if (collision.gameObject.tag == GameSceneManager.ENEMY_TAG)
        {
            manager.RestartTurn();
        }
        if ( collision.gameObject.tag == GameSceneManager.POWERUPS_TAG )
        {
            manager.ApplySlowSpell();
            collision.gameObject.SetActive(false);
        }
    }


}
