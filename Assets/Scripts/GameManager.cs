using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public AudioClip die;
    public int level = 1;
    public int food = 20;

    private bool sleepStep = true;
    [HideInInspector]public List<Enemy> enemyList = new List<Enemy>();
    private Text failText;
    private Player player;
    private MapManager mapManager;
    private Text dayText;



    [HideInInspector]public bool isEnd = false;

    private Text foodText;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        InitGame();
        DontDestroyOnLoad(gameObject);
        
       
        
    }
    void InitGame()
    {
        //初始化地图
        mapManager = GetComponent<MapManager>();
        mapManager.InitMap();

        //初始化UI
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        UpdateFoodText(0);

        dayText = GameObject.Find("DayText").GetComponent<Text>();
        dayText.text = "Day：" + level;

        failText = GameObject.Find("FailText").GetComponent<Text>();
        failText.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        isEnd = false;
        enemyList.Clear();
    }
    void UpdateFoodText(int foodChange)
    {
        if(foodChange == 0)
        {
            foodText.text = "Food：" + food;
        }
        else
        {
            string str = "";
            if(foodChange < 0)
            {
                str = foodChange.ToString();
            }
            else
            {
                str = "+" + foodChange;
            }
            foodText.text = str + "    Food" + food;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void addFood(int count)
    {
        food += count;
        UpdateFoodText(count);
    }
    public void subFood(int count)
    {
        food -= count;
        UpdateFoodText(-count);
        if(food <= 0)
        {
            failText.enabled = true;
            AudioManager.Instance.RandomPlay(die);
            AudioManager.Instance.StopBg();
        }
    }

    public void OnPlayerMove()
    {
        if(sleepStep==true)
        {
            sleepStep = false;
        }
        else
        {
            foreach(var ene in enemyList)
            {
                ene.Move();
            }
            sleepStep = true;
        }
        if(player.targetPos.x == mapManager.cols-2 && player.targetPos.y == mapManager.rows - 2)
        {
            isEnd = true;
            //加载下一个关卡
            //Application.LoadLevel(Application.loadedLevel);
            SceneManager.LoadScene("Main");
            //SceneManager.sceneLoaded += AddLevel;
        }
    }
    /*
    void AddLevel(Scene scene, LoadSceneMode loadSceneMode)
    {
        level++;
        InitGame();
    }
    */
    private void OnLevelWasLoaded(int sceneLevel)
    {
        level++;
        InitGame();
    }
}
