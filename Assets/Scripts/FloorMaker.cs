using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMaker : MonoBehaviour
{
    [Header("Not Less then 30")]
    public int Rows;

    [Header("Not Less then 22")]
    public int Columns;

    [SerializeField]
    Camera SceneCam;

    // Start is called before the first frame update
    void Start()
    {
        if ( Rows<30 || Columns<22 )
        {
            Rows = 30;Columns = 22;
        }
        this.transform.localScale = new Vector3(Rows,Columns,this.transform.localScale.z);
        SceneCam.orthographicSize = (Columns / 2);
        SceneCam.transform.position = new Vector3(Rows/2,Columns/2,SceneCam.transform.position.z);

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
