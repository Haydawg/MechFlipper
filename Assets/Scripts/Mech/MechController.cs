using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MechController : MonoBehaviour
{
    public enum State
    {
        MovingToAttack,
        Aiming,
        Firing,
    }
    [SerializeField] private State state;
    [Header("Stats")]
    [SerializeField] private float stateUpdateTick = 0.02f;
    private float stateUpdateTimer;
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float attackTimer;
    [SerializeField] private float searchRadius;

    [Header("Components")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private List<GameObject> potentialDrops = new List<GameObject>();

    [Header("Weapons")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float projectileVelocity;
    [SerializeField] private Transform projectileSpawnLoc;

    [Header("Level Information")]
    [SerializeField] private Transform endPoint;

    [SerializeField] private GameObject target;
    [SerializeField] private bool isMoving;
    public enum Team
    {
        Player,
        Enemy
    };

    [SerializeField] Team team;

    private void Start()
    {
        
    }
    private void Update()
    {
        stateUpdateTimer += Time.deltaTime;
        if (stateUpdateTimer < stateUpdateTick) return; // pervents checking every update 
        stateUpdateTimer= 0;
        if(!target)
        {
            Debug.Log("NoTarget");
            agent.enabled = true;
            agent.SetDestination(endPoint.position);
            anim.SetBool("Is Moving", true);
            //isMoving = true;
            Debug.Log("MovingtoPoint");
            target = SearchForEnemy();
            return;
        }
        //agent.SetDestination(target.transform.position);
        switch (state)
        {
            case State.MovingToAttack:
                Movement();
                break;
            case State.Aiming:
                Aiming();
                break;
            case State.Firing:
                break;
        }
        
    }

    public GameObject SearchForEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent<MechController>(out MechController mech))
            {
                if (mech.team != team & mech.isActiveAndEnabled)
                {

                    agent.SetDestination(mech.gameObject.transform.position);
                    state = State.MovingToAttack;
                    return mech.gameObject;
                }
            }
        }
        return null;
    }

    public void Idle()
    {
        if (isMoving)
        {
            Debug.Log(("Stop"));
            agent.enabled= false;
            isMoving = false;
            anim.SetBool("Is Moving", false);
        }
    }

        public void Movement()
    {
        if (!TargetInRange(target))
        {
          
            if (!isMoving)
            {
                agent.enabled = true;

                Debug.Log("MovingtoTarget");
                isMoving = true;
                agent.SetDestination(endPoint.position);
                anim.SetBool("Is Moving", true);
            }
        }
        else
        {
            Debug.Log("Target In range");
            state = State.Aiming;
        }
    }
    public void Aiming()
    {
        Idle();

        transform.LookAt(target.transform.position);
    
        Firing();
    
    }

    public void Firing()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= fireRate)
        {
            anim.SetTrigger("Shoot");
            Projectile projectile = Instantiate(projectilePrefab, projectileSpawnLoc.position, projectileSpawnLoc.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * 1000);
            attackTimer = 0;
        }
    }
    void Die()
    {
        if (team == Team.Enemy)
        {
            SpawnController.Instance.RemoveFromSpawnedList();
            GameObject objectToDrop = potentialDrops[Random.Range(0, potentialDrops.Count)];
            Instantiate(objectToDrop, objectToDrop.transform.position, objectToDrop.transform.rotation);
        }
        else
        {
            GameManager.Instance.DeadFriendlyMech();
        }
        Destroy(gameObject);
        //DoDie stuff

    }

    public void TakeDamager(float damageAmount)
    {
        health -= damageAmount;
        if(health <= 0)
        {
            Die();
        }
    }

    private bool TargetInRange(GameObject targetObject)
    {
        return Vector3.Distance(transform.position, targetObject.transform.position) < range;
    }

    public void InitMech(Transform walkEndPoint)
    {
        endPoint = walkEndPoint;
    }
    //team getter and setter
    public void SetTeam(Team team)
    {
        this.team = team;
    }
    public Team GetTeam() 
    {
        return this.team;
    }
}
