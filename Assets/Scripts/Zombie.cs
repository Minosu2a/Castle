using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{

    //3 TYPES OF EVILS BEHAVIOUR
    // 1. Dormants : They are stationary until a player trigger them
    // 2. Chase : They know the player location and hunt him down
    // 3. Traps : Hide and wait to jump at the player (Usually on celling, holes etc... )

    [Header("Values")]
    [SerializeField] private EZombieModeType _behaviour = EZombieModeType.NONE;

    [SerializeField] private int _hp = 3;

    [Tooltip("No need to change the NavMesh movespeed because the movespeed attribute of zombie class will overwrite the value at start")]
    [SerializeField] private float _movespeed = 6f;

    [SerializeField] private float _callRadius = 25f;


    [Header("Damage")]
    [SerializeField] private float _damagedMovespeed = 4f;
    [SerializeField] private float _damagedMalusTime = 2f;


    private Vector3 _dir = Vector3.zero;
    private Transform _characterPos = null;

    private Rigidbody _rb = null;
    private NavMeshAgent _navMesh = null;




    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _navMesh = GetComponent<NavMeshAgent>();
        _navMesh.speed = _movespeed;
        AudioManager.Instance.Start3DSound("S_EvilBreathing", this.transform); //REPLACE THIS TO BE IN DORMANT
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            WakingUp();
        }
    }

    private void WakingUp()
    {
        if(_behaviour != EZombieModeType.HUNTER)
        {
        _behaviour = EZombieModeType.HUNTER;
        AudioManager.Instance.Start3DSound("S_EvilYell", this.transform);
        AudioManager.Instance.StopSound(ESoundType.REPETITIVE3D, "S_EvilBreathing");
        }
    }

    private void Call()
    {
     /*   if(Physics.SphereCast(transform.position,_callRadius,transform.forward, out hit, 10))
        {

        }*/
    }

    private void Update()
    {
      
            switch(_behaviour)
            {
                case EZombieModeType.DORMANT:
                //SOME AUDIO IN LOOP OR SOMETHING
                    break;
                case EZombieModeType.HUNTER:
                    _navMesh.SetDestination(CharacterManager.Instance.CharacterController.transform.position);
                  /*  _dir = _characterPos.position - transform.position;
                    _rb.velocity = _dir.normalized * _movespeed;
                    transform.forward = _dir;*/
                    break;
                case EZombieModeType.HIDED:
                    break;
            }
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }


    public void TakeDamage(int damage)
    {
        _hp = _hp - damage;

        if(_hp <= 0)
        {
            Death();
        }
        else
        {
            StartCoroutine(DamageEffect(_damagedMalusTime));
           // Yelling Sound
           // Blood Effect
           // Slow down the movement 
        }
    }

    IEnumerator DamageEffect(float delayTime)
    {
        _navMesh.speed = _damagedMovespeed;
        yield return new WaitForSeconds(delayTime);
        _navMesh.speed = _movespeed;
    }
}
