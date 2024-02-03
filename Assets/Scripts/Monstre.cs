using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

namespace UnityEngine{
    public class Monstre : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent; 
        [SerializeField] private monsterState state;
        [SerializeField] private GameObject player;
        [SerializeField] float distanceLimit;
        [SerializeField] int fov;// moitié du fov!
        [SerializeField] private float searchInterval;
        [SerializeField] private float searchRadius;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstructionMask;
        [SerializeField] private GameObject[] checkpoint;
        [SerializeField] private bool canSeePlayer;
        //public GameEvent monsterEvent;

        void Start()
        {
            canSeePlayer=false;
            if(player==null)
            {
                player=GameObject.FindGameObjectWithTag("Player");
            }
            state=monsterState.Stalk;
            IsStalking();
            //CurrentState();
            StartCoroutine(FOVRoutine());
        }

        // Update is called once per frame
        void Update()
        {
            agent.SetDestination(player.transform.position);
            //player is near?Raycast?
            // Debug.Log(state.ToString());
            if(canSeePlayer)
            {
                if(Vector3.Distance(this.transform.position,player.transform.position)<distanceLimit)
                {
                    if(state!=monsterState.Attack)
                    {
                        state=monsterState.Attack;
                        StateChange();
                    }
                }
                else
                {
                    if(state!=monsterState.Chase)
                    {
                        state=monsterState.Chase;
                        StateChange();
                    }
                }
            }
            else
            {
                if(agent.destination==null)
                {
                    Debug.Log("destination null");
                    agent.SetDestination(checkpoint[0].transform.position);
                }
                if(state!=monsterState.Stalk)
                {
                    state=monsterState.Stalk;
                    StateChange();
                }
            }

        } 
        private IEnumerator FOVRoutine()
        {
            WaitForSeconds wait= new WaitForSeconds(searchInterval);
            while(true)
            {
                yield return wait;
                Debug.Log("FOV Routine");
                IsSeeingPlayer();
            }
        }
        private void IsSeeingPlayer()
        {
            Collider [] rangeChecks=Physics.OverlapSphere(transform.position,searchRadius,targetMask);
            if(rangeChecks.Length!=0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTraget= (target.position-transform.position).normalized;
                if(Vector3.Angle(transform.forward,directionToTraget)>fov/2)
                {
                    float distanceToTarget= Vector3.Distance(transform.position,target.position);
                    if(!Physics.Raycast(transform.position,directionToTraget,distanceToTarget))
                    {
                        canSeePlayer=true;
                    }
                    else
                    {
                        //Monster must be more inquisitive!State change?
                        canSeePlayer=false;
                    }
                }
                else 
                {
                canSeePlayer = false;
                }
            }
            else if (canSeePlayer)
            {
                //canSeePlayer=false;
            }
        }
        private void IsChasing()
        {
            //Music chase?
            agent.destination=player.transform.position;
        }

        private void IsStalking()
        {
            //Teleport to nearest checkpoint to Player
            //If too far from player, tp to nearest checkpoint to player
            Debug.Log("Is Stalking");
            float[] checkpointDistance=new float[checkpoint.Length];
            agent.autoBraking=false;
            if(checkpoint.Length==0)
            {
                UnityEngine.Debug.Log("<color=red>No checkpoint in store Error<color/>");
                return;
            }
            if(Vector3.Distance(transform.position,player.transform.position)>500f)
            {
                
                for(int i=0;i>checkpoint.Length;i++)
                {
                    checkpointDistance[i]=Vector3.Distance(transform.position,checkpoint[i].transform.position);
                }
                float temp=checkpointDistance[0];
                int index=0;
                for(int j=0;j<checkpointDistance.Length;index++)
                {
                    if(checkpointDistance[j]<=temp)
                    {
                        temp=checkpointDistance[j];
                        index=j;
                    }
                }
                transform.position=checkpoint[index].transform.position;
            }
            if(agent.remainingDistance<=agent.stoppingDistance||agent.destination==null)
            {
                Vector3 point;
                if(RandomPoint(transform.position,5f,out point))
                {
                    agent.SetDestination(point);
                }
            }
        }
        private bool RandomPoint(Vector3 center,float range,out Vector3 result)
        {
            Vector3 randomPoint =center+ Random.insideUnitSphere*range;
            NavMeshHit hit;
            if(NavMesh.SamplePosition(randomPoint,out hit,1.0f,NavMesh.AllAreas))
            {
                result=hit.position;
                return true;
            }
            result= Vector3.zero;
            return false;
        }
        private void IsAttacking()
        {
            Debug.Log("Player in Attack Range!");
        }
        private void IsRespawning()
        {
            //Mettre in timer pour calculer quand il faut mixup
        }
        // public void CurrentState()
        // {
        //     Debug.Log("Sending");
        //     monsterEvent.Raise(this,state.ToString());
        // }
        public void StateChange()
        {
            //CurrentState();
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

    }
}
public enum monsterState
{
    Stalk,
    Chase,
    Attack,
    Respawn,
    Angry,

}
