using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeedMax = 0.5f;
    [SerializeField] private float _moveSpeed = 0.2f;
    [SerializeField] private int _damage = 0;

    private float _timeStamp = 0f;
    [SerializeField] private float _destroyTimer = 5f;

    private Vector3 _dir = Vector3.zero;
    private Rigidbody _rb = null;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
            Debug.LogError("No RigidBody found on Bullet");

        _rb.velocity = _dir * _moveSpeed;
    }

    public void Init(Vector3 newDir, float spread)
    {
      //  _dir = newDir;
        float randomSpreadX = Random.Range(spread, -spread);
        float randomSpreadZ = Random.Range(spread, -spread);

        Vector3 directionWithSpread = new Vector3(randomSpreadX, 0 , randomSpreadZ);
        _dir = newDir + directionWithSpread;

     
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemi")
        {
            other.GetComponent<Zombie>().TakeDamage(1);
            Destroy(this.gameObject);
        }
        else if(other.tag == "Wall")
        {
            Destroy(this.gameObject);
            AudioManager.Instance.Start2DSound("S_Impact");
        }

    }


    void Update()
    {
        // _rb.AddForce(_dir * _moveSpeed * Time.deltaTime);
        //  transform.position += _dir * _movespeed * Time.deltaTime;

        _timeStamp += Time.deltaTime;

        if (_timeStamp >= _destroyTimer)
        {
            Destroy(this.gameObject);
        }

    }
}


