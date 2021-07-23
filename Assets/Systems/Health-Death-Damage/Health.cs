using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *   // Listener for Death: AddDeathListener(OnDeathDelegate method) -> OnDeath passes the PlayerObject (main object that houses everything)
    // Listener for Health Changes: AddHealthChangeListener(OnHealthChangeDelegate method) -> OnHealthChangeDelegate passes health (float) and health percentage (float, 0-1)
 *     AddHealth(float) Adds health
 *     SubtractHealth(float) Subtracts health
 *     ResetHealth() Resets health to max
 *     
 *     
 *     PauseHealthChange() / UnpauseHealthChange()
 *     
 *     
 */



public class Health : MonoBehaviour
{
    private float HealthToRestore = 0.50f;


    public float health;
    public float maxHealth;
    public int NumberOfEggs;
    private int MaxNumberOfEggs;
    private int respawn_NumberOfEggs;
    private float respawn_health;
    private int old_numberOfEggs;
    public AudioSource sizzle;
    public AudioSource crack; 
    public GameObject CharacterRootObject;

    public bool DEBUGFreezeHealth;

    public Respawn respawnController;


    private void Awake()
    {
        print("Player Start"); 
        if(CharacterRootObject == null) CharacterRootObject = GameObject.FindGameObjectWithTag("Player");
        if (NumberOfEggs == 0) NumberOfEggs = 7;

        MaxNumberOfEggs = NumberOfEggs;
        old_numberOfEggs = NumberOfEggs;


     //   GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
      //  respawnController = gameController.GetComponent<Respawn>();

        respawnController.AddRespawnListener(OnRespawn);
        respawnController.AddSetRespawnPointListener(OnRespawnSet);

        respawn_NumberOfEggs = NumberOfEggs;
        respawn_health = health;
        print("Player End");
    }



    #region OnHealthChangeDelegate event methods

    // --- the Delegate, for alerting other classes that we've changed XP levels
    public delegate void OnHealthChangeDelegate(float health, float healthPercentage);
    public OnHealthChangeDelegate OnHealthChange = delegate { }; 

    public void AddHealthChangeListener(OnHealthChangeDelegate __del)
    {
       // print("A");
        //print(OnHealthChange);
        //print(" _DEL: " + __del);
       // OnHealthChange += __del;
        //print("B");
    }

    public void RemoveHealthChangeListener(OnHealthChangeDelegate __del)
    {
        OnHealthChange -= __del;
    }


    #endregion



    #region OnDeathDelegate event methods

    // --- the Delegate, for alerting other classes that we've changed XP levels
    public delegate void OnDeathDelegate(GameObject PlayerObject);
    public event OnDeathDelegate OnDeath;

    public void AddDeathListener(OnDeathDelegate __del)
    {
        OnDeath += __del;

    }

    public void RemoveDeathListener(OnDeathDelegate __del)
    {
        OnDeath -= __del;
    }


    #endregion




    #region OnEggChange event methods (float eggTotal, float eggChange)

    // --- the Delegate, for alerting other classes that we've changed XP levels
    public delegate void OnEggChangeDelegate(int eggTotal,int eggChange);
    public event OnEggChangeDelegate OnEggChange;

    public void AddEggChangeListener(OnEggChangeDelegate __del)
    {
        OnEggChange += __del;

    }

    public void RemoveEggChangeListener(OnEggChangeDelegate __del)
    {
        OnEggChange -= __del;
    }


    #endregion



    public void PauseHealthChange()
    {
        DEBUGFreezeHealth = true;

    }

    public void UnpauseHealthChange()
    {
        DEBUGFreezeHealth = false;

    }




    public void ResetHealth()
    {
        AddHealth(10000);

    }



    public void AddHealth(float __health)
    {

        _AddHealth(__health);

    }

    public void SubtractHealth(float __health)
    {
        _AddHealth(-__health);
    }

    public float GetHealth()
    {
        return health;

    }


    private void _AddHealth(float __health)

    {
        if (DEBUGFreezeHealth) return;
        print("7" + " WE ARE IN THE HEALTH SCRIPT"); 
        float __oldhealth = health;

        health = Mathf.Clamp(health + __health, 0, maxHealth);

        if (health != __oldhealth)
        {
            print("8");
            if (OnHealthChange != null)
            {
                print("9");
                print(health);
                print( maxHealth);
                print(OnHealthChange);
                print("test"); 
                OnHealthChange?.Invoke(health, health/maxHealth);
                print("10");
            }
        }

        if (health <= 0)
        {
            SubtractEgg(1);

            health = HealthToRestore * maxHealth;

            if(NumberOfEggs <= 0)
            {
                if (OnDeath != null) OnDeath(CharacterRootObject);
            }
            
        }
        print("11");
    }

    
    void OnRespawnSet(GameObject playerRootObj)
    {
        respawn_NumberOfEggs = NumberOfEggs;
        respawn_health = health;
    }


    void OnRespawn(GameObject playerRootObj)
    {
        NumberOfEggs = respawn_NumberOfEggs;
        AddHealth(respawn_health - health);

        //health = respawn_health;

        if (OnEggChange != null) OnEggChange(NumberOfEggs, NumberOfEggs - old_numberOfEggs);
        old_numberOfEggs = NumberOfEggs;

    }


    void SubtractEgg(int number)
    {
        crack.Play(); 
        NumberOfEggs = Mathf.Clamp(NumberOfEggs - number, 0, MaxNumberOfEggs);
        if(NumberOfEggs != old_numberOfEggs)
        {
            old_numberOfEggs = NumberOfEggs;
            if (OnEggChange != null) OnEggChange(NumberOfEggs, -number);
        }

    }


    public int GetEggs()
    {
        return NumberOfEggs;

    }


}
