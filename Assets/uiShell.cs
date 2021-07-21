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
    private int FlashRate_Half;


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

        FlashRate_Half = (int)Mathf.Floor(60f * FlashRatePerSecond);
        FlashRate = 2 * FlashRate_Half;

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
        // Flash Rate is multiplied by 2 at the start (FlashRate_Half), so after we pass the middle, we subtract the lerp from 1
        // why?  This lets the alpha fade smoothly in and out
        FlashRateCount = (FlashRateCount + 1) % FlashRate;

        int singleFlashRateCount = FlashRateCount % (FlashRate_Half);

        ShellInterior.fillAmount = Mathf.InverseLerp(shellData.minSize + ClampSizeFloat, shellData.maxSize - ClampSizeFloat, _size);

        float _baseLine = shellData.maxSize - ClampSizeFloat;

        if (_size >= (_baseLine - WarningStartsAtXBeforeEndOfSize))
        {
            //float _lerp = Mathf.InverseLerp(_baseLine - WarningStartsAtXBeforeEndOfSize, _baseLine, _size);
            float _lerp = Mathf.InverseLerp(0, FlashRate_Half - 1, singleFlashRateCount);
            if (FlashRateCount >= FlashRate_Half) _lerp = 1 - _lerp;
            
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
