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
using System.Linq;

// Whoever reads this, dont look below. It's a mess. I'm sorry.

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
    public Hack(Action hackFunction, float chaosValue = 0f, float stressValue = 0f,  float duration = 0f){
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
    public float duration;
    public ReimuAction(string chatText, int expression, TweenExpressionType tweenType, float duration = 2f){
        this.chatText = chatText;
        this.expressionIndex = expression;
        this.tweenType = tweenType;
        this.duration = duration;
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
    [SerializeField] private GameObject shutdownPanel;
    [SerializeField] private GameObject bannedStreamPanel;
    [SerializeField] private GameObject streamEndPanel;

    [SerializeField] Sprite[] adSprites;

    [Header("Reimu")]
    [SerializeField] AudioClip[] reimuSounds;
    private Bloom bloomLayer;
    [Header("UI Hacks")]
    [SerializeField] private GameObject reimuCam;
    [SerializeField] private Image reimuSprite;
    [SerializeField] private TextMeshProUGUI reimuChatBubble;
    [SerializeField] private TextMeshProUGUI reimuSubCount;
    [SerializeField] private Sprite[] reimuExpressions;
    [SerializeField] private GameObject chatLog;
    [SerializeField] private GameObject chatBlockPrefab;
    [SerializeField] private GameObject akaSupaPrefab;

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

        //Test
        if(Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.S)){
            InitHackSelection();
            }
            if (Input.GetKeyDown(KeyCode.D)){
                //StartReimu();
                StartReimuInGame();
                StartCoroutine(AutoChatBlock());
            }
            if (Input.GetKeyDown(KeyCode.F)){
                AddChatBlock(HackReactions.NormalReactions);
            }
        }
        
    }

