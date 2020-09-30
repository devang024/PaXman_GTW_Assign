using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{

    public List<FloorTile> Neighbours = new List<FloorTile>();
    public enum TileType
    {
        Concrete,
        Tentative,
        Space,
        Counting
    }
    GameSceneManager _manager;
    public void StartGame()
    {
        _manager = FindObjectOfType<GameSceneManager>();
    }

    TileType _tileType = TileType.Space;

    public TileType TileTypeGetSet
    {
        get
        {
            return this._tileType;
        }

        set
        {
            switch( value )
            {
                case TileType.Concrete:
                    {
                        GetComponent<SpriteRenderer>().color = Color.white;
                        GetComponent<BoxCollider2D>().isTrigger = false;
                        
                    }
                    break;

                case TileType.Tentative:
                    {
                        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                    }
                    break;
            }
            this._tileType = value;
        }
    }

    public int fillThis(TileType _tileType)
    {
        int retCount = 0;

        foreach( var item in Neighbours )
        {
            if (item.TileTypeGetSet == TileType.Space)
            {
                item.TileTypeGetSet = _tileType;
                retCount += 1 + item.fillThis(_tileType);
            }
        }

        return retCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
