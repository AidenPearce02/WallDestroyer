using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public SwipeType SwipeType { get; private set; } = SwipeType.NONE;
    public bool GameOver { get; private set; } = false;
    public bool FreeMode { get; private set; } = true;
    public bool IsGame { get; set; } = false;

    public Text tScore, tMoney, tRecord, tScoreEnd;
    public float Score { get; private set; } = 0.0f;
    public int Money { get; private set; } = 0;
    public void AddMoney(int money) {
        Money += money;
    }

    public float timeGreenBooster = 0.0f;
    public float timeRedBooster = 0.0f;
    private float delayBetweenPoints = 1.0f;
    private int fiveSecondCount = 0;
    private float oneSecondPassed = 0.0f;
    private float time = 0.0f;
    private int tempPoint = 1;

    private Transform firstWall;

    public bool isEffectedWall { get; private set; } = false;
    public void SetIsEffectedWall(bool temp)
    {
        isEffectedWall = temp;
    }
    public GameObject Player;
    public GameObject countDown;
    public bool Immortal { get; private set; } = false;
    public void SetImmortal(bool state)
    {
        Immortal = state;
    }
    float immortalTime = 0.0f;
    public int DurationImmortal { get; private set; }
    public void SetDurationImmortal(int duration)
    {
        DurationImmortal = duration;
    }
    public static LevelManager Instance { get; set; }

    private List<GameObject> roofs=new List<GameObject>();

    //EndLess Mode
    private Dictionary<string, GameObject> roofsPool, leftLegsPool, rightLegsPool;
    private GameObject bonusWall;
    private float spawnY = 4f;
    private float spawnBonus = 4.8f;
    private Transform parentOfXmlWalls;
    private float bonusWallTime = 0.0f;
    private int delayBetweenBonus = 60;
    private int durationBonus = 5;
    private int rightPlusLeg = 0, rightMinusLeg = 0;
    private bool isBonus = false;
    private bool isBonusCreated = false;
    private float distance = 0.0f;
    private GameObject trackedWall = null;
    private bool isRecord=false;
    private float recordTime = 0.0f;
    public bool isSecondLife = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        Player.GetComponent<SpriteRenderer>().sprite = SkinManager.Instance.CurrentSkin.skinFull;
        if (GameManager.Instance.CurrentLevel != 0) {
            FreeMode = false;
        }
        if(PerkManager.Instance.checkPerk("Perk 13", 5))
        {
            Player.GetComponent<Effect>().SetCurrentEffect(Effect.EffectType.YELLOW);
            isEffectedWall = true;
        }
        if (!FreeMode){
            tScore.gameObject.SetActive(false);
            DeserializedLevelsLoader d = new DeserializedLevelsLoader();
            d.GenerateItems(GameManager.Instance.CurrentLevel);
            roofs.AddRange(GameObject.FindGameObjectsWithTag("Wall"));
        }
        else
        {
            EndLessMode();
        }
    }
    const string xmlItemsGOName = "XmlItems";
    void EndLessMode() {
        const string prefabsFolder = "Prefabs/";
        const string roofsFolder = "Roofs/";
        const string leftLegsFolder = "LeftLegs/";
        const string rightLegsFolder = "RightLegs/";
        //переделать
        parentOfXmlWalls = new GameObject(xmlItemsGOName).transform;
        roofsPool = new Dictionary<string, GameObject>();
        leftLegsPool = new Dictionary<string, GameObject>();
        rightLegsPool = new Dictionary<string, GameObject>();
        List<GameObject> roofsList = Resources.LoadAll<GameObject>(prefabsFolder + roofsFolder).ToList();
        List<GameObject> leftLegsList = Resources.LoadAll<GameObject>(prefabsFolder + leftLegsFolder).ToList();
        List<GameObject> rightLegsList = Resources.LoadAll<GameObject>(prefabsFolder + rightLegsFolder).ToList();
        foreach (GameObject item in roofsList)
        {
            if (!item.name.Contains("Bonus"))
            {
                roofsPool.Add(item.name, item);
            }
            else bonusWall = item;
        }
        foreach (GameObject item in leftLegsList)
        {
            leftLegsPool.Add(item.name, item);
        }
        foreach (GameObject item in rightLegsList)
        {
            rightLegsPool.Add(item.name, item);
        }
        rightPlusLeg = 0;
        rightMinusLeg = 0;
        foreach (var item in rightLegsPool)
        {
            if (item.Key.Contains("Plus"))
            {
                break;
            }
            rightPlusLeg++;
        }
        foreach (var item in rightLegsPool)
        {
            if (item.Key.Contains("Minus"))
            {
                break;
            }
            rightMinusLeg++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameOver&&IsGame){
            oneSecondPassed += Time.deltaTime;
            time += Time.deltaTime;
            if (oneSecondPassed >= delayBetweenPoints)
            {
                oneSecondPassed = 0.0f;
                fiveSecondCount++;
                if (fiveSecondCount == 5)
                {
                    fiveSecondCount = 0;
                    tempPoint++;
                    delayBetweenPoints = 1.0f / tempPoint;
                }
                float temp = (Player.GetComponent<Effect>().CurrentEffect == Effect.EffectType.BLACK) ? 2f : 1f;
                temp *= (PerkManager.Instance.checkPerk("Perk 16", 6)) ? 1.5f : 1f;
                Score += temp;
            }
            if (Immortal)
            {
                while(firstWall)
                {
                    if (firstWall.childCount==1)
                    {
                        if (firstWall.GetChild(0).gameObject.GetComponent<Renderer>().IsVisibleFrom(Camera.main))
                        {
                            DestroyWall(firstWall.GetChild(0).gameObject.name);
                            roofs.Remove(firstWall.gameObject);
                            Destroy(firstWall.gameObject);
                            if (roofs.Count != 0)
                            {
                                firstWall = roofs[0].transform;
                            }
                            else
                            {
                                firstWall = null;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (firstWall.GetChild(1).gameObject.GetComponent<Renderer>().IsVisibleFrom(Camera.main))
                        {
                            DestroyWall(firstWall.GetChild(0).gameObject.name);
                            roofs.Remove(firstWall.gameObject);
                            Destroy(firstWall.gameObject);
                            if (roofs.Count != 0)
                            {
                                firstWall = roofs[0].transform;
                            }
                            else
                            {
                                firstWall = null;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                immortalTime += Time.deltaTime;
                if(immortalTime >= DurationImmortal)
                {
                    immortalTime = 0.0f;
                    Immortal = false;
                }
            }
            if (FreeMode)
            {
                if(PerkManager.Instance.checkPerk("Perk 7", 3))
                {
                    timeGreenBooster += Time.deltaTime;
                    if (timeGreenBooster >= 50 && !isEffectedWall)
                    {
                        timeGreenBooster = 0.0f;
                        isEffectedWall = true;
                        Player.GetComponent<Effect>().SetCurrentEffect(Effect.EffectType.GREEN);
                        Player.GetComponent<Effect>().SetDuration(5);
                    }
                }
                if (PerkManager.Instance.checkPerk("Perk 17", 6))
                {
                    timeRedBooster += Time.deltaTime;
                    if (timeRedBooster >= 15 && !isEffectedWall)
                    {
                        timeRedBooster = 0.0f;
                        isEffectedWall = true;
                        Player.GetComponent<Effect>().SetCurrentEffect(Effect.EffectType.RED);
                    }
                }
                float temp = (Player.GetComponent<Effect>().CurrentEffect == Effect.EffectType.PURPLE) ? 0.5f : 1f;
                bonusWallTime += temp*Time.deltaTime;
                if (!isBonus)
                {
                    int tempBonus = 0;
                    if (PerkManager.Instance.checkPerk("Perk 5", 2)) tempBonus = 2;
                    if (bonusWallTime >= delayBetweenBonus+tempBonus&&!Immortal)
                    {
                        bonusWallTime = 0.0f;
                        isBonus = true;
                    }
                }
                else
                {
                    if (roofs.Count == 0)
                    {
                        bonusWallTime = 0.0f;
                        CreateBonusWall();
                    }
                    if (bonusWallTime >= durationBonus && isBonusCreated)
                    {
                        bonusWallTime = 0.0f;
                        durationBonus++;
                        isBonus = false;
                        if (!isEffectedWall)
                        {
                            List<Effect.EffectType> effects = new List<Effect.EffectType>()
                            {
                                Effect.EffectType.YELLOW,
                                Effect.EffectType.GREEN,
                                Effect.EffectType.RED,
                                Effect.EffectType.PURPLE,
                                Effect.EffectType.BLUE
                            };
                            if (PerkManager.Instance.checkPerk("Perk 11", 4))
                            {
                                effects.Add(Effect.EffectType.BLACK);
                            }
                            if (PerkManager.Instance.checkPerk("Perk 14", 5))
                            {
                                effects.Add(Effect.EffectType.RAINBOW);
                            }
                            int typeOfEffect = Random.Range(0, effects.Count);
                            Player.GetComponent<Effect>().SetCurrentEffect(effects.ToArray()[typeOfEffect]);
                            isEffectedWall = true;
                        }
                        roofs.Remove(firstWall.gameObject);
                        Destroy(firstWall.gameObject);
                    }
                }
                if (trackedWall == null)
                {
                    CreateWall();
                }
                else
                {
                    if (trackedWall.transform.childCount != 1)
                    {
                        if (trackedWall.transform.GetChild(1).gameObject.GetComponent<Renderer>().IsVisibleFrom(Camera.main))
                        {
                            CreateWall();
                        }
                    }
                    else
                    {
                        if (trackedWall.transform.GetChild(0).gameObject.GetComponent<Renderer>().IsVisibleFrom(Camera.main))
                        {
                            CreateWall();
                        }
                    }
                }
            }
#if UNITY_ANDROID
            if (roofs.Count != 0)
            {
                if (SwipeType != SwipeType.NONE && SwipeType != SwipeType.DOWN)
                {
                    firstWall = roofs[0].transform;
                    if (SwipeType == SwipeType.LEFT)
                    {
                        foreach (Transform prefab in firstWall)
                        {
                            if (prefab.name.Contains("LeftLeg"))
                            {
                                TryDoDestroyObject(prefab);
                                SwipeType = SwipeType.NONE;
                                break;
                            }
                        }
                    }
                    else if (SwipeType == SwipeType.RIGHT)
                    {
                        foreach (Transform prefab in firstWall)
                        {
                            if (prefab.name.Contains("RightLeg"))
                            {
                                TryDoDestroyObject(prefab);
                                SwipeType = SwipeType.NONE;
                                break;
                            }
                        }
                    }
                    else if (SwipeType == SwipeType.UP)
                    {
                        if (!firstWall.gameObject.name.Contains("Bonus"))
                        {
                            foreach (Transform prefab in firstWall)
                            {
                                if (prefab.name.Contains("Roof"))
                                {
                                    TryDoDestroyObject(prefab);
                                    SwipeType = SwipeType.NONE;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            TryDoDestroyObject(firstWall);
                            SwipeType = SwipeType.NONE;
                        }
                    }
                }
            }
#endif
#if UNITY_EDITOR_WIN
            if (roofs.Count != 0)
            {
                firstWall = roofs[0].transform;
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    foreach (Transform prefab in firstWall)
                    {
                        if (prefab.name.Contains("LeftLeg"))
                        {
                            TryDoDestroyObject(prefab);
                            SwipeType = SwipeType.NONE;
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    foreach (Transform prefab in firstWall)
                    {
                        if (prefab.name.Contains("RightLeg"))
                        {
                            TryDoDestroyObject(prefab);
                            SwipeType = SwipeType.NONE;
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (!firstWall.gameObject.name.Contains("Bonus"))
                    {
                        foreach (Transform prefab in firstWall)
                        {
                            if (prefab.name.Contains("Roof"))
                            {
                                TryDoDestroyObject(prefab);
                                SwipeType = SwipeType.NONE;
                                break;
                            }
                        }
                    }
                    else
                    {
                        TryDoDestroyObject(firstWall);
                        SwipeType = SwipeType.NONE;
                    }
                }
            }
#endif
            tMoney.text = "Earned money:\n" + Money;
            tScoreEnd.text = "Score:\n" + Score;
            tScore.text = "Score: " + Score;
            if (tScore.gameObject.activeSelf && Score > GameManager.Instance.Record && recordTime == 0.0f) {
                tRecord.gameObject.SetActive(true);
                tScoreEnd.transform.GetChild(0).gameObject.SetActive(true);
                isRecord = true;
            }
            if (isRecord) {
                recordTime += Time.unscaledDeltaTime;
                if (recordTime >= 3) {
                    tRecord.gameObject.SetActive(false);
                    isRecord = false;
                }
            }
        }
        
    }

    private void CreateBonusWall() {
        GameObject bonusWallTemp = Instantiate(bonusWall, parentOfXmlWalls);
        SetPos2D(bonusWallTemp, new Vector2(0, spawnBonus));
        bonusWallTemp.gameObject.tag = "Wall";
        roofs.Add(bonusWallTemp);
        isBonusCreated = true;
    }

    private void CreateWall() {
        float max = 0f, min = -2.5f;
        GameObject newWall = new GameObject();
        int typeOfRoof = Random.Range(0, roofsPool.Count);
        GameObject roofObject = Instantiate(roofsPool.Values.ElementAt(typeOfRoof), newWall.transform);
        if (!isEffectedWall && Player.GetComponent<Effect>().CurrentEffect == Effect.EffectType.NONE)
        {
            int chanceOfEffect = Random.Range(0, 5);
            if (chanceOfEffect == 0)
            {
                List<Effect.EffectType> effects = new List<Effect.EffectType>()
                {
                    Effect.EffectType.YELLOW,
                    Effect.EffectType.GREEN,
                    Effect.EffectType.RED,
                    Effect.EffectType.PURPLE,
                    Effect.EffectType.BLUE
                };
                if (PerkManager.Instance.checkPerk("Perk 11", 4)) 
                {
                    effects.Add(Effect.EffectType.BLACK);
                }
                if (PerkManager.Instance.checkPerk("Perk 14", 5))
                {
                    effects.Add(Effect.EffectType.RAINBOW);
                }
                int typeOfEffect = Random.Range(0, effects.Count);
                roofObject.GetComponent<Effect>().SetCurrentEffect(effects.ToArray()[typeOfEffect]);
                isEffectedWall = true;
            }
        }
        if(distance==0) SetPos2D(roofObject, new Vector2(0, spawnY));
        else{
            SetPos2D(roofObject, new Vector2(0, trackedWall.transform.GetChild(0).position.y+distance));
        }
        newWall.name = roofsPool.Keys.ElementAt(typeOfRoof)+",";
        int typeOfLeftLeg = Random.Range(-1, leftLegsPool.Count);
        int charge = 0;
        if (typeOfLeftLeg != -1)
        {
            GameObject leftLegObject = Instantiate(leftLegsPool.Values.ElementAt(typeOfLeftLeg), newWall.transform);
            if (leftLegsPool.Keys.ElementAt(typeOfLeftLeg).Contains("Plus")) charge = 1;
            else if (leftLegsPool.Keys.ElementAt(typeOfLeftLeg).Contains("Minus")) charge = 2;
            if (distance == 0) SetPos2D(leftLegObject, new Vector2(leftLegObject.transform.position.x, spawnY - 0.5f));
            else
            {
                SetPos2D(leftLegObject, new Vector2(leftLegObject.transform.position.x,
                    trackedWall.transform.GetChild(0).position.y + distance - 0.5f));
            }
            newWall.name += leftLegsPool.Keys.ElementAt(typeOfLeftLeg) + ",";
        }
        else
        {
            newWall.name += "null,";
        }
        int typeOfRightLeg = 0;
        if (charge == 0)
        {
            typeOfRightLeg = Random.Range(-1, rightLegsPool.Count);
            while (typeOfRightLeg==rightPlusLeg||typeOfRightLeg==rightMinusLeg)
            {
                typeOfRightLeg = Random.Range(-1, rightLegsPool.Count);
            }        
        }
        else if(charge == 1)
        {
            typeOfRightLeg = rightMinusLeg;
        }
        else if(charge == 2)
        {
            typeOfRightLeg = rightPlusLeg;
        }
        if (typeOfRightLeg != -1)
        {
            GameObject rightLegObject = Instantiate(rightLegsPool.Values.ElementAt(typeOfRightLeg), newWall.transform);
            if (distance == 0) SetPos2D(rightLegObject, new Vector2(rightLegObject.transform.position.x, spawnY - 0.5f));
            else
            {
                SetPos2D(rightLegObject, new Vector2(rightLegObject.transform.position.x,
                    trackedWall.transform.GetChild(0).position.y + distance - 0.5f));
            }
            newWall.name += rightLegsPool.Keys.ElementAt(typeOfRightLeg);
        }
        else
        {
            newWall.name += "null";
        }
        if (distance == 0) SetPos2D(newWall, new Vector2(0, spawnY));
        else
        {
            SetPos2D(newWall, new Vector2(0, trackedWall.transform.GetChild(0).position.y + distance));
        }
        newWall.gameObject.tag = "Wall";
        newWall.transform.parent = parentOfXmlWalls;
        roofs.Add(newWall);
        distance = Random.Range(min, max);
        trackedWall = newWall;
    }

    void SetPos2D(GameObject g, Vector2 pos)
    {
        g.transform.position = new Vector3(
            pos.x,
            pos.y,
            g.transform.position.z
        );
    }

    private void TryDoDestroyObject(Transform temp) {
        if (temp.gameObject.GetComponent<Renderer>().IsVisibleFrom(Camera.main))
        {
            Health health = temp.gameObject.GetComponent<Health>();
            bool isDamaged = false;
            if (health.Life != 0)
            {
                if (temp.gameObject.name.Contains("Bonus"))
                {
                    Money += 5;
                }
                else
                {
                    if (firstWall.childCount != 1)
                    {
                        if (temp.gameObject.name.Contains("Roof"))
                        {
                            int n = 0;
                            foreach (Transform prefab in firstWall)
                            {
                                if (prefab.name.Contains("Leg"))
                                {
                                    if (prefab.name.Contains("Studded"))
                                    {
                                        n++;
                                    }
                                    else
                                    {
                                        n--;
                                    }
                                }

                            }
                            if (n > 0)
                            {
                                isDamaged = true;
                                health.DropLife();
                                if (Player.GetComponent<Effect>().CurrentEffect == Effect.EffectType.BLUE)
                                {
                                    health.SetLife(0);
                                }
                            }
                        }
                        else if (temp.gameObject.name.Contains("Leg"))
                        {
                            if (!temp.gameObject.name.Contains("Studded") && !(temp.gameObject.name.Contains("Minus") && firstWall.childCount == 3)) isDamaged = true;
                            health.DropLife();
                        }
                    }
                    else
                    {
                        isDamaged = true;
                        health.DropLife();
                        if (Player.GetComponent<Effect>().CurrentEffect == Effect.EffectType.BLUE)
                        {
                            health.SetLife(0);
                        }
                    }
                }
            }
            if (isDamaged && Player.gameObject.GetComponent<Effect>().CurrentEffect == Effect.EffectType.RED)
            {
                SetImmortal(2);
                return;
            }
            if (health.Life == 0)
            {
                if (temp.gameObject.name.Contains("Roof"))
                {
                    DestroyWall(temp.gameObject.name);
                    roofs.Remove(firstWall.gameObject);
                    Destroy(firstWall.gameObject);
                }
                else if (temp.gameObject.name.Contains("Leg"))
                {
                    if (temp.gameObject.name.Contains("Studded"))
                    {
                        GameOver = true;
                    }
                    else if (temp.gameObject.name.Contains("TNT"))
                    {
                        string name = firstWall.GetChild(0).gameObject.name;
                        DestroyWall(name);
                        roofs.Remove(firstWall.gameObject);
                        Destroy(firstWall.gameObject);
                    }
                    else if (temp.gameObject.name.Contains("Minus") && firstWall.childCount == 3)
                    {
                        GameOver = true;
                    }
                }
                if (temp.gameObject) Destroy(temp.gameObject);
            }
        }
    }

    public void SetImmortal(int duration) {
        Player.gameObject.GetComponent<Effect>().Default();
        SetImmortal(true);
        SetDurationImmortal(duration);
    }

    public void DestroyWall(string name)
    {
        int temp = (Player.gameObject.GetComponent<Effect>().CurrentEffect == Effect.EffectType.GREEN) ? 2 : 1;
        if (name.Contains("Wood"))
        {
            Score += 10 + (1 * (int)(Mathf.Round(time)) / 5);
            Money += temp * 10;
        }
        else if (name.Contains("Brick"))
        {
            Score += 20 + (1 * (int)(Mathf.Round(time)) / 5);
            Money += temp * 20;
        }
        else if (name.Contains("Iron"))
        {
            Score += 30 + (1 * (int)(Mathf.Round(time)) / 5);
            Money += temp * 30;
        }
        if (!isEffectedWall)
        {
            int chance = Random.Range(0, 50);
            if (chance == 0)
            {
                List<Effect.EffectType> effects=new List<Effect.EffectType>();
                if(PerkManager.Instance.checkPerk("Perk 1", 1))
                {
                    effects.Add(Effect.EffectType.RED);
                }
                if (PerkManager.Instance.checkPerk("Perk 2", 1))
                {
                    effects.Add(Effect.EffectType.BLUE);
                }
                if (PerkManager.Instance.checkPerk("Perk 3", 1))
                {
                    effects.Add(Effect.EffectType.PURPLE);
                }
                if (PerkManager.Instance.checkPerk("Perk 18", 6))
                {
                    effects.Add(Effect.EffectType.BLACK);
                }
                if (effects.Count != 0)
                {
                    int wall = Random.Range(0, effects.Count);
                    Player.GetComponent<Effect>().SetCurrentEffect(effects.ToArray()[wall]);
                    if (effects.ToArray()[wall] != Effect.EffectType.RED)
                    {
                        Player.GetComponent<Effect>().SetDuration(5);
                    }
                    isEffectedWall = true;
                }
            }
        }
        else
        {
            Effect effect = firstWall.GetChild(0).gameObject.GetComponent<Effect>();
            if (effect.CurrentEffect != Effect.EffectType.NONE)
            {
                Player.GetComponent<Effect>().SetCurrentEffect(effect.CurrentEffect);
                int tempDurration = 1;
                if (PerkManager.Instance.checkPerk("Perk 4", 2)) tempDurration = 2;
                if (effect.CurrentEffect == Effect.EffectType.GREEN || effect.CurrentEffect == Effect.EffectType.PURPLE ||
                    effect.CurrentEffect == Effect.EffectType.BLUE || effect.CurrentEffect == Effect.EffectType.BLACK)
                {
                    Player.GetComponent<Effect>().SetDuration(tempDurration * 10);
                }
                else if (effect.CurrentEffect == Effect.EffectType.RAINBOW)
                {
                    Player.GetComponent<Effect>().SetDuration(tempDurration * 5);
                }
            }
        }
    }

    public void EndGame()
    {
        GameOver = true;
    }

    public void SetSwipe(SwipeType temp)
    {
        SwipeType = temp;
    }

    public List<GameObject> GetObjects() {
        return roofs;
    }

    public void StartGame() {
        GameOver = false;
        isSecondLife = true;
        SetImmortal(2);
        countDown.GetComponent<CountDown>().StartCountDown();
    }

}
