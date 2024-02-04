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

        [field:Header("References")]
        [SerializeField] public NavMeshAgent agent {get;set;}
        [SerializeField] public  GameObject player;
        [field:Header("Monster Agent Speed Modifier")]
        [SerializeField] public int rageCount;
        [SerializeField] public float speedMonster;
        [field:Header("Monster FOV Parameters")]
        [SerializeField] float attackDistance; //Disstance du joueur avant d'attaquer
        [Range(1,360)]
        [SerializeField] int fov;// moitié du fov!
        [SerializeField] public float searchInterval;//Interval de la coroutine de recherche
        [SerializeField] public float searchRadius; //Rayon de recherche de la sphere
        [field:Header("Monster Respawn")]
        public GameObject[] checkpoint; //Gameobjects pour respawn
        [SerializeField] public float respawnDistance; //Rayon de recherche de la sphere
        
        [field:Header("Monster Mask Paramaters")]
        [SerializeField] public LayerMask targetMask; //Mask du joueur
        [SerializeField] public LayerMask obstructionMask; //Mask de l'environment
        public bool canSeePlayer{get;private set;} //Bool pour savoir si le joueur est en ligne de vue
        //public GameEvent monsterEvent;

        [field:Header("Monster States")]
        [SerializeField] public StateController stateController;
        [SerializeField] public State currentState;
        [SerializeField] public bool isStunned;
        private Stalk stalk;
        private Chase chase;
        private Attack attack;
        private Respawn respawn;
        private Angry angry;
        private Stunned stunned;
        
        void Awake()
        {
            stateController=GetComponent<StateController>();
            agent=GetComponent<NavMeshAgent>();
            stalk=new(this);
            chase=new(this);
            attack=new(this);
            respawn=new(this);
            angry=new(this);
            stunned=new(this);

            if(player==null)
            {
                player=GameObject.FindGameObjectWithTag("Player");
            }
            if(checkpoint.Length==0)
            {
                checkpoint=GameObject.FindGameObjectsWithTag("Checkpoint");
            }
        }
        void Start()
        {
            stateController.ChangeState(stalk);
            canSeePlayer=false;
            if(player==null)
            {
                player=GameObject.FindGameObjectWithTag("Player");
            }
            StartCoroutine(FOVRoutine());

            //Debug.Log($"Current state is: {stateController.currentState}");
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log($"<color=yellow>Can see player? :{canSeePlayer}</color>");
            if(isStunned)
            {
                if(stateController.currentState!=stunned)stateController.ChangeState(stunned);
            }
            else if(canSeePlayer)
            {
                if(Vector3.Distance(transform.position,player.transform.position)<attackDistance)
                {
                    if(stateController.currentState!=attack)
                    {
                        stateController.ChangeState(attack);
                    }
                }
                else
                {
                    if(rageCount<2)
                    {
                        //Debug.Log($"{stateController.currentState}");
                        if(stateController.currentState!=chase)stateController.ChangeState(chase);
                    }
                }
            }
            else
            {
                if(agent.destination==null)
                {
                    Debug.Log("destination null");
                    //Checkpoint nearest to player?
                    agent.SetDestination(checkpoint[0].transform.position);
                }
                
                if(Vector3.Distance(transform.position,player.transform.position)<=respawnDistance && rageCount<2)
                {
                    if(stateController.currentState!=stalk)stateController.ChangeState(stalk);
                }
                else if(Vector3.Distance(transform.position,player.transform.position)>respawnDistance)
                {
                    if(stateController.currentState!=respawn)stateController.ChangeState(respawn);
                }
                if(rageCount>=2)
                {
                    if(stateController.currentState!=angry)stateController.ChangeState(angry);
                }
            }

        } 
        private IEnumerator FOVRoutine()
        {
            WaitForSeconds wait= new WaitForSeconds(searchInterval);
            while(true)
            {
                yield return wait;
                //Debug.Log("FOV Routine");
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
                        Debug.Log("In angle, no obstructions");
                    }
                    else
                    {
                        //Monster must be more inquisitive!State change?
                        Debug.Log("Looking at Player");
                        transform.rotation=Quaternion.LookRotation(player.transform.position,Vector3.up);
                        //canSeePlayer=false;
                    }
                }
                canSeePlayer=true;
            }
            else 
            {
                Debug.Log("Out of angle");
                canSeePlayer = false;
            }
            // else if (canSeePlayer)
            // {
            //     //canSeePlayer=false;
            // }
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
    }
}

