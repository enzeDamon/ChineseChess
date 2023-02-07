using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
public enum GameState
{
    Black,
    Red,
}

public class GameManager : MonoBehaviour
{
    // position getting in
    public GameObject positions;
    // getting in chess name in
    public Sprite[] names;
    // chessman prefabs
    public GameObject chessmanPrefab;
    // singleton
    public static GameManager instance;
    // positionPrefab 是用来创建空的点击位置的
    public GameObject positionPrefab;
    // chessmanprefab 是用来创建新的chessman的
    public int rowNum = 10;
    public int colNum = 9;
    // 这个是用来放选中的的棋子的，
    public GameObject selected;
    // 把所有的潜在位置都放进它的子对象里面去, 先列后
    public List<List<Transform>> potentialPositions;
    // 把棋子放进去，通过空间节省时间搜索
    public ChessMan[,] chessmanPut;
    private Transform chessmanParent;

    public GameState gamestate;
    void Awake()
    {
        if (instance == null) instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {   
        InitBoard();
        InitChessmen();
        InitTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void InitBoard()
    {
        // 首先先创建好空位置用来方便进行点击
        // 这里用list很重要，避免了反复使用getObjectbyName 去反复获取物体，以免造成大量性能浪费
        potentialPositions = new List<List<Transform>>();
        chessmanPut = new ChessMan[rowNum, colNum];
        for (int i = 0; i < rowNum; i++)
        {
            List<Transform> aRow = new List<Transform>();
            for (int j = 0; j < colNum; j++)
            {
                float col_position = positions.transform.GetChild(j).position.x;
                int row_position_index = i != 0 ?  i + colNum - 1: 0;
                float row_position = positions.transform.GetChild(row_position_index).position.y;
                GameObject go = Instantiate(positionPrefab);
                // 把位置信息全部都扔给gameManager
                go.name = i.ToString()+ "," + j.ToString();
                go.transform.SetParent(transform.GetChild(0));
                go.transform.position = new Vector2(col_position, row_position);
                PositionScript ps = go.GetComponent<PositionScript>();
                ps.intPosition(i, j);
                aRow.Add(go.transform);
            }
            potentialPositions.Add(aRow);
        }
        // 然后将棋子们分别进行创建
    }
    public void stateTurn()
    {
        switch (this.gamestate)
        {
            case GameState.Black:
                this.gamestate = GameState.Red;
                break;
            case GameState.Red:
                this.gamestate = GameState.Black;
                break;    
        }
    }
        
    private void InitChessmen()
    {
        chessmanParent = GameObject.Find("ChessmanContainer").transform;
        InitAchessman(ChessManName.Che, 0, 0, false, chessmanParent);
        InitAchessman(ChessManName.Che, 0, 8, false, chessmanParent);
        InitAchessman(ChessManName.Che, 9, 0, true, chessmanParent);
        InitAchessman(ChessManName.Che, 9, 8, true, chessmanParent);
        // init 马
        InitAchessman(ChessManName.Ma, 0, 1, false, chessmanParent);
        InitAchessman(ChessManName.Ma, 0, 7, false, chessmanParent);
        InitAchessman(ChessManName.Ma, 9, 1, true, chessmanParent);
        InitAchessman(ChessManName.Ma, 9, 7, true, chessmanParent);
        // init 相
        InitAchessman(ChessManName.Xiang, 0, 2, false, chessmanParent);
        InitAchessman(ChessManName.Xiang, 0, 6, false, chessmanParent);
        InitAchessman(ChessManName.Xiang, 9, 2, true, chessmanParent);
        InitAchessman(ChessManName.Xiang, 9, 6, true, chessmanParent);
        // init 仕
        InitAchessman(ChessManName.Shi, 0, 3, false, chessmanParent);
        InitAchessman(ChessManName.Shi, 0, 5, false, chessmanParent);
        InitAchessman(ChessManName.Shi, 9, 3, true, chessmanParent);
        InitAchessman(ChessManName.Shi, 9, 5, true, chessmanParent);
        // init 炮
        InitAchessman(ChessManName.Pao, 2, 1, false, chessmanParent);
        InitAchessman(ChessManName.Pao, 2, 7, false, chessmanParent);
        InitAchessman(ChessManName.Pao, 7, 1, true, chessmanParent);
        InitAchessman(ChessManName.Pao, 7, 7, true, chessmanParent);
        // init 将
        InitAchessman(ChessManName.LaoJiang, 0, 4, false, chessmanParent);
        InitAchessman(ChessManName.LaoJiang, 9, 4, true, chessmanParent);
        // init 兵
        InitAchessman(ChessManName.Bing, 3, 0, false, chessmanParent);
        InitAchessman(ChessManName.Bing, 3, 2, false, chessmanParent);
        InitAchessman(ChessManName.Bing, 3, 4, false, chessmanParent);
        InitAchessman(ChessManName.Bing, 3, 6, false, chessmanParent);
        InitAchessman(ChessManName.Bing, 3, 8, false, chessmanParent);
        InitAchessman(ChessManName.Bing, 6, 0, true, chessmanParent);
        InitAchessman(ChessManName.Bing, 6, 2, true, chessmanParent);
        InitAchessman(ChessManName.Bing, 6, 4, true, chessmanParent);
        InitAchessman(ChessManName.Bing, 6, 6, true, chessmanParent);
        InitAchessman(ChessManName.Bing, 6, 8, true, chessmanParent);

    }
    // init one chessman
    private void InitAchessman(ChessManName chessManName, int row, int col, bool black, Transform parent)
    {

        ChessMan chessman = Instantiate(chessmanPrefab).GetComponent<ChessMan>();
        chessmanPut[row, col] = chessman;
        chessman.transform.SetParent(parent);
        chessman.initChessman(chessManName, row, col, black);
    }
    private void InitTurn()
    {
        this.gamestate = GameState.Red;
    }
}
