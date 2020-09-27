using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{

    public enum TileType
    {
        Boundary,
        Fixed,
        Tentative,
        Space
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
                case TileType.Boundary:
                    GetComponent<SpriteRenderer>().color = Color.white;
                    break;
            }
            this._tileType = value;
        }
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
