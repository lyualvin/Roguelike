using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public GameObject[] outWallArray;
    public GameObject[] floorArray;
    public GameObject[] wallArray;
    public GameObject ExitIcon;
    public GameObject[] foodArray;
    public GameObject[] enemyArray;


    public int rows = 10;
    public int cols = 10;


    private List<Vector2> positionList = new List<Vector2>();

    private int minCountWalls = 2;
    private int maxCountWalls = 8;


    private GameManager gameManager;

    private Transform mapHolder;

    //初始化地图
    public void InitMap()
    {
        gameManager = this.GetComponent<GameManager>();
        mapHolder = new GameObject("Map").transform;
        //生成墙壁和地板
        for(int x = 0; x < rows; x++)
        {
            for(int y = 0; y < cols ;y++)
            {
                if(x == 0 || y == 0 || x == cols -1 || y == rows -1 )
                {
                    int index = Random.Range(0, outWallArray.Length);
                    GameObject go =  GameObject.Instantiate(outWallArray[index], new Vector3(x,y,0), Quaternion.identity);
                    go.transform.SetParent(mapHolder);
                }
                else
                {
                    int index = Random.Range(0, floorArray.Length);
                    GameObject go =  GameObject.Instantiate(floorArray[index], new Vector3(x, y, 0), Quaternion.identity);
                    go.transform.SetParent(mapHolder);
                }
            }
        }

        //获取地图内部坐标
        positionList.Clear();
        for(int x =2; x < rows - 2; x ++)
        {
            for(int y = 2; y < cols -2; y ++)
            {
                positionList.Add(new Vector2(x, y));
            }
        }
        //创建障碍物 食物 和 敌人
        //创建障碍物
        int wallCount = Random.Range(minCountWalls,maxCountWalls+1);
        InstantiateItems(wallCount,wallArray );

        //创建食物 2-level*2
        int foodCount = Random.Range(2, gameManager.level*2+1);
        InstantiateItems(foodCount, foodArray);

        //创建敌人  数量为level/2
        int enemyCount = gameManager.level / 2;
        InstantiateItems(enemyCount, enemyArray);

        //创建出口
        GameObject go2 = GameObject.Instantiate(ExitIcon, new Vector2(rows-2, cols-2), Quaternion.identity);
        go2.transform.SetParent(mapHolder);

    }

    private void InstantiateItems(int count , GameObject[] prefabs )
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = RandPosition();
            GameObject enemyPre = RandomPrefab(prefabs);
            GameObject go = GameObject.Instantiate(enemyPre, pos, Quaternion.identity);
            go.transform.SetParent(mapHolder);
        }
    }

    private Vector2 RandPosition()
    {
        int rand = Random.Range(0, positionList.Count);
        Vector2 pos = positionList[rand];
        positionList.RemoveAt(rand);
        return pos;
    }

    private GameObject RandomPrefab(GameObject[] prefabs)
    {
        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }
}

