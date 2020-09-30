﻿using System.Collections;
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

    int Xindex, Yindex;
    FloorMaker.GridIndex _gIndex;
    private void FixedUpdate()
    {
        if (gameRunning)
        {
            Xindex = Mathf.FloorToInt(this.transform.position.x);
            Yindex = Mathf.FloorToInt(this.transform.position.y);
            _gIndex = new FloorMaker.GridIndex(Xindex, Yindex);
#if UNITY_EDITOR || UNITY_STANDALONE

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //MoveMe(0f,1f);
                if (_direction.y == 0f || onTheFloor)
                {
                    _direction.x = 0f;
                    _direction.y = 1f;
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    this.transform.position = new Vector2(manager.floorMaker.Grid[_gIndex].transform.position.x + 0.5f,
                        manager.floorMaker.Grid[_gIndex].transform.position.y + 0.5f);

                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_direction.y == 0f || onTheFloor)
                {
                    //MoveMe(0f,-1f);
                    _direction.x = 0f;
                    _direction.y = -1f;
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                    this.transform.position = new Vector2(manager.floorMaker.Grid[_gIndex].transform.position.x + 0.5f,
                        manager.floorMaker.Grid[_gIndex].transform.position.y + 0.5f);

                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_direction.x == 0f || onTheFloor)
                {
                    //MoveMe(-1f,0f);
                    _direction.x = -1f;
                    _direction.y = 0f;
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    this.transform.position = new Vector2(manager.floorMaker.Grid[_gIndex].transform.position.x + 0.5f,
                        manager.floorMaker.Grid[_gIndex].transform.position.y + 0.5f);
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_direction.x == 0f || onTheFloor)
                {
                    _direction.x = 1f;
                    _direction.y = 0f;
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    this.transform.position = new Vector2(manager.floorMaker.Grid[_gIndex].transform.position.x + 0.5f,
                        manager.floorMaker.Grid[_gIndex].transform.position.y + 0.5f);

                }
            }

#elif (UNITY_ANDROID || UNITY_IOS)
        
#endif

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
    }


}
