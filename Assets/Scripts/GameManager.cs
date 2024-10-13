using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using System;
using Tweens;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


public enum TweenExpressionType{
    Normal,
    Jump,
    SideStep,
    HappyTalk,
}
public enum GameOverType{
    Normal,
    Banned,
    Shutdown,
}
public class Hack{
    public Action hackFunction;
    public float stressValue;
    public float chaosValue;
    public float duration;
    public Hack(Action hackFunction, float stressValue = 0f, float chaosValue = 0f, float duration = 0f){
        this.hackFunction = hackFunction;
        this.stressValue = stressValue;
        this.chaosValue = chaosValue;
        this.duration = duration;
    }
}
public struct ReimuAction {
    public string chatText;
    public int expressionIndex;
    public TweenExpressionType tweenType;
    public ReimuAction(string chatText, int expression, TweenExpressionType tweenType){
        this.chatText = chatText;
        this.expressionIndex = expression;
        this.tweenType = tweenType;
    }

}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] public CinemachineVirtualCamera gameCamera;
    private CinemachineBasicMultiChannelPerlin gameCameraNoise;

    [SerializeField] public CinemachineVirtualCamera kappaCamera;
    private CinemachineBasicMultiChannelPerlin kappaCameraNoise;

    [Header("Hacks")]
    [SerializeField] private SpriteRenderer fakePlayerSprite;
    [SerializeField] private SpriteRenderer fakeObstacleSprite;
    [SerializeField] private SpriteRenderer backdropSprite;

    [SerializeField] private FakeObstacleMove fakeObstacleMove;
    [SerializeField] private GameObject adWindowPrefab;
    [SerializeField] private GameObject hackHorrorPanel;
    [SerializeField] private GameObject hackR18Panel;
    [SerializeField] private GameObject hackFlashPanel;
    [SerializeField] private Volume gameLocalPPVolume;
    private Bloom bloomLayer;
    [Header("UI Hacks")]
    [SerializeField] private GameObject reimuCam;
    [SerializeField] private Image reimuSprite;
    [SerializeField] private TextMeshProUGUI reimuChatBubble;
    [SerializeField] private TextMeshProUGUI reimuSubCount;
    [SerializeField] private Sprite[] reimuExpressions;
    [SerializeField] private GameObject chatLog;
    [SerializeField] private GameObject chatBlockPrefab;

    [Header("Fake Player HP UI")]
    [SerializeField] private GameObject hpPanel;
    [SerializeField] private GameObject hpUiIconPrefab;
    [Header("Kappa UI")]
    [SerializeField] private GameObject hackCardPrefab;
    [SerializeField] private GameObject[] hackCardPositions = new GameObject[3];
    [SerializeField] private TextMeshPro stressValue;
    [SerializeField] private TextMeshPro chaosValue;
    [SerializeField] private GameObject kappaOSText;
    [SerializeField] private GameObject _BlackScreen;
    [SerializeField] private GameObject KAPPAOS_MONITOR;
    [SerializeField] private GameObject BIGHAMD;
    [SerializeField] private TextMeshPro scoreTEXT;
    [SerializeField] private GameObject retryPanel;


    public float Stress = 0;
    public float Chaos = 0;
    [Header("Game Init")]
    public GameObject startHackingBtn = null;
    public Parallax bg1, bg2;

    private int fakePlayerHP = 3;
    private int subcount = 11;

    private Coroutine gameCameraShakeCoroutine;
    private Coroutine stressChaosCoroutine;
    void Awake(){
        if (instance == null){
            instance = this;
        }else if (instance != this){
            Destroy(gameObject);
        }
        gameLocalPPVolume.profile.TryGet(out bloomLayer);
    }
    void Start()
    {
        gameCameraNoise = gameCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        kappaCameraNoise = kappaCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        AddHacks();
    }

    void Update(){
        if (Stress > 0 && stressChaosCoroutine == null){
            stressChaosCoroutine = StartCoroutine(DecrementStressChaos());
        }

        //Test
        if (Input.GetKeyDown(KeyCode.S)){
            InitHackSelection();
        }
        if (Input.GetKeyDown(KeyCode.D)){
            SingleLineDialogue(0, "こん霊夢です");
            TweenExpression(TweenExpressionType.HappyTalk);
        }
        if (Input.GetKeyDown(KeyCode.F)){
            AddChatBlock(HackReactions.NormalReactions);
        }
    }

