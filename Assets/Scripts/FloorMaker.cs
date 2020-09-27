using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMaker : MonoBehaviour
{
    [Header("Not Less then 30")]
    public int Rows;

    [Header("Not Less then 22,Should match 30/22=1.36 Aspect Ratio.")]
    public int Columns;

    [SerializeField]
    Camera SceneCam;

    public GameObject PrefabTile;
    public Transform TileParent;

    public FloorTile[,] Grid;

    // Start is called before the first frame update
    void Start()
    {
        if ( Rows<30 || Columns<22 )
        {
            Rows = 30;Columns = 22;
        }
        Grid = new FloorTile[Rows, Columns];
        this.transform.localScale = new Vector3(Rows,Columns,this.transform.localScale.z);

        float offset = (((float)Columns) * (4.5f / 100.0f));//We offset the height of Grid by 5% of Columns number
        SceneCam.orthographicSize = (Columns / 2) + offset;

        SceneCam.transform.position = new Vector3((float)Rows*0.5f, (float)Columns*0.5f ,SceneCam.transform.position.z);
        PopulateTiles();
    }

    void PopulateTiles()
    {
        GameObject _tempObj;
        for( int Xindex=0;Xindex<Rows;Xindex++ )
        {
            for(int Yindex=0;Yindex<Columns;Yindex++)
            {
                _tempObj = Instantiate(PrefabTile,TileParent) as GameObject;
                if( _tempObj != null )
                {
                    Grid[Xindex, Yindex] = _tempObj.GetComponent<FloorTile>();
                    _tempObj.transform.position = new Vector3(Xindex,Yindex,_tempObj.transform.position.z);
                    if ( Xindex == 0 || Yindex==0 || Xindex==Rows-1 || Yindex==Columns-1 )
                    {
                        Grid[Xindex, Yindex].TileTypeGetSet = FloorTile.TileType.Boundary;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
