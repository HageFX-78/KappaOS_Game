using System.Collections;
using System.Collections.Generic;
using Tweens;
using Tweens.Core;
using UnityEngine;

public class HackButton : MonoBehaviour
{
    private TweenInstance scaleUpTweenInstance;
    private TweenInstance scaleDownTweenInstance;

    private TweenInstance clickTweenInstance;
    private Vector3 defaultScale = new Vector3(5f, 5f, 5f);
    private Vector3 hoverScale = new Vector3(5.1f, 5.1f, 5.1f);
    private Vector3 clickScale = new Vector3(4.5f, 4.5f, 4.5f);

    public bool IsStartHack = false;
    public bool IsWatchButton = false;
    public bool ISRETRY = false;
    public bool isEndGame = false;

    public bool guideBook = false;

    void Start()
    {
        defaultScale = transform.localScale ;
        var popupTween = new LocalPositionYTween
        {
            from = transform.localPosition.y - 3,
            to = transform.localPosition.y,
            duration = 0.3f,
            easeType = EaseType.ElasticOut
        };
        gameObject.AddTween(popupTween);
        clickScale = defaultScale * 0.9f;

    }

    void OnMouseEnter()
    {
        StartScaleTween(defaultScale * 1.1f, ref scaleUpTweenInstance, ref scaleDownTweenInstance, 0.1f);
    }

    void OnMouseExit()
    {
        StartScaleTween(defaultScale, ref scaleDownTweenInstance, ref scaleUpTweenInstance, 0.1f);
    }

    void OnMouseDown()
    {
        

        if (clickTweenInstance != null)
            return;
        AudioManager.amInstance.PlaySF("keyboard");
        
        // Cancel both existing tweens and create a ping-pong effect for the click
        CancelTweens();
        var tween = new LocalScaleTween
        {
            to = clickScale,
            duration = 0.1f,
            usePingPong = true,
            easeType = EaseType.SineInOut,
            onEnd = (instance) => { 
                clickTweenInstance = null;

                if (isEndGame)
                {
                    Application.Quit();
                    return;
                }

                if (guideBook)
                {
                    GameManager.instance.ShowGuideBook();
                    return;
                }
                if (ISRETRY)
                {
                    GameManager.instance.Retry();
                    return;
                }
                if (IsWatchButton)
                {
                    GameManager.instance.WatchStream();
                    Destroy(gameObject);
                }
                else if (!IsStartHack)
                    GameManager.instance.CallHack(gameObject.name);
                else{
                    GameManager.instance.StartHacking();
                    Destroy(gameObject);
                }
                    
            }
        };
        clickTweenInstance = gameObject.AddTween(tween);
    }

    private void StartScaleTween(Vector3 targetScale, ref TweenInstance tweenToStart, ref TweenInstance tweenToCancel, float duration)
    {
        // Cancel any active tween that is being replaced
        tweenToCancel?.Cancel();

        // Set up and start the new tween
        var tween = new LocalScaleTween
        {
            to = targetScale,
            duration = duration,
            easeType = EaseType.QuintInOut
        };
        tweenToStart = gameObject.AddTween(tween);
    }

    private void CancelTweens()
    {
        scaleUpTweenInstance?.Cancel();
        scaleDownTweenInstance?.Cancel();
    }
}
