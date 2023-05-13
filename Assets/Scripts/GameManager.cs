using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int mechsRemaining;
    public static GameManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeadFriendlyMech()
    {
        mechsRemaining--;
        if(mechsRemaining == 0 ) 
        {
            //GameOver
        }
    }
}
