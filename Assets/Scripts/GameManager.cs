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
    // positionPrefab �����������յĵ��λ�õ�
    public GameObject positionPrefab;
    // chessmanprefab �����������µ�chessman��
    public int rowNum = 10;
    public int colNum = 9;
    // �����������ѡ�еĵ����ӵģ�
    public GameObject selected;
    // �����е�Ǳ��λ�ö��Ž������Ӷ�������ȥ, ���к�
    public List<List<Transform>> potentialPositions;
    // �����ӷŽ�ȥ��ͨ���ռ��ʡʱ������
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
        // �����ȴ����ÿ�λ������������е��
        // ������list����Ҫ�������˷���ʹ��getObjectbyName ȥ������ȡ���壬������ɴ��������˷�
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
                // ��λ����Ϣȫ�����Ӹ�gameManager
                go.name = i.ToString()+ "," + j.ToString();
                go.transform.SetParent(transform.GetChild(0));
                go.transform.position = new Vector2(col_position, row_position);
                PositionScript ps = go.GetComponent<PositionScript>();
                ps.intPosition(i, j);
                aRow.Add(go.transform);
            }
            potentialPositions.Add(aRow);
        }
        // Ȼ�������Ƿֱ���д���
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
        // init ��
        InitAchessman(ChessManName.Ma, 0, 1, false, chessmanParent);
        InitAchessman(ChessManName.Ma, 0, 7, false, chessmanParent);
        InitAchessman(ChessManName.Ma, 9, 1, true, chessmanParent);
        InitAchessman(ChessManName.Ma, 9, 7, true, chessmanParent);
        // init ��
        InitAchessman(ChessManName.Xiang, 0, 2, false, chessmanParent);
        InitAchessman(ChessManName.Xiang, 0, 6, false, chessmanParent);
        InitAchessman(ChessManName.Xiang, 9, 2, true, chessmanParent);
        InitAchessman(ChessManName.Xiang, 9, 6, true, chessmanParent);
        // init ��
        InitAchessman(ChessManName.Shi, 0, 3, false, chessmanParent);
        InitAchessman(ChessManName.Shi, 0, 5, false, chessmanParent);
        InitAchessman(ChessManName.Shi, 9, 3, true, chessmanParent);
        InitAchessman(ChessManName.Shi, 9, 5, true, chessmanParent);
        // init ��
        InitAchessman(ChessManName.Pao, 2, 1, false, chessmanParent);
        InitAchessman(ChessManName.Pao, 2, 7, false, chessmanParent);
        InitAchessman(ChessManName.Pao, 7, 1, true, chessmanParent);
        InitAchessman(ChessManName.Pao, 7, 7, true, chessmanParent);
        // init ��
        InitAchessman(ChessManName.LaoJiang, 0, 4, false, chessmanParent);
        InitAchessman(ChessManName.LaoJiang, 9, 4, true, chessmanParent);
        // init ��
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
