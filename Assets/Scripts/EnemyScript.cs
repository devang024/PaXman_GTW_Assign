using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float SlowSpeed, FastSpeed;
    Rigidbody2D _rigidbody;
    public enum EnemyType
    {
        Slow,
        Fast
    }

    float _speed = 0f;

    bool gameRunning = false;
    public void StartGame()
    {
        gameRunning = true;
        if (this.enemyType == EnemyType.Slow) _speed = SlowSpeed;
        else _speed = FastSpeed;
        //_velocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized*_speed;
        changeDirection();
    }
    Vector2 _velocity = new Vector2();
    public EnemyType enemyType = EnemyType.Slow;

    // Start is called before the first frame update
    void Start()
    {
         _floorMaker = FindObjectOfType<FloorMaker>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    Vector3 _postion = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.identity;
        if ( gameRunning )
        {
            _postion.x = this.transform.position.x + (_velocity.normalized.x * _speed);
            _postion.y = this.transform.position.y + (_velocity.normalized.y * _speed);
            this.transform.position = _postion;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == GameSceneManager.TILE_TAG)
        {
            FloorTile _tile = collision.gameObject.GetComponent<FloorTile>();

            if (_tile != null)
            {
                if (_tile.TileTypeGetSet == FloorTile.TileType.Concrete)
                {
                    changeDirection();
                }
            }
        }
    }

    FloorMaker _floorMaker = null;
    void changeDirection()
    {        
        if (_floorMaker != null)
        {
            float _x = Random.Range(0, _floorMaker.Rows);
            float _y = Random.Range(0, _floorMaker.Columns);
            _velocity = (new Vector2(_x - this.transform.position.x, _y - this.transform.position.y)).normalized;

        }

    }

}
