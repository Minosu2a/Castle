using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiManager : MonoBehaviour
{
    [SerializeField] private List<Enemi> _enemiObject = null;
    [SerializeField] private GameObject _spawners = null;

    private bool _waveStarted = false;
    public List<Enemi> EnemiObjects
  {
        get
        {
            return _enemiObject;
        }
        set
        {
            _enemiObject = value;
        }
  }

    public GameObject Spawners
    {
        get
        {
            return _spawners;
        }
        set
        {
            _spawners = value;
        }
    }
    public bool WaveStarted
    {
        get
        {
            return _waveStarted;
        }
        set
        {
            _waveStarted = value;
        }
    }

    private void Start()
    {
        _enemiObject = new List<Enemi>();
    }
}
