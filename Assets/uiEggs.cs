using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiEggs : MonoBehaviour
{
    public Text eggCount;

    public Image redEgg;

    [SerializeField] private Health healthManager;
    private int NumberOfEggs;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");

        healthManager = gameManager.GetComponent<Health>();

        //healthManager.AddHealthChangeListener(onHealthChange);
       // healthManager.AddEggChangeListener(OnEggChange);

        NumberOfEggs = healthManager.GetEggs();
        OnEggChange(NumberOfEggs);



    }
    private void Update()
    {
        NumberOfEggs = healthManager.GetEggs();
        OnEggChange(NumberOfEggs);
        onHealthChange(healthManager.health,healthManager.health/healthManager.maxHealth); 
    }
    private void OnEggChange(int eggNumber)
    {
        eggCount.text = eggNumber.ToString();
        NumberOfEggs = eggNumber;

    }

    private void onHealthChange(float newHealth,float healthPercent)
    {
        redEgg.fillAmount = 1 - healthPercent;
    }


}
