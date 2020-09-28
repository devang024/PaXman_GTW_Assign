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
                case TileType.Concrete:
                    GetComponent<SpriteRenderer>().color = Color.white;
                    break;

                case TileType.Tentative:
                    GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.5f);
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
