using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnController : MonoBehaviour
{
    public static SpawnController Instance;



    private int numOfEnemyMechs;
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private int maxNumOfMechs;
    [SerializeField] private float spawnTime;
    private float spawnTimer;
    [SerializeField] private MechController mechPrefab;
    [SerializeField] private Transform walkToTarget;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(numOfEnemyMechs < maxNumOfMechs)
        {
            spawnTimer += Time.deltaTime;
            if(spawnTimer < spawnTime)
            {
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        MechController mech = Instantiate(mechPrefab, spawnLocations[0].position, Quaternion.identity);
        numOfEnemyMechs++;
        mech.SetTeam(MechController.Team.Enemy);
        mech.InitMech(walkToTarget);
    }

    public void RemoveFromSpawnedList()
    {
        numOfEnemyMechs--;
    }
}
