using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManScript : MonoBehaviour
{
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
                StopMe();
                manager.makeConcreteTiles();
            }
            else if (_tempPositionVector.x + xchange > XLimits.y)
            {
                _tempPositionVector.x = _rows - 0.5f;
                StopMe();
                manager.makeConcreteTiles();
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
                StopMe();
                manager.makeConcreteTiles();
            }
            else if (_tempPositionVector.y + yChange > YLimits.y)
            {
                _tempPositionVector.y = _columns - 0.5f;
                StopMe();
                manager.makeConcreteTiles();
            }
        }
        
        this.transform.position = _tempPositionVector;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE

        if ( Input.GetKeyDown(KeyCode.UpArrow) )
        {
            //MoveMe(0f,1f);
            if ( _direction.y==0f || onTheFloor )
            {
                _direction.x = 0f;
                _direction.y = 1f;
                this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                if (_entered == null)
                    this.transform.position = new Vector2(Mathf.Floor(this.transform.position.x) + 0.5f, Mathf.Floor(this.transform.position.y) + 0.5f);
                else this.transform.position = new Vector2(_entered.transform.position.x + 0.5f, _entered.transform.position.y + 0.5f);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if ( _direction.y==0f || onTheFloor)
            {
                //MoveMe(0f,-1f);
                _direction.x = 0f;
                _direction.y = -1f;
                this.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                if (_entered == null)
                    this.transform.position = new Vector2(Mathf.Floor(this.transform.position.x) + 0.5f, Mathf.Floor(this.transform.position.y) + 0.5f);
                else this.transform.position = new Vector2(_entered.transform.position.x + 0.5f, _entered.transform.position.y + 0.5f);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if ( _direction.x==0f || onTheFloor)
            {
                //MoveMe(-1f,0f);
                _direction.x = -1f;
                _direction.y = 0f;
                this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                if (_entered == null)
                    this.transform.position = new Vector2(Mathf.Floor(this.transform.position.x) + 0.5f, Mathf.Floor(this.transform.position.y) + 0.5f);
                else this.transform.position = new Vector2(_entered.transform.position.x + 0.5f, _entered.transform.position.y + 0.5f);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if ( _direction.x==0f || onTheFloor)
            {
                _direction.x = 1f;
                _direction.y = 0f;
                this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                if (_entered == null)
                    this.transform.position = new Vector2(Mathf.Floor(this.transform.position.x) + 0.5f, Mathf.Floor(this.transform.position.y) + 0.5f);
                else this.transform.position = new Vector2( _entered.transform.position.x + 0.5f,_entered.transform.position.y+0.5f );
            }
        }
        MoveMe();

#elif (UNITY_ANDROID || UNITY_IOS)
        
#endif
    }

    FloorTile _entered;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided Gameobject name:"+collision.gameObject.name);
        if (manager.gameState == GameSceneManager.GameState.GameStarted)
        {
            if (collision.gameObject.tag == GameSceneManager.TILE_TAG)
            {
                _entered = collision.gameObject.GetComponent<FloorTile>();
                if (_entered.TileTypeGetSet == FloorTile.TileType.Concrete)
                {
                    onTheFloor = true;
                }
                else onTheFloor = false;

            }
        }
    }

    FloorTile _exited;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ( manager.gameState == GameSceneManager.GameState.GameStarted )
        {
            if ( collision.gameObject.tag == GameSceneManager.TILE_TAG )
            {
                _exited = collision.gameObject.GetComponent<FloorTile>();

                if (_exited != null)
                {
                    if (_exited.TileTypeGetSet != FloorTile.TileType.Concrete  )
                    {
                        _exited.TileTypeGetSet = FloorTile.TileType.Tentative;
                        manager.addToTentativeList(_exited);
                    }

                    if (_entered.TileTypeGetSet == FloorTile.TileType.Concrete)
                    {
                        if (_exited.TileTypeGetSet == FloorTile.TileType.Tentative)
                        {
                            StopMe();
                            manager.makeConcreteTiles();
                        }
                    }

                }                
            }            
        }
    }




}
