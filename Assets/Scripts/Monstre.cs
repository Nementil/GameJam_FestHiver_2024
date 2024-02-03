using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Monstre : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent; 
    [SerializeField] private monsterState state;
    [SerializeField] private GameObject player;
    [SerializeField] float distanceToTarget;
    [SerializeField] int fov;// moitié du fov!
    
    void Start()
    {
        if(player==null)
        {
            player=GameObject.FindGameObjectWithTag("Player");
        }
        state=monsterState.Stalk;
    }

    // Update is called once per frame
    void Update()
    {
        //player is near?Raycast?
        if(IsSeeingPlayer())
        {
            if(Vector3.Distance(this.transform.position,player.transform.position)<distanceToTarget)
            {
                state=monsterState.Attack;
            }
            else
            {
                state=monsterState.Chase;
            }
        }
        else
        {
            state=monsterState.Stalk;
        }

        switch (state) {
            case monsterState.Chase:
                IsChasing();
                break;
            case monsterState.Stalk:
                IsStalking();
                break;
            case monsterState.Attack:
                IsAttacking();
                break;
            case monsterState.Respawn:
                IsRespawning();
                break;
            default :
                Debug.Log("Out of state, get respawn?");
                break;
        }
    } 

    private bool IsSeeingPlayer()
    {
        RaycastHit raycastHit;

        int angle=5;
        for (int i = -fov; i > fov; i+=angle)
        {
            //raycastHit=Physics.Raycast(transform.position,new Vector3(0f,i,0f),)   
        }
        return false;
    }
    private void IsChasing()
    {

    }

    private void IsStalking()
    {

    }

    private void IsAttacking()
    {

    }
    private void IsRespawning()
    {

    }
}

enum monsterState
{
    Stalk,
    Chase,
    Attack,
    Respawn,
    Angry,

}
