using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiShell : MonoBehaviour
{
    public Image ShellOuter;
    public Image ShellInterior;

    private PanelController ShellOuterCon;
    private PanelController ShellInteriorCon;

    public Color WarningColor;
    private Color StandardColor;

    [Tooltip("The clamp value on either side of the Size variable - minSize-maxSize")]
    public float ClampSizeFloat;

    [Tooltip("How much before (maxSize - ClampSizeFloat) the bar will star turning red")]
    public float WarningStartsAtXBeforeEndOfSize;

    public float FlashRatePerSecond;
    private int FlashRate;
    private int FlashRateCount;

    private GameObject gameManager;
    private ShellManager shellManager;
    private CrabSizeManager sizeManager;

    float _size;
    
    bool wearingShell;
    WearableShell.ShellData shellData;



    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        shellManager = gameManager.GetComponent<ShellManager>();
        sizeManager = gameManager.GetComponent<CrabSizeManager>();

        sizeManager.AddSizeChangeListener(OnSizeChange);
        shellManager.AddShellChangeListener(OnShellChange);

        StandardColor = Color.white;

        ShellOuterCon = ShellOuter.gameObject.GetComponent<PanelController>();
        ShellInteriorCon = ShellInterior.gameObject.GetComponent<PanelController>();

        ShellOuterCon.TweenOut();
        ShellInteriorCon.TweenOut();

        FlashRate = 2 * (int)Mathf.Floor(60f * FlashRatePerSecond);

    }

    private void Update()
    {

        if (!wearingShell) return;

        _setShellUI();


    }

    private void OnSizeChange(float __size)
    {
        _size = __size;
        
    }

    private void OnShellChange(bool __wearingShell, WearableShell.ShellData __shellData)
    {
        wearingShell = __wearingShell;
        shellData = __shellData;

        if(wearingShell)
        {
            ShellInteriorCon.Tween();
            ShellOuterCon.Tween();
        }
        else
        {
            ShellInteriorCon.TweenOut();
            ShellOuterCon.TweenOut();
        }

    }

    private void _setShellUI()
    {
        FlashRateCount = (FlashRateCount + 1) % FlashRate;

        ShellInterior.fillAmount = Mathf.InverseLerp(shellData.minSize + ClampSizeFloat, shellData.maxSize - ClampSizeFloat, _size);

        float _baseLine = shellData.maxSize - ClampSizeFloat;

        if (_size >= (_baseLine - WarningStartsAtXBeforeEndOfSize))
        {
            //float _lerp = Mathf.InverseLerp(_baseLine - WarningStartsAtXBeforeEndOfSize, _baseLine, _size);
            float _lerp = Mathf.InverseLerp(0, FlashRate - 1, FlashRateCount);
            
            //Color _col = Color.Lerp(StandardColor, WarningColor, _lerp);
            Color _col = WarningColor;
            _col[3] = _lerp;

            _setShellColor(_col);
        }
        else
        {
            _setShellColor(StandardColor);
        }

    }

    private void _setShellColor(Color _color)
    {
        ShellInterior.color = _color;
        //ShellOuter.color = _color;

    }

}