#region KEY FUNCTIONS
    public void WatchStream(){
        // FIRST
        AudioManager.amInstance.PlaySF("startup");
        
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
        Invoke("StartReimuInGame", 16f);
    }
    
    public void StartReimuInGame(){
        AudioManager.amInstance.PlayBGM("bgm");
        UpdateHPUI();
        fakeObstacleMove.moveSpeed = 5f;
        bg1.moveSpeed = 3f;
        bg2.moveSpeed = 2f;
        
        startHackingBtn.SetActive(true);
         UpdateStressChaosUI();
         stressChaosCoroutine = StartCoroutine(SCPassiveModifier());
    }
    public void StartHacking(){
        InitHackSelection();
        StartCoroutine(SubCountIncrement());
    }
    public void GameOver(GameOverType gameOverType = GameOverType.Normal){
        StopAllCoroutines();
        StopGame();
        AudioManager.amInstance.StopBGM();
        switch(gameOverType){
            case GameOverType.Normal:
                break;
            case GameOverType.Banned:
                bannedStreamPanel.SetActive(true);
                AudioManager.amInstance.StopBGM();
                break;
            case GameOverType.Shutdown:
                break;
        }
        StartCoroutine(GameOverCoroutine());
    }
    public IEnumerator GameOverCoroutine(){
        yield return new WaitForSeconds(1f);
        BIGHAMD.SetActive(true);
        AudioManager.amInstance.PlaySF("hand");
        ShakeKappaCamera(0.2f);
        yield return new WaitForSeconds(1f);
        AudioManager.amInstance.PlaySF(reimuSounds[UnityEngine.Random.Range(0, reimuSounds.Length)].name);
        yield return new WaitForSeconds(1f);
        var pulldownTween = new LocalPositionYTween{
            to = -15,
            duration = 1f,
            easeType = EaseType.SineInOut,
            onEnd = (instance) => {
                AudioManager.amInstance.PlaySF("plyerDeath");

                retryPanel.SetActive(true);
                scoreTEXT.text = "登録者数: " + subcount.ToString();
            }
        };
        KAPPAOS_MONITOR.AddTween(pulldownTween);
    }
    public void Retry(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
#endregion

#region Big boy functions
    //--------------------------------------------------------------------------------\

    public void StopGame(){
        fakeObstacleMove.moveSpeed = 0;
        bg1.moveSpeed = 0;
        bg2.moveSpeed = 0;
        AudioManager.amInstance.StopBGM();
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

    public GameObject gb1, gb2;
    public GameObject canvas;
    public void ShowGuideBook(){
        gb1.SetActive(true);
        canvas.SetActive(true);
    }
    public void Setgb1(){
        gb1.SetActive(true);
        gb2.SetActive(false);
    }
    public void Setgb2(){
        gb1.SetActive(false);
        gb2.SetActive(true);
    }
    public void CLoseGuideBook(){
        canvas.SetActive(false);
    }

    
#endregion
#region Stress and Chaos

    [HideInInspector] public float AdditionalHackStressPercentage = 0; 
    private float PassiveStressModifier = 0.1f;
    private float PassiveChaosModifier = -0.5f;
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

        if (Stress >= 100){
            GameOver(GameOverType.Normal);
        }
        UpdateStressChaosUI();
    }
    public IEnumerator SCPassiveModifier(){
        while (true){
            Stress += PassiveStressModifier;
            Chaos += PassiveChaosModifier;

            if (Stress < 0){
                Stress = 0;
            }
            if (Chaos < 0){
                Chaos = 0;
            }

            UpdateStressChaosUI();

            yield return new WaitForSeconds(3f);
        }
    }
    public void AddPassiveStressChaos(float stress, float chaos){
        PassiveStressModifier += stress;
        PassiveChaosModifier += chaos;
    }
    public void UpdateStressChaosUI(){
        Stress = Mathf.Round(Stress * 10.0f) * 0.1f;
        Chaos = Mathf.Round(Chaos * 10.0f) * 0.1f;

        stressValue.text = "<color=" + ((Stress > 66f)?"red":((Stress > 33f)?"yellow":"green")) + ">" + Stress + "%</color>"
        + " \n<color=yellow><size=2>(" + PassiveStressModifier + "/s)</color>";
        chaosValue.text = "<color=" + ((Chaos > 66f)?"green":((Chaos > 33f)?"yellow":"red")) + ">" + Chaos + "%</color>"
        + " \n<color=yellow><size=2>(" + PassiveChaosModifier + "/s)</color>";
    }
#endregion
#region Sub count
    private bool subCountMark1Reached = false;//1k
    private bool subCountMark2Reached = false;//100k
    private bool subCountMark3Reached = false;//1m
    private IEnumerator SubCountIncrement(){
        
        while (true){
            subcount += (int)(1 * Chaos);
            if (subcount >= 1000 && !subCountMark1Reached){
                subCountMark1Reached = true;
                TriggerSubCountMark(0);
            }
            if (subcount >= 100000 && !subCountMark2Reached){
                subCountMark2Reached = true;
                TriggerSubCountMark(1);
            }
            if (subcount >= 1000000 && !subCountMark3Reached){
                subCountMark3Reached = true;
                TriggerSubCountMark(2);
            }
            reimuSubCount.text = subcount.ToString() + "人";
            yield return new WaitForSeconds(5f);
        }
    }

    public void TriggerSubCountMark(int markIndex){
        DestroyCards();
        if(PrimaryHackCoroutine != null){
            StopCoroutine(PrimaryHackCoroutine);
            PrimaryHackCoroutine = null;
        }
        switch(markIndex){
            case 0:
                GoThroughDialogue(ReimuReactions.SubCountMark1, true);
                break;
            case 1:

                GoThroughDialogue(ReimuReactions.SubCountMark100k, true);
                break;
            case 2:

                GoThroughDialogue(ReimuReactions.SubCountMark1M, true);
                break;
        }
    }
#endregion
#region Camera

    //--------------------------------------------------------------------------------
        public void ShakeGameCamera(float duration){
        if (gameCameraShakeCoroutine != null){
            StopCoroutine(gameCameraShakeCoroutine);
        }
       gameCameraShakeCoroutine = StartCoroutine(ShakeGameCameraCoroutine(duration));
    }

    private IEnumerator ShakeGameCameraCoroutine(float duration){
        gameCameraNoise.m_AmplitudeGain = 10f;
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
        kappaCameraNoise.m_AmplitudeGain = 10f;
        kappaCameraNoise.m_FrequencyGain = 0.2f;

        yield return new WaitForSeconds(duration);

        kappaCameraNoise.m_AmplitudeGain = 0f;
        kappaCameraNoise.m_FrequencyGain = 0f;
    }
#endregion
#region Chat
    //--------------------------------------------------------------------------------
    private Queue<GameObject> chatBlocks = new Queue<GameObject>();

    public void AddChatBlock(string chatText, bool isRedSupa = false){
        if (chatBlocks.Count >= 20){
            Destroy(chatBlocks.Dequeue());
        }

        GameObject newChatBlock;
        if (isRedSupa){
            newChatBlock = Instantiate(akaSupaPrefab, chatLog.transform);
        }
        else
        {    
            newChatBlock = Instantiate(chatBlockPrefab, chatLog.transform);
        }
        string resultString  = "<color=yellow>" + HackReactions.Usernames[UnityEngine.Random.Range(0, HackReactions.Usernames.Length)] + "</color>: " + chatText;
        newChatBlock.GetComponentInChildren<TextMeshProUGUI>().text = resultString;
        chatBlocks.Enqueue(newChatBlock);
    }
    public void AddChatBlock(string[] textList, bool isRedSupa = false){
        if (chatBlocks.Count >= 20){
            Destroy(chatBlocks.Dequeue());
        }

        string chatText = "";
        GameObject newChatBlock;
        if (isRedSupa){
            chatText = textList[0];
            newChatBlock = Instantiate(akaSupaPrefab, chatLog.transform);
        }
        else
        {    
            chatText = textList[UnityEngine.Random.Range(0, textList.Length)];
            newChatBlock = Instantiate(chatBlockPrefab, chatLog.transform);
        }
        string resultString = "<color=yellow>" + HackReactions.Usernames[UnityEngine.Random.Range(0, HackReactions.Usernames.Length)] + "</color>: " + chatText;
        newChatBlock.GetComponentInChildren<TextMeshProUGUI>().text = resultString;
        chatBlocks.Enqueue(newChatBlock);
    }
    public void AddChatBlockReaction(string[] theChatList) {
        StartCoroutine(AddRandomChatMessages(theChatList));
    }

    private IEnumerator AddRandomChatMessages(string[] theChatList) {
        // Randomly determine the number of messages to add (between 5 and 10)
        int numMessages = UnityEngine.Random.Range(5, 11);

        for (int i = 0; i < numMessages; i++) {
            // Randomly pick a message from the chat list
            string randomMessage = theChatList[UnityEngine.Random.Range(0, theChatList.Length)];

            AddChatBlock(randomMessage);

            // Wait for a random interval before adding the next message (between 0.5 and 1.5 seconds)
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.3f, 1.5f));
        }
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


    public void GoThroughDialogue(ReimuAction[] dialogue, bool callFromSpecialEvent = false){
        if (dialogueCoroutine != null){
            StopCoroutine(dialogueCoroutine);
        }
        dialogueCoroutine = StartCoroutine(DialogueCoroutine(dialogue, callFromSpecialEvent));
    }
    private IEnumerator DialogueCoroutine(ReimuAction[] rAction, bool callFromSpecialEvent){
        for (int x = 0; x < rAction.Length; x++){
            SingleLineDialogue(rAction[x].expressionIndex, rAction[x].chatText);
            if (rAction[x].tweenType != TweenExpressionType.Normal)
                TweenExpression(rAction[x].tweenType);
            yield return new WaitForSeconds(rAction[x].duration);
        }
        dialogueCoroutine = null;

        // // Start idle dialogue
        // GoThroughDialogue(ReimuReactions.NormalReactions);

        if (callFromSpecialEvent){
            EndHack();
        }
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

    private bool faceHasBeenRevealed = false;
    private Dictionary<string, Hack> hacks = new Dictionary<string, Hack>();
    private GameObject[] cardsOnDisplay = new GameObject[3]; 
    private Coroutine PrimaryHackCoroutine;
    private List<string> hackTypes = new List<string>{
        "加速",
        "ホラー",
        "広告",
        "R18",//td
        "キラキラ",
        "入り代わり",
        "顔ばれ",
        "Shutdown",//td
        "HP改造",//td
        "ノックノック",//td
        "あかすぱ",
        "まりさをよぶ"
    };
    private void AddHacks(){
        hacks.Add("加速", new Hack(Hack_HastenObstacle, 20f, 5f));
        hacks.Add("ホラー", new Hack(Hack_Horror, 50f, 5f));
        hacks.Add("広告", new Hack(Hack_Advertisement, 60f, 20f));
        hacks.Add("R18", new Hack(Hack_R18, 50f, 30f));
        hacks.Add("キラキラ", new Hack(Hack_Flash, 70f, 40f));
        hacks.Add("入り代わり", new Hack(Hack_SpriteChange, 10f, 10f));
        hacks.Add("顔ばれ", new Hack(Hack_Kaobare, 80f, 40f));
        hacks.Add("Shutdown", new Hack(Hack_Shutdown, 100f, 100f));
        hacks.Add("HP改造", new Hack(Hack_HPChange, 10f, 10f));
        hacks.Add("ノックノック", new Hack(Hack_KnockKnock, 40f, 10f));
        hacks.Add("あかすぱ", new Hack(Hack_RedSupa, 30f, 10f));
        hacks.Add("まりさをよぶ", new Hack(Hack_MarisaContact, 20f, 10f));
    }
    public void InitHackSelection(){
        if (cardsOnDisplay[0] != null){
            DestroyCards();
        }
        for (int x = 0; x < hackCardPositions.Length; x++){
            string randomHack = hackTypes[UnityEngine.Random.Range(0, hackTypes.Count)];
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
   public void EndHack()
    {
        ReimuAction randomReturnLine = ReimuReactions.ReturnLines[UnityEngine.Random.Range(0, ReimuReactions.ReturnLines.Length)];
        
        // Add randomReturnLine to the start of NormalReactions
        ReimuAction[] defaultPlusRand = new ReimuAction[] { randomReturnLine }.Concat(ReimuReactions.NormalReactions).ToArray();

        // Reinitiate idle dialogue with defaultPlusRand (which includes randomReturnLine)
        GoThroughDialogue(defaultPlusRand);
        
        InitHackSelection();

        if (PrimaryHackCoroutine != null){
            StopCoroutine(PrimaryHackCoroutine);
            PrimaryHackCoroutine = null;
        }
            
    }



    // Hacks ---------------------------------------------------------------------
    float hackStress = 0;
    float hackChaos = 0;
    // ################################################### Hasten Obstacle ###################################################
    public void Hack_HastenObstacle()
    {
        PrimaryHackCoroutine = StartCoroutine(HackHastenObstacleCoroutine());
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
        PrimaryHackCoroutine = StartCoroutine(HackHorrorCoroutine());
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
        AddChatBlockReaction(HackReactions.HorrorJumpscareReactions);
        yield return new WaitForSeconds(ReimuReactions.HorrorReactions.Length * 2f);
        AudioManager.amInstance.PlayBGM("bgm");
        EndHack();

    }
    // ################################################### Advertisement ###################################################
    public void Hack_Advertisement(){
        PrimaryHackCoroutine = StartCoroutine(HackAdvertisementCoroutine());
    }
    private IEnumerator HackAdvertisementCoroutine(){
        Stack<GameObject> adWindows = new Stack<GameObject>();

        yield return new WaitForSeconds(1f);
        for (int x = 0; x < 15; x++){
            //Spawn ad prefab around the screen
            AudioManager.amInstance.PlaySF("popup");
            GameObject newAd = Instantiate(adWindowPrefab, new Vector3(UnityEngine.Random.Range(-18f, 18f), UnityEngine.Random.Range(-3f, 10f), 0f), Quaternion.identity);
            newAd.GetComponent<SpriteRenderer>().sprite = adSprites[UnityEngine.Random.Range(0, adSprites.Length)];
            adWindows.Push(newAd);
            yield return new WaitForSeconds(0.1f);
        }
        AdditionalHackStressPercentage = 30;
        SetStressChaos();
        yield return new WaitForSeconds(1f);
        GoThroughDialogue(ReimuReactions.AdWindowReactions);
        yield return new WaitForSeconds(ReimuReactions.AdWindowReactions.Length * 2f);
        
        for (int x = 0; x < 15; x++){
            if (adWindows.Count > 0){
                AudioManager.amInstance.PlaySF("click");
                Destroy(adWindows.Pop());
            }
            yield return new WaitForSeconds(0.3f);
        }
        
        AdditionalHackStressPercentage = 0;
        EndHack();
    }
    // ################################################### R18 ###################################################
    public void Hack_R18(){
        PrimaryHackCoroutine = StartCoroutine(HackR18Coroutine());
    }
    private IEnumerator HackR18Coroutine(){
        yield return new WaitForSeconds(1f);
        hackR18Panel.SetActive(true);
        GoThroughDialogue(ReimuReactions.R18Reactions);
        yield return new WaitForSeconds(ReimuReactions.R18Reactions.Length * 2f);
        StopGame();

        yield return new WaitForSeconds(2f);
        GameOver(GameOverType.Banned);
    }
    // ################################################### Flash ###################################################
    public void Hack_Flash(){
        PrimaryHackCoroutine = StartCoroutine(HackFlashCoroutine());
    }
    private IEnumerator HackFlashCoroutine(){
        yield return new WaitForSeconds(1f);
        
        GoThroughDialogue(ReimuReactions.FlashReactions);
        yield return new WaitForSeconds(1f);
        hackFlashPanel.SetActive(true);
        bloomLayer.intensity.value = 30f;
        AudioManager.amInstance.PlaySF("flashbang");
        
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
        AddChatBlockReaction(HackReactions.FlashbangReactions);
        SetStressChaos();
        yield return new WaitForSeconds(ReimuReactions.FlashReactions.Length * 2f - 2f);
        EndHack();
    }
    // ################################################### SpriteChange ###################################################
    public void Hack_SpriteChange(){
        PrimaryHackCoroutine = StartCoroutine(HackSpriteChangeCoroutine());
    }
    private IEnumerator HackSpriteChangeCoroutine(){
        yield return new WaitForSeconds(1f);
        fakePlayerSprite.sprite = reimuExpressions[2];
        fakeObstacleSprite.sprite = reimuExpressions[2];
        GoThroughDialogue(ReimuReactions.SpriteSwapReactions);
        yield return new WaitForSeconds(1f);
        backdropSprite.sprite = reimuExpressions[2];
        AddPassiveStressChaos(0.1f, 0.1f);
        SetStressChaos();

        yield return new WaitForSeconds(ReimuReactions.SpriteSwapReactions.Length * 2f);
        EndHack();
    }
    // ################################################### Kaobare ###################################################
    public void Hack_Kaobare(){
        PrimaryHackCoroutine = StartCoroutine(HackKaobareCoroutine());
    }
    private IEnumerator HackKaobareCoroutine(){
        ReimuAction[] currentList = faceHasBeenRevealed?ReimuReactions.PostKaobareReactions:ReimuReactions.KaobareReactions;

        yield return new WaitForSeconds(1f);
        GoThroughDialogue(currentList);
        yield return new WaitForSeconds(2f);
        AddChatBlockReaction(HackReactions.FaceRevealReactions);
        SetStressChaos();
        yield return new WaitForSeconds(currentList.Length * 2f - 2f);

        faceHasBeenRevealed = true;
        EndHack();
    }
    // ################################################### Shutdown ###################################################
    public void Hack_Shutdown(){
        PrimaryHackCoroutine = StartCoroutine(HackShutdownCoroutine());
    }
    private IEnumerator HackShutdownCoroutine(){
        yield return new WaitForSeconds(1f);
        StopGame();
        shutdownPanel.SetActive(true);
        AudioManager.amInstance.PlaySF("shutdown");
        yield return new WaitForSeconds(1f);
        GoThroughDialogue(ReimuReactions.ShutdownReactions);
        yield return new WaitForSeconds(ReimuReactions.ShutdownReactions.Length * 2f - 2f);
        streamEndPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameOver(GameOverType.Shutdown);
    }
    // ################################################### AddressReveal ###################################################
    // public void Hack_AddressReveal(){
    //     StartCoroutine(HackAddressRevealCoroutine());
    // }
    // private IEnumerator HackAddressRevealCoroutine(){
    //     yield return new WaitForSeconds(1f);
    // }
    // ################################################### HPChange ###################################################
    public void Hack_HPChange(){
        PrimaryHackCoroutine = StartCoroutine(HackHPChangeCoroutine());
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
        PrimaryHackCoroutine = StartCoroutine(HackKnockKnockCoroutine());
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

    // ################################################### RedSupa ###################################################
    public void Hack_RedSupa(){
        PrimaryHackCoroutine = StartCoroutine(HackRedSupaCoroutine());
    }
    private IEnumerator HackRedSupaCoroutine(){
        string[] pickedSupa = HackReactions.AkaSupaList[UnityEngine.Random.Range(0, HackReactions.AkaSupaList.Count)];
        yield return new WaitForSeconds(1f);
        AudioManager.amInstance.PlaySF("supacha");
        AudioManager.amInstance.PlaySF(pickedSupa[1]);
        AddChatBlock(pickedSupa[0], true);
        yield return new WaitForSeconds(float.Parse(pickedSupa[2]));

        GoThroughDialogue(ReimuReactions.SupaReactions);
        SetStressChaos();
        yield return new WaitForSeconds(ReimuReactions.SupaReactions.Length * 2f + 2f);
        
        EndHack();
    }

    // ################################################### MarisaContact ###################################################
    private Coroutine MarisaCompanionCoroutine;
    [SerializeField] public AudioClip[] marisaSounds;
    public void Hack_MarisaContact(){
        if (MarisaCompanionCoroutine != null){

            AudioManager.amInstance.PlaySF("marisaExtra");
            EndHack();
            return;
        }
        PrimaryHackCoroutine = StartCoroutine(HackMarisaContactCoroutine());
    }
    private IEnumerator HackMarisaContactCoroutine(){
        AudioManager.amInstance.PlaySF("marisaKnock");
        yield return new WaitForSeconds(2f);
        AudioManager.amInstance.PlaySF("marisaIntroMute");
        yield return new WaitForSeconds(1f);
        GoThroughDialogue(ReimuReactions.MarisaContactReactions);
        yield return new WaitForSeconds(6f);
        AudioManager.amInstance.PlaySF("marisaDoorOpen");
        yield return new WaitForSeconds(2f);
        AudioManager.amInstance.PlaySF("marisaIntro2");
        AddChatBlockReaction(HackReactions.MarisaIntrudingReactions);
        yield return new WaitForSeconds(ReimuReactions.MarisaContactReactions.Length * 2f - 8f);

        if (MarisaCompanionCoroutine != null){
            StopCoroutine(MarisaCompanionCoroutine);
        }
        MarisaCompanionCoroutine = StartCoroutine(SpecialMarisaCoroutine());
        AddPassiveStressChaos(0.5f, 1.5f);
        SetStressChaos();
        EndHack();
    }

    private IEnumerator SpecialMarisaCoroutine(){
        while (true){
            yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
            AudioManager.amInstance.PlaySF(marisaSounds[UnityEngine.Random.Range(0, marisaSounds.Length)].name);
        }
        
    }
#endregion
}
