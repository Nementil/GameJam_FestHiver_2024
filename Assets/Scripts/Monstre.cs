using System.Runtime.Serialization;
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
        [SerializeField] public monsterState state {get;private set;}
        [SerializeField] private GameObject player;
        [SerializeField] private float speedMonster;
        [SerializeField] private float accelerationMonster;
        [SerializeField] float attackDistance; //Disstance du joueur avant d'attaquer
        [Range(1,360)]
        [SerializeField] int fov;// moitié du fov!
        [SerializeField] private float searchInterval;//Interval de la coroutine de recherche
        [SerializeField] private float searchRadius; //Rayon de recherche de la sphere
        [SerializeField] private float respawnDistance; //Rayon de recherche de la sphere
        [SerializeField] public int rageCount {get;private set;}
        private bool isAngry=false;
        [SerializeField] private LayerMask targetMask; //Mask du joueur
        [SerializeField] private LayerMask obstructionMask; //Mask de l'environment
        [SerializeField] public GameObject[] checkpoint{get; private set;} //Gameobjects pour respawn
        [SerializeField] public bool canSeePlayer{get;private set;} //Bool pour savoir si le joueur est en ligne de vue
        //public GameEvent monsterEvent;
        [SerializeField] public StateController stateController;
        [SerializeField] public State currentState;
        void Start()
        {
            speedMonster=agent.speed;
            accelerationMonster=agent.acceleration;
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
            currentState.OnEnter(stateController);
            //agent.SetDestination(player.transform.position);
            //player is near?Raycast?
            //Debug.Log($"Agent destination is: {agent.destination}");
            Debug.Log($"<color=yellow>Can see player? :{canSeePlayer}</color>");
            if(canSeePlayer)
            {
                if(Vector3.Distance(transform.position,player.transform.position)<attackDistance)
                {
                    if(state!=monsterState.Attack)
                    {
                        state=monsterState.Attack;
                        StateChange();
                    }
                }
                else
                {
                   
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
                
                if(Vector3.Distance(transform.position,player.transform.position)<=respawnDistance)
                {
                    state=monsterState.Stalk;
                    StateChange();
                }
                else
                {
                    state=monsterState.Respawn;
                    StateChange();
                }
                if(rageCount>5 && !isAngry)
                {
                    state=monsterState.Angry;
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
                //Debug.Log("Player in Sphere");
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget= (target.position-transform.position).normalized;
                Debug.DrawRay(transform.position,directionToTarget,Color.red);
                if(Vector3.Angle(transform.forward,directionToTarget)>fov/2)
                {
                    Debug.Log("In line of sight");
                    float distanceToTarget= Vector3.Distance(transform.position,target.position);
                    if(!Physics.Raycast(transform.position,directionToTarget,distanceToTarget,obstructionMask))
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
            agent.speed=speedMonster;
            agent.acceleration=accelerationMonster;
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
                if(RandomPoint(transform.position,10f,out point))
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
        private void IsAngry()
        {
            agent.speed*=1.5f;
            agent.acceleration*=1.1f;
            agent.destination=player.transform.position;
        }

        public void StateChange()
        {
            //CurrentState();
            switch (state) {
            case monsterState.Chase:
                IsChasing();
                rageCount++;
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
            case monsterState.Angry:
                IsAngry();
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
