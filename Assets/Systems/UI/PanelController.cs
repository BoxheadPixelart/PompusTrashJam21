using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using DentedPixel.LTExamples; 

public class PanelController : MonoBehaviour
{
    // Variables
    public enum TweenIn { move, rotateX, rotateY, rotateZ, scale, }
    public string animationTitle;
    public GameObject objectToAnimate;
    public TweenIn tweenOptions;

    [Header("Time")]
    public float durationTime;
    public float delayTime = 0;

    [Header("Parameters")]
    public Vector3 startingPosition;
    public Vector3 endingPosition;
    public float startingScale = 1;
    public float endingScale = 1;
    public float startingRotation = 0;
    public float endingRotation;

    [Header("Add Ons")]
    public LeanTweenType easeType;
    public LeanTweenType easeoutType;
    public bool doLoop = false;
    public int numberLoop;
    public bool doFade = false;
    public float startingAlpha;
    public float endingAlpha;

    // private variables
    bool animateOut;
    bool tweenedIn;
    bool isTweening;
    LTDescr animationTween;

    private void Awake() {
        startingPosition = objectToAnimate.transform.localPosition;
        objectToAnimate.transform.localScale = new Vector3(startingScale, startingScale, 1);
        tweenedIn = false;
        isTweening = false;
    }


    public void StartTween() {
        if (isTweening == false && tweenedIn == false)
            Tween();
        else if (isTweening == false && tweenedIn == true)
            TweenOut();
    }
    

    public void Tween() 
    { 
        // Assign animation.
        switch(tweenOptions)
        {
            case TweenIn.move:
                animationTween = LeanTween.moveLocal(objectToAnimate, endingPosition, durationTime).setEase(easeType).setDelay(delayTime).setOnComplete(TweenComplete);
            break;
            case TweenIn.rotateX:
                animationTween = LeanTween.rotateX(objectToAnimate, endingRotation, durationTime).setEase(easeType).setDelay(delayTime).setOnComplete(TweenComplete);
            break;
            case TweenIn.rotateY:
                animationTween = LeanTween.rotateY(objectToAnimate, endingRotation, durationTime).setEase(easeType).setDelay(delayTime).setOnComplete(TweenComplete);
            break;
            case TweenIn.rotateZ:
                animationTween = LeanTween.rotateZ(objectToAnimate, endingRotation, durationTime).setEase(easeType).setDelay(delayTime).setOnComplete(TweenComplete);
            break;
            case TweenIn.scale:
                animationTween = LeanTween.scale(objectToAnimate, new Vector3(endingScale, endingScale, 1), durationTime).setEase(easeType).setDelay(delayTime).setOnComplete(TweenComplete);
            break;
        }

        // Do you want to loop?
        if (doLoop) {
             animationTween.setRepeat(numberLoop);
         }
         else {
             animationTween.setLoopOnce();
        }

        // Do you want a badass fade?
        if (doFade) {
            LeanTween.alpha(objectToAnimate.GetComponent<RectTransform>(), endingAlpha, durationTime).setEase(easeType).setDelay(delayTime);
        }

        isTweening = true;
    }


    public void TweenOut()
    {
        // Assign animation.
        switch(tweenOptions)
        {
            case TweenIn.move:
                animationTween = LeanTween.moveLocal(objectToAnimate, startingPosition, durationTime).setEase(easeoutType).setDelay(delayTime).setOnComplete(TweenComplete);
            break;
            case TweenIn.rotateX:
                animationTween = LeanTween.rotateX(objectToAnimate, startingRotation, durationTime).setEase(easeoutType).setDelay(delayTime).setOnComplete(TweenComplete);
            break;
            case TweenIn.rotateY:
                animationTween = LeanTween.rotateY(objectToAnimate, startingRotation, durationTime).setEase(easeoutType).setDelay(delayTime).setOnComplete(TweenComplete);
            break;
            case TweenIn.rotateZ:
                animationTween = LeanTween.rotateZ(objectToAnimate, startingRotation, durationTime).setEase(easeoutType).setDelay(delayTime).setOnComplete(TweenComplete);
            break;
            case TweenIn.scale:
                animationTween = LeanTween.scale(objectToAnimate, new Vector3(startingScale, startingScale, 1), durationTime).setEase(easeoutType).setDelay(delayTime).setOnComplete(TweenComplete);
            break;
        }

        // Do you want to loop?
        if (doLoop) {
             animationTween.setRepeat(-1);
         }
         else {
             animationTween.setLoopOnce();
        }

        // Do you want a badass fade?
        if (doFade) {
            LeanTween.alpha(objectToAnimate.GetComponent<RectTransform>(), startingAlpha, durationTime).setEase(easeoutType).setDelay(delayTime);
        }

        isTweening = true;
    }


    public void TweenComplete() 
    {
        if (tweenedIn == false){
            isTweening =  false;
            tweenedIn = true;
        }
        else {
            isTweening = false;
            tweenedIn = false;
        }
    }


    // Class system
    // THIS IS EXPERIMENTAL. IGNORE FOR NOW!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //

    [System.Serializable]
    public class TweenObject {
        public enum TweenIn { move, rotateX, rotateY, rotateZ, scale, }
        public string animationTitle;
        public GameObject objectToAnimate;
        public TweenIn tweenIn;

        [Header("Time")]
        public float durationTime;
        public float delayTime = 0;

        [Header("Parameters")]
        public Vector3 startingPosition;
        public Vector3 endingPosition;
        public float startingScale = 1;
        public float endingScale = 1;
        public float startingRotation = 0;
        public float endingRotation;

        [Header("Add Ons")]
        public LeanTweenType easeType;
        public bool doLoop = false;
        public int numberLoop;
        public bool doFade = false;
        public float endingFade;

        [Header("Return")]
        public bool animateOut;

        LTDescr animationTween;
    }

    public List<TweenObject> tweenObject = new List<TweenObject>();

    public void ClassTween (int whichTween) {
        LeanTween.moveLocal(tweenObject[whichTween].objectToAnimate, tweenObject[whichTween].endingPosition, tweenObject[whichTween].durationTime);
    }
}
