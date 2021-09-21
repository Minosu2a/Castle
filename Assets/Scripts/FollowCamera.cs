using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] private GameObject _character = null;
    [SerializeField] private float _distanceHighCam = 15;

    private float _charaX = 0f;
    private float _charaY = 0f;
    private float _charaZ = 0f;

    void Update()
    {
        _charaX = _character.transform.position.x;
        _charaY = _character.transform.position.y + _distanceHighCam;
        _charaZ = _character.transform.position.z;

        Vector3 camMove = new Vector3(_charaX, _charaY, _charaZ);

        transform.position = camMove;
    }
}