#region Big boy functions
    //--------------------------------------------------------------------------------
    public void WatchStream(){
        
        
        var moveToHeaderTween = new AnchoredPositionYTween{
            from = 4,
            to = 9,
            duration = 0.2f,
            easeType = EaseType.SineInOut,
        };
        var scaleTween = new LocalScaleTween{
            from = new Vector3(10f, 10f, 1f),
            to = new Vector3(2.5f, 2.5f, 1f),
            duration = 0.2f,
            easeType = EaseType.SineInOut,
        };
        kappaOSText.AddTween(moveToHeaderTween);
        kappaOSText.AddTween(scaleTween);

        _BlackScreen.SetActive(false);
        Invoke("StartDelayedGame", 1f);
    }
    public void StartDelayedGame(){
        StartReimu();
        StartCoroutine(AutoChatBlock());
        UpdateStressChaosUI();
    }
    public void StartReimu()
    {
        GoThroughDialogue(ReimuReactions.GreetingReactions);
        Invoke("DelayedStartBtn", 16f);
    }
    
    public void DelayedStartBtn(){
        AudioManager.amInstance.PlayBGM("bgm");
        UpdateHPUI();
        fakeObstacleMove.moveSpeed = 5f;
        bg1.moveSpeed = 3f;
        bg2.moveSpeed = 2f;
        
        startHackingBtn.SetActive(true);
    }
    public void StartHacking(){
        InitHackSelection();
        StartCoroutine(SubCountIncrement());
    }

    //--------------------------------------------------------------------------------
    public void UpdateHPUI(){
        if(hpPanel.transform.childCount > 0){
            if (hpPanel.transform.childCount > fakePlayerHP){
                for(int x = 0; x < hpPanel.transform.childCount - fakePlayerHP; x++){
                    Destroy(hpPanel.transform.GetChild(x).gameObject);
                }
            }
            else if (hpPanel.transform.childCount < fakePlayerHP){
                for(int x = 0; x < fakePlayerHP - hpPanel.transform.childCount; x++){
                    GameObject newHpIcon = Instantiate(hpUiIconPrefab, hpPanel.transform);
                }
            }
        }else{
            for(int x = 0; x < fakePlayerHP; x++){
                GameObject newHpIcon = Instantiate(hpUiIconPrefab, hpPanel.transform);
            }
        }
    }
    public void FakePlayerHitObstacle(){
        fakePlayerHP -= 1;
        UpdateHPUI();
        if (fakePlayerHP <= 0){
            GameOver();
        }
    }
    public void GameOver(GameOverType gameOverType = GameOverType.Normal){
        StopAllCoroutines();
        AudioManager.amInstance.StopBGM();
        switch(gameOverType){
            case GameOverType.Normal:
                break;
            case GameOverType.Banned:
                break;
            case GameOverType.Shutdown:
                break;
        }
        BIGHAMD.SetActive(true);
        ShakeKappaCamera(0.5f);
        var pulldownTween = new LocalPositionYTween{
            to = -15,
            duration = 1f,
            easeType = EaseType.SineInOut,
            onEnd = (instance) => {
                AudioManager.amInstance.PlaySF("plyerDeath");
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                retryPanel.SetActive(true);
                scoreTEXT.text = "登録者数: " + subcount.ToString();
            }
        };
        KAPPAOS_MONITOR.AddTween(pulldownTween);
        
        
    }
    public void Retry(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SetStressChaos(){
        var popTween = new LocalScaleTween{
            from = new Vector3(1f, 1f, 0f),
            to = new Vector3(1.2f, 1.2f, 0f),
            usePingPong = true,
            duration = 0.1f,
            easeType = EaseType.SineInOut,
        };
        stressValue.gameObject.AddTween(popTween);
        chaosValue.gameObject.AddTween(popTween);

        Stress += hackStress;
        Chaos += hackChaos;

        UpdateStressChaosUI();
    }
    public IEnumerator DecrementStressChaos(){
        while (Stress > 0){
            Stress -= 1;
            Chaos -= 1;

            UpdateStressChaosUI();

            yield return new WaitForSeconds(3f);
        }

        Stress = 0;
        Chaos = 0;
        stressChaosCoroutine = null;
    }
    public void UpdateStressChaosUI(){
        stressValue.text = "<color=" + ((Stress > 66f)?"red":((Stress > 33f)?"yellow":"green")) + ">" + Stress + "%</color>";
        chaosValue.text = "<color=" + ((Chaos > 66f)?"green":((Chaos > 33f)?"yellow":"red")) + ">" + Chaos + "%</color>";
    }


    public void ShakeGameCamera(float duration){
        if (gameCameraShakeCoroutine != null){
            StopCoroutine(gameCameraShakeCoroutine);
        }
       gameCameraShakeCoroutine = StartCoroutine(ShakeGameCameraCoroutine(duration));
    }

    private IEnumerator ShakeGameCameraCoroutine(float duration){
        gameCameraNoise.m_AmplitudeGain = 5f;
        gameCameraNoise.m_FrequencyGain = 0.2f;

        yield return new WaitForSeconds(duration);

        gameCameraNoise.m_AmplitudeGain = 0f;
        gameCameraNoise.m_FrequencyGain = 0f;

        gameCameraShakeCoroutine = null;
    }
    public void ShakeKappaCamera(float duration){
        if (gameCameraShakeCoroutine != null){
            StopCoroutine(gameCameraShakeCoroutine);
        }
         gameCameraShakeCoroutine = StartCoroutine(ShakeKappaCameraCoroutine(duration));
    }
    private IEnumerator ShakeKappaCameraCoroutine(float duration){
        kappaCameraNoise.m_AmplitudeGain = 5f;
        kappaCameraNoise.m_FrequencyGain = 0.2f;

        yield return new WaitForSeconds(duration);

        kappaCameraNoise.m_AmplitudeGain = 0f;
        kappaCameraNoise.m_FrequencyGain = 0f;
    }

    private IEnumerator SubCountIncrement(){
        while (true){
            subcount += (int)(1 * Chaos);
            reimuSubCount.text = subcount.ToString() + "人";
            yield return new WaitForSeconds(5f);
        }
    }
#endregion
#region Chat
    //--------------------------------------------------------------------------------
    private Queue<GameObject> chatBlocks = new Queue<GameObject>();
    public void AddChatBlock(string[] textList){
        if (chatBlocks.Count >= 20){
            Destroy(chatBlocks.Dequeue());
        }

        string chatText = textList[UnityEngine.Random.Range(0, textList.Length)];
        string resultString = "<color=grey>" + HackReactions.Usernames[UnityEngine.Random.Range(0, HackReactions.Usernames.Length)] + "</color>: " + chatText;
        GameObject newChatBlock = Instantiate(chatBlockPrefab, chatLog.transform);
        newChatBlock.GetComponentInChildren<TextMeshProUGUI>().text = resultString;
        chatBlocks.Enqueue(newChatBlock);
    }
    public IEnumerator AutoChatBlock(){
        while (true){
            AddChatBlock(HackReactions.NormalReactions);
            yield return new WaitForSeconds(4f);
        }
    }

#endregion
#region REIMU EXPRESSION
    //--------------------------------------------------------------------------------
  
    private Coroutine dialogueCoroutine;
    private Coroutine chatBubbleCoroutine;
    private TweenInstance reimuSpriteTweenInstance;


    public void GoThroughDialogue(ReimuAction[] dialogue, float interval = 2f){
        if (dialogueCoroutine != null){
            StopCoroutine(dialogueCoroutine);
        }
        dialogueCoroutine = StartCoroutine(DialogueCoroutine(dialogue, interval));
    }
    private IEnumerator DialogueCoroutine(ReimuAction[] rAction, float interval){
        for (int x = 0; x < rAction.Length; x++){
            SingleLineDialogue(rAction[x].expressionIndex, rAction[x].chatText);
            if (rAction[x].tweenType != TweenExpressionType.Normal)
                TweenExpression(rAction[x].tweenType);
            yield return new WaitForSeconds(interval);
        }
        dialogueCoroutine = null;

        // Start idle dialogue
        GoThroughDialogue(ReimuReactions.NormalReactions, 8f);
    }
    public void SingleLineDialogue(int spriteExpressionIndex, string chatText= "。。。", bool interrupt = false){
        if (interrupt){
            if (dialogueCoroutine != null){
                StopCoroutine(dialogueCoroutine);
            }
        }

        reimuSprite.sprite = reimuExpressions[spriteExpressionIndex];
        if (chatBubbleCoroutine != null){
            StopCoroutine(chatBubbleCoroutine);
        }
        
        chatBubbleCoroutine = StartCoroutine(TypeChatBubble(chatText));
    }
    public IEnumerator TypeChatBubble(string chatText){
        string currentText = "";
        for (int x = 0; x < chatText.Length; x++){
            currentText += chatText[x];
            reimuChatBubble.text = currentText;
            AudioManager.amInstance.PlaySF("dBeep");
            yield return new WaitForSeconds(0.05f);
        }

        chatBubbleCoroutine = null;
    }
    public void ResetReimuTween(){
        reimuCam.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -25);
        reimuSpriteTweenInstance = null;
    }
    public void TweenExpression(TweenExpressionType tweenType)
    {
        if (reimuSpriteTweenInstance != null){
            reimuSpriteTweenInstance.Cancel();
        }
        switch(tweenType){
            case TweenExpressionType.Jump:
                var jumpTween = new AnchoredPositionYTween {
                    from = -25,
                    to = 25,
                    duration = 0.1f,
                    usePingPong = true,
                    easeType = EaseType.SineInOut,
                    onEnd = (instance) => {ResetReimuTween();}
                };
                reimuSpriteTweenInstance = reimuCam.AddTween(jumpTween);
                break;
            case TweenExpressionType.HappyTalk:
                var happyTalkTween = new AnchoredPositionYTween {
                    from = -25,
                    to = 25,
                    duration = 0.2f,
                    usePingPong = true,
                    easeType = EaseType.SineInOut,
                    loops = 2,
                    onEnd = (instance) => {ResetReimuTween();}
                };
                reimuSpriteTweenInstance = reimuCam.AddTween(happyTalkTween);
                break;
            case TweenExpressionType.SideStep:
                var sideStepTween = new AnchoredPositionXTween {
                    from = -100,
                    to = 100,
                    duration = 0.05f,
                    usePingPong = true,
                    easeType = EaseType.QuintInOut,
                    loops = 20,
                    onEnd = (instance) => {ResetReimuTween();}
                };
                reimuSpriteTweenInstance = reimuCam.AddTween(sideStepTween);
                break;
        }
    }
