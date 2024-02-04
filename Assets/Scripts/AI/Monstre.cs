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
        [SerializeField] public NavMeshAgent agent {get;set;}
        [SerializeField] public monsterState state {get;set;}
        [SerializeField] public  GameObject player;
        [SerializeField] public float speedMonster{get;set;}
        [SerializeField] public float accelerationMonster{get;set;}
        [SerializeField] float attackDistance; //Disstance du joueur avant d'attaquer
        [Range(1,360)]
        [SerializeField] int fov;// moitié du fov!
        [SerializeField] public float searchInterval;//Interval de la coroutine de recherche
        [SerializeField] public float searchRadius; //Rayon de recherche de la sphere
        [SerializeField] public float respawnDistance{get;set;} //Rayon de recherche de la sphere
        [SerializeField] public int rageCount {get;set;}
        [SerializeField] public LayerMask targetMask; //Mask du joueur
        [SerializeField] public LayerMask obstructionMask; //Mask de l'environment
        public GameObject[] checkpoint; //Gameobjects pour respawn
        public bool canSeePlayer{get;private set;} //Bool pour savoir si le joueur est en ligne de vue
        //public GameEvent monsterEvent;
        [SerializeField] public StateController stateController;
        [SerializeField] public State currentState;
        private Stalk stalk;
        private Chase chase;
        private Attack attack;
        private Respawn respawn;
        private Angry angry;
        
        void Awake()
        {
            stateController=GetComponent<StateController>();
            agent=GetComponent<NavMeshAgent>();
            stalk=new(this);
            chase=new(this);
            attack=new(this);
            if(player==null)
            {
                player=GameObject.FindGameObjectWithTag("Player");
            }
        }
        void Start()
        {
            speedMonster=agent.speed;
            accelerationMonster=agent.acceleration;
            canSeePlayer=false;
            if(player==null)
            {
                player=GameObject.FindGameObjectWithTag("Player");
            }
            StartCoroutine(FOVRoutine());

            stateController.ChangeState(stalk);
            Debug.Log($"Current state is: {stateController.currentState}");
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log($"<color=yellow>Can see player? :{canSeePlayer}</color>");
            if(canSeePlayer)
            {
                if(Vector3.Distance(transform.position,player.transform.position)<attackDistance)
                {
                    if(state!=monsterState.Attack)
                    {
                        state=monsterState.Attack;
                        stateController.ChangeState(attack);
                    }
                }
                else
                {
                    {
                        state=monsterState.Chase;
                        stateController.ChangeState(chase);
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
                    stateController.ChangeState(stalk);
                }
                else
                {
                    state=monsterState.Respawn;
                    stateController.ChangeState(respawn);
                }
                if(rageCount>5)
                {
                    state=monsterState.Angry;
                    stateController.ChangeState(angry);
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
                Debug.Log("Player in Sphere");
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

        public bool RandomPoint(Vector3 center,float range,out Vector3 result)
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
