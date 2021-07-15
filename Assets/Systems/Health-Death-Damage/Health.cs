using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *   // Listener for Death: AddDeathListener(OnDeathDelegate method) -> OnDeath passes the PlayerObject (main object that houses everything)
    // Listener for Health Changes: AddHealthChangeListener(OnHealthChangeDelegate method) -> OnHealthChangeDelegate passes health (float)
 *     AddHealth(float) Adds health
 *     SubtractHealth(float) Subtracts health
 *     ResetHealth() Resets health to max
 */



public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public GameObject CharacterRootObject;


   

    #region OnHealthChangeDelegate event methods

    // --- the Delegate, for alerting other classes that we've changed XP levels
    public delegate void OnHealthChangeDelegate(float health);
    public event OnHealthChangeDelegate OnHealthChange;

    public void AddHealthChangeListener(OnHealthChangeDelegate __del)
    {
        OnHealthChange += __del;

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


    private void _AddHealth(float __health)

    {
        float __oldhealth = health;

        health = Mathf.Clamp(health + __health, 0, maxHealth);

        if (health != __oldhealth)
        {
            if(OnHealthChange != null) OnHealthChange(health);
        }

        if (health <= 0)
        {
            if(OnDeath != null) OnDeath(CharacterRootObject);
        }

    }

    



}