#endregion
#region HACKS
        //--------------------------------------------------------------------------------

    private Dictionary<string, Hack> hacks = new Dictionary<string, Hack>();
    private GameObject[] cardsOnDisplay = new GameObject[3]; 
    private string[] hackTypes = new string[]{
        "加速",
        "ホラー",
        "広告",
        "R18",//td
        "キラキラ",
        "入り代わり",
        "顔ばれ",
        //"Shutdown",//td
        //"住所ばれ",//td
        "HP改造",//td
        "ノックノック"//td
    };
    private void AddHacks(){
        hacks.Add("加速", new Hack(Hack_HastenObstacle, 5f, 5f));
        hacks.Add("ホラー", new Hack(Hack_Horror, 20f, 5f));
        hacks.Add("広告", new Hack(Hack_Advertisement, 10f, 20f));
        hacks.Add("R18", new Hack(Hack_R18, 30f, 30f));
        hacks.Add("キラキラ", new Hack(Hack_Flash, 50f, 40f));
        hacks.Add("入り代わり", new Hack(Hack_SpriteChange, 10f, 10f));
        hacks.Add("顔ばれ", new Hack(Hack_Kaobare, 50f, 40f));
        //hacks.Add("Shutdown", new Hack(Hack_Shutdown, 100f, 100f));
        //hacks.Add("住所ばれ", new Hack(Hack_AddressReveal, 30f, 30f));
        hacks.Add("HP改造", new Hack(Hack_HPChange, 10f, 10f));
        hacks.Add("ノックノック", new Hack(Hack_KnockKnock, 30f, 10f));
    }
    public void InitHackSelection(){
        if (cardsOnDisplay[0] != null){
            DestroyCards();
        }
        for (int x = 0; x < hackCardPositions.Length; x++){
            string randomHack = hackTypes[UnityEngine.Random.Range(0, hackTypes.Length)];
            GameObject newCard = Instantiate(hackCardPrefab, hackCardPositions[x].transform);
            newCard.name = randomHack;
            newCard.GetComponentInChildren<TextMeshPro>().text = randomHack;
            cardsOnDisplay[x] = newCard;
        }
    }
    public void DestroyCards(){
        for (int x = 0; x < cardsOnDisplay.Length; x++){
            Destroy(cardsOnDisplay[x]);
        }
    }
    public void CallHack(string hackName){
        if (hacks.ContainsKey(hackName)){
            Debug.Log("Calling hack: " + hackName);
            hackStress = hacks[hackName].stressValue;
            hackChaos = hacks[hackName].chaosValue;
            hacks[hackName].hackFunction();
        }else{
            Debug.LogError("Hack not found: " + hackName);
        }
        DestroyCards();
    }
    public void EndHack(){
        ReimuAction randomReturnLine = ReimuReactions.ReturnLines[UnityEngine.Random.Range(0, ReimuReactions.ReturnLines.Length)];
        SingleLineDialogue(randomReturnLine.expressionIndex, randomReturnLine.chatText);
        InitHackSelection();
    }


    // Hacks ---------------------------------------------------------------------
    float hackStress = 0;
    float hackChaos = 0;
    // ################################################### Hasten Obstacle ###################################################
    public void Hack_HastenObstacle()
    {
        StartCoroutine(HackHastenObstacleCoroutine());
    }
    private IEnumerator HackHastenObstacleCoroutine(){
        fakeObstacleMove.SetNewConfig();
        yield return new WaitForSeconds(1f);
        GoThroughDialogue(ReimuReactions.SpeedUPReactions);
        yield return new WaitForSeconds(ReimuReactions.SpeedUPReactions.Length * 2f);
        SetStressChaos();
        EndHack();
    }
    // ################################################### Horror ###################################################
    public void Hack_Horror(){
        StartCoroutine(HackHorrorCoroutine());
    }
    private IEnumerator HackHorrorCoroutine(){
        AudioManager.amInstance.StopBGM();
        yield return new WaitForSeconds(3f);
        SingleLineDialogue( 0, "あれ？なんか静かになってない？");
        yield return new WaitForSeconds(3f);
        hackHorrorPanel.SetActive(true);
        AudioManager.amInstance.PlaySF("scream");
        GoThroughDialogue(ReimuReactions.HorrorReactions);
        
        ShakeGameCamera(0.1f);
        SetStressChaos();
        yield return new WaitForSeconds(1f);
        hackHorrorPanel.SetActive(false);
        yield return new WaitForSeconds(ReimuReactions.HorrorReactions.Length * 2f);
        AudioManager.amInstance.PlayBGM("bgm");
        EndHack();

    }
    // ################################################### Advertisement ###################################################
    public void Hack_Advertisement(){
        StartCoroutine(HackAdvertisementCoroutine());
    }
    private IEnumerator HackAdvertisementCoroutine(){
        Stack<GameObject> adWindows = new Stack<GameObject>();

        yield return new WaitForSeconds(1f);
        for (int x = 0; x < 15; x++){
            //Spawn ad prefab around the screen
            GameObject newAd = Instantiate(adWindowPrefab, new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-5f, 5f), 0f), Quaternion.identity);
            adWindows.Push(newAd);
            yield return new WaitForSeconds(0.1f);
        }
        SetStressChaos();
        yield return new WaitForSeconds(1f);
        GoThroughDialogue(ReimuReactions.AdWindowReactions);
        yield return new WaitForSeconds(ReimuReactions.AdWindowReactions.Length * 2f);
        
        for (int x = 0; x < 15; x++){
            if (adWindows.Count > 0){
                Destroy(adWindows.Pop());
            }
            yield return new WaitForSeconds(0.3f);
        }
        
        EndHack();
    }
    // ################################################### R18 ###################################################
    public void Hack_R18(){
        StartCoroutine(HackR18Coroutine());
    }
    private IEnumerator HackR18Coroutine(){
        yield return new WaitForSeconds(1f);
        hackR18Panel.SetActive(true);
        GoThroughDialogue(ReimuReactions.R18Reactions);
        yield return new WaitForSeconds(ReimuReactions.R18Reactions.Length * 2f);
        GameOver(GameOverType.Banned);
    }
    // ################################################### Flash ###################################################
    public void Hack_Flash(){
        StartCoroutine(HackFlashCoroutine());
    }
    private IEnumerator HackFlashCoroutine(){
        yield return new WaitForSeconds(1f);
        hackFlashPanel.SetActive(true);
        bloomLayer.intensity.value = 30f;
        GoThroughDialogue(ReimuReactions.FlashReactions);
        yield return new WaitForSeconds(1f);
        
        float localTimer = 0f;
        float durationLerp = 5f;
        while (localTimer < durationLerp)
        {
            localTimer += Time.deltaTime;
            bloomLayer.intensity.value = Mathf.Lerp(30f, 0f, localTimer / durationLerp);
            hackFlashPanel.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, localTimer / 5f));
            yield return null;
        }
        hackFlashPanel.SetActive(false);
        hackFlashPanel.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        SetStressChaos();
        yield return new WaitForSeconds(ReimuReactions.FlashReactions.Length * 2f);
        EndHack();
    }
    // ################################################### SpriteChange ###################################################
    public void Hack_SpriteChange(){
        StartCoroutine(HackSpriteChangeCoroutine());
    }
    private IEnumerator HackSpriteChangeCoroutine(){
        yield return new WaitForSeconds(1f);
        fakePlayerSprite.sprite = reimuExpressions[2];
        fakeObstacleSprite.sprite = reimuExpressions[2];
        GoThroughDialogue(ReimuReactions.SpriteSwapReactions);
        yield return new WaitForSeconds(1f);
        backdropSprite.sprite = reimuExpressions[2];

        yield return new WaitForSeconds(ReimuReactions.SpriteSwapReactions.Length * 2f);
        EndHack();
    }
    // ################################################### Kaobare ###################################################
    public void Hack_Kaobare(){
        StartCoroutine(HackKaobareCoroutine());
    }
    private IEnumerator HackKaobareCoroutine(){
        yield return new WaitForSeconds(1f);
        GoThroughDialogue(ReimuReactions.KaobareReactions);
        yield return new WaitForSeconds(2f);
        SetStressChaos();
        yield return new WaitForSeconds(ReimuReactions.KaobareReactions.Length * 2f - 2f);
        EndHack();
    }
    // ################################################### Shutdown ###################################################
    // public void Hack_Shutdown(){
    //     StartCoroutine(HackShutdownCoroutine());
    // }
    // private IEnumerator HackShutdownCoroutine(){
    //     yield return new WaitForSeconds(1f);
    // }
    // ################################################### AddressReveal ###################################################
    // public void Hack_AddressReveal(){
    //     StartCoroutine(HackAddressRevealCoroutine());
    // }
    // private IEnumerator HackAddressRevealCoroutine(){
    //     yield return new WaitForSeconds(1f);
    // }
    // ################################################### HPChange ###################################################
    public void Hack_HPChange(){
        StartCoroutine(HackHPChangeCoroutine());
    }
    private IEnumerator HackHPChangeCoroutine(){
        int oldHP = fakePlayerHP;
        fakePlayerHP = UnityEngine.Random.Range(1, 11);
        UpdateHPUI();
        yield return new WaitForSeconds(1f);
        if  (fakePlayerHP < oldHP){
            GoThroughDialogue(ReimuReactions.HPChangeReactionsBad);

            yield return new WaitForSeconds(ReimuReactions.HPChangeReactionsBad.Length * 2f);
        }else{
            GoThroughDialogue(ReimuReactions.HPChangeReactionsGood);
            hackStress = -10;
            yield return new WaitForSeconds(ReimuReactions.HPChangeReactionsGood.Length * 2f);
        }
        SetStressChaos();
        EndHack();
        
    }
    // ################################################### KnockKnock ###################################################       
    public void Hack_KnockKnock(){
        StartCoroutine(HackKnockKnockCoroutine());
    }
    private IEnumerator HackKnockKnockCoroutine(){
        yield return new WaitForSeconds(1f);
        AudioManager.amInstance.PlaySF("knock");
        yield return new WaitForSeconds(1f);
        GoThroughDialogue(ReimuReactions.KnockKnockReactions);
        yield return new WaitForSeconds(ReimuReactions.KnockKnockReactions.Length * 2f);
        SetStressChaos();
        EndHack();
    }

#endregion
}
