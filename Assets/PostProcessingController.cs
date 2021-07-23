using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



public class PostProcessingController : MonoBehaviour
{

    public GameObject PostProcessingObject;
    public bool PauseProcess = false;

    private VolumeProfile volumeProfile;
    

    public float SecondsForHeatEffectToWearOff;

    public float MaxBurningVignetteIntensity = 0.7f;
    public float MaxContrast = 25f;

    private GameObject PlayerContainerObj;

    public SunDamage sunDamageManager;

    public float BurningFXMaxAtXPercentHealth = 0.6f;



    private Health healthController;

    private float _healthPercent;
    private bool _heatHasFinished = false;

    
    private Bloom bloom;
    private ColorAdjustments colorAdjustments;
    private ChannelMixer channelMixer;
    private LiftGammaGain liftGammaGain;
    private Tonemapping toneMapping;
    private Vignette vignette;
    private DepthOfField depthOfField;

    private float ca_ContrastDefault;





    // Start is called before the first frame update
    void Start()
    {

        PlayerContainerObj = GameObject.FindGameObjectWithTag("Player");

        //sunDamageManager = PlayerContainerObj.GetComponentInChildren<SunDamage>();

        healthController = gameObject.GetComponent<Health>();
       // print()
        //healthController.AddHealthChangeListener(OnHealthChange);
       // healthController.AddDeathListener(OnDeath);

        _healthPercent = 1f;

        volumeProfile = PostProcessingObject.GetComponent<Volume>().profile;

        if (!volumeProfile.TryGet(out bloom)) throw new System.NullReferenceException(nameof(bloom));
        if (!volumeProfile.TryGet(out colorAdjustments)) throw new System.NullReferenceException(nameof(colorAdjustments));
        if (!volumeProfile.TryGet(out liftGammaGain)) throw new System.NullReferenceException(nameof(liftGammaGain));
        if (!volumeProfile.TryGet(out toneMapping)) throw new System.NullReferenceException(nameof(toneMapping));
        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        if (!volumeProfile.TryGet(out depthOfField)) throw new System.NullReferenceException(nameof(depthOfField));

        SecondsForHeatEffectToWearOff = 1/(60 * SecondsForHeatEffectToWearOff);

        ca_ContrastDefault = colorAdjustments.contrast.value;


    }

    // Update is called once per frame
    /*
    void Update()
    {
        if(!sunDamageManager.InSunlight() && _healthPercent < 1f)
        {
            _healthPercent = Mathf.Clamp(_healthPercent + SecondsForHeatEffectToWearOff, 0f, 1f);
            _HeatEffect(_healthPercent);
            if (_healthPercent >= 1f) _heatHasFinished = false;
        }
    }
    */
    /*
    void OnHealthChange(float _health, float _healthPerVal)
    {
        bool in_sun = sunDamageManager.InSunlight();

        if (in_sun)
        {
            float _val = Mathf.Clamp(_healthPerVal, _healthPercent - 0.0155f, _healthPercent + 0.0155f);
            _HeatEffect(_val);
            _heatHasFinished = false;
            _healthPercent = _val;
        }
        else
        {
            _healthPercent = Mathf.Clamp(_healthPercent + 0.0125f,0f,1f);
            _HeatEffect(_healthPercent);
            if (_healthPercent >= 1f) _healthPercent = 1f;
        }

    }
    */
    private void FixedUpdate()
    {
        bool in_sun = sunDamageManager.InSunlight();

        if (in_sun)
        {
            float _val = Mathf.Clamp(healthController.health/ healthController.maxHealth, _healthPercent - 0.0155f, _healthPercent + 0.0155f);
            _HeatEffect(_val);
            _heatHasFinished = false;
            _healthPercent = _val;
        }
        else
        {
            _healthPercent = Mathf.Clamp(_healthPercent + 0.0125f, 0f, 1f);
            _HeatEffect(_healthPercent);
            if (_healthPercent >= 1f) _healthPercent = 1f;
        }

    }

    void _HeatEffect(float _amt)
    {
      
        float _val = Mathf.InverseLerp(0f, BurningFXMaxAtXPercentHealth, 1 - _amt);

        vignette.intensity.overrideState = true;
        vignette.intensity.value = Mathf.Clamp(_val, 0f, MaxBurningVignetteIntensity);
        
        //float _ca = Mathf.Round(Mathf.Lerp(ca_ContrastDefault, MaxContrast, _val) * 1000f);
        //colorAdjustments.contrast.value = _ca / 1000f;

    }


    void OnDeath(GameObject playerObj)
    {

        // do stuff in post processing if we're dead


    }


    public void SetCameraDOF(float cameraDOFFocalDistance, float cameraDOFFocalLength, float cameraDOFAperture)
    {

        depthOfField.focusDistance.overrideState = true;
        depthOfField.focusDistance.value = cameraDOFFocalDistance;

        depthOfField.focalLength.overrideState = true;
        depthOfField.focalLength.value = cameraDOFFocalLength;

        depthOfField.aperture.overrideState = true;
        depthOfField.aperture.value = cameraDOFAperture;

    }

}
