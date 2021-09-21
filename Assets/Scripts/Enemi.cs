using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemi : MonoBehaviour
{
    [SerializeField] private float _movespeed = 3f;

    private Vector3 _dir = Vector3.zero;
    private Transform _characterPos = null;
    private EnemiManager _enemiManager = null;

    private Rigidbody _rb = null;

    private bool _death = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Init(Transform newDir, EnemiManager enemiManager)
    {
        _characterPos = newDir;
        _enemiManager = enemiManager;
    }

    void Update()
    {
        if (_death == false)
        {
            _dir = _characterPos.position - transform.position;
            _rb.velocity = _dir.normalized * _movespeed;
            transform.forward = _dir;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.Instance.Start2DSound("S_Hit");
        }
    }

    public void Stop()
    {
        Debug.Log(this.gameObject);
        _death = true;
    }

    public void Death()
    {
        _enemiManager.EnemiObjects.Remove(this);
        Destroy(this.gameObject);
    }

    public void Restart()
    {
        _enemiManager.EnemiObjects.Remove(this);
        Destroy(this.gameObject);
    }

}
