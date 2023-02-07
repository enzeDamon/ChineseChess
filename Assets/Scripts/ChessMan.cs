using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

enum ChessManState
{
    SELECTED,
    UNSELECTED
}
public enum ChessManName
{
    Che,
    Ma,
    Xiang,
    Shi,
    LaoJiang,
    Pao,
    Bing
}
public class ChessMan : MonoBehaviour, IPointerClickHandler
{
    public Sprite Selcted, Unselected;
    // true 就是黑色的
    public bool black;
    // this one is to
    public SpriteRenderer srName, srSelcted;
    // record the row
    public int row, col;
    ChessManState selection;
    // 这个东西决定了它的规则
    public ChessManName chessManName;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameState thisBlack = this.black ? GameState.Black : GameState.Red;
        if (GameManager.instance.selected)
            GameManager.instance.selected.GetComponent<ChessMan>().stateGoes(this.col, this.row, gameObject);
        else if (GameManager.instance.gamestate == thisBlack && !GameManager.instance.selected)
                stateGoes(this.col, this.row, null);
        else 
            Hint.Instance.showWrongSelection();

    }
    void Awake()
    {
        srName = transform.GetChild(0).GetComponent<SpriteRenderer>();
        srSelcted = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void getSelected()
    {

        GameManager.instance.selected = gameObject;
        this.selection = ChessManState.SELECTED;
        srSelcted.sprite = Selcted;
    }

    public void getUnselected()
    {
        GameManager.instance.selected = null;
        this.selection = ChessManState.UNSELECTED;
        srSelcted.sprite = Unselected;                                                                                            
    }
    // 下面这个玩意就是个状态机，可以在外面被调用的
    public void stateGoes(int clickCol, int clickRow, GameObject targetedChessmanPosition)
    {
        Debug.Log(selection);
        switch (selection)
        {
            case ChessManState.SELECTED:


                if (clickCol == this.col && clickRow == this.row)
                {
                    getUnselected();
                } else
                {
                    
                    ChessMan[,] temp = GameManager.instance.chessmanPut;
                    if (temp[clickRow, clickCol])
                    {

                        bool tempBlack = temp[clickRow, clickCol].black;

                        if (tempBlack == this.black)
                            Hint.Instance.showWrongStep();
                        else
                        {

                            if (checkRulePermission(clickCol, clickRow))
                            {
                                this.Move(clickRow, clickCol);

                                getUnselected();
                                GameManager.instance.stateTurn();
                                Destroy(targetedChessmanPosition);

                            }
                            else
                            {

                                Hint.Instance.showWrongStep();
                            }
                        }

                    }
                    else
                    {
                        if (checkRulePermission(clickCol, clickRow))
                        {
                            this.Move(clickRow, clickCol);

                            getUnselected();
                            GameManager.instance.stateTurn();

                        }
                        else
                        {
                            Hint.Instance.showWrongStep();
                        }
                    }
                    
                    

                }
                
                break;
            case ChessManState.UNSELECTED:
                    getSelected();
                
                break;
        }
    }
    public void Move(int row, int col)
    {
        transform.position = GameManager.instance.potentialPositions[row][col].position;
        GameManager.instance.chessmanPut[this.row, this.col] = null;
        GameManager.instance.chessmanPut[row, col] = this;
        this.col = col;
        this.row = row;
    }


    public void initChessman(ChessManName chessManName, int row, int col, bool black)
    {
        this.chessManName = chessManName;
        this.row = row;
        this.col = col;
        this.black = black;
        this.selection = ChessManState.UNSELECTED;
        switch(this.chessManName)
        {
            case ChessManName.LaoJiang:
                if(black)
                {
                    srName.sprite = GameManager.instance.names[11];
                    
                } else
                {
                    srName.sprite = GameManager.instance.names[4];
                }
                transform.position = GameManager.instance.potentialPositions[row][col].position;
                break;
            case ChessManName.Che:
                if (black)
                {
                    srName.sprite = GameManager.instance.names[7];
                }
                else
                {
                    srName.sprite = GameManager.instance.names[0];
                }
                transform.position = GameManager.instance.potentialPositions[row][col].position;
                break;
            case ChessManName.Ma:
                if (black)
                {
                    srName.sprite = GameManager.instance.names[8];
                } else
                {
                    srName.sprite = GameManager.instance.names[1];
                }
                transform.position = GameManager.instance.potentialPositions[row][col].position;
                break;
            case ChessManName.Xiang:
                if (black)
                {
                    srName.sprite = GameManager.instance.names[9];
                }
                else
                {
                    srName.sprite = GameManager.instance.names[2];
                }
                transform.position = GameManager.instance.potentialPositions[row][col].position;
                break;
            case ChessManName.Shi:
                if (black)
                {
                    srName.sprite = GameManager.instance.names[10];
                }
                else
                {
                    srName.sprite = GameManager.instance.names[3];
                }
                transform.position = GameManager.instance.potentialPositions[row][col].position;
                break;
            case ChessManName.Pao:
                if (black)
                {
                    srName.sprite = GameManager.instance.names[12];
                }
                else
                {
                    srName.sprite = GameManager.instance.names[5];
                }
                transform.position = GameManager.instance.potentialPositions[row][col].position;
                break;
            case ChessManName.Bing:
                if (black)
                {
                    srName.sprite = GameManager.instance.names[13];
                }
                else
                {
                    srName.sprite = GameManager.instance.names[6];
                }
                transform.position = GameManager.instance.potentialPositions[row][col].position;
                break;

        }
    }
    public bool checkRulePermission(int clickCol, int clickRow)
    {

        
        switch (this.chessManName)
        {
            case ChessManName.LaoJiang:
                // 首先确认帅可以在周围一步走动
                if ((Mathf.Abs(this.col - clickCol) == 1 && this.row == clickRow) ^ (Mathf.Abs(this.row - clickRow) == 1 && this.col == clickCol))
                {
                    if (this.black)
                    {
                        if (clickCol > 5 || clickCol < 3 || clickRow < 7) return false;
                    }
                    else
                    {
                        if (clickCol > 5 || clickCol < 3 || clickRow > 2) return false;
                    }
                }
                else if (GameManager.instance.chessmanPut[clickRow,clickCol].chessManName == ChessManName.LaoJiang)
                {
                    for (int i = this.row > clickRow ? clickRow + 1 : this.row + 1; i < (this.row < clickRow ? clickRow : this.row); i++)
                        if(GameManager.instance.chessmanPut[i, this.col]) return false;

                }
                else { 
                    return false;
                }

                break;
            case ChessManName.Che:
                
                if (this.col != clickCol && this.row != clickRow)
                {
                    return false;
                }
                else if (this.row == clickRow)
                {
                    for (int i = this.col > clickCol ? clickCol + 1 : this.col + 1; i < (this.col < clickCol ? clickCol : this.col ); i++)
                    {

                        if (GameManager.instance.chessmanPut[row, i]) return false;
                    }
                }
                else if (this.col == clickCol)
                {
                    for (int i = this.row> clickRow ? clickRow + 1 : this.row + 1; i < (this.row < clickRow ? clickRow : this.row ); i++)
                    {
                        if (GameManager.instance.chessmanPut[i, this.col]) return false;
                    }
                }
                break;
            case ChessManName.Ma:
                //判定右马腿
                if ((clickCol - col == 2) && Mathf.Abs(clickRow - row) == 1) 
                {
                    if (GameManager.instance.chessmanPut[row, col+ 1]) 
                        return false;
                 
                }
                //判定左马腿
                else if ((col - clickCol == 2) && Mathf.Abs(clickRow - row) == 1)
                {
                    if (GameManager.instance.chessmanPut[row, col - 1]) 
                        return false;
                }
                else if((clickRow - row == 2) && Mathf.Abs(col - clickCol) == 1)
                {
                    if (GameManager.instance.chessmanPut[row + 1, col])
                        return false;
                }
                else if ((row - clickRow == 2) && Mathf.Abs(col - clickCol) == 1)
                {
                    if (GameManager.instance.chessmanPut[row - 1, col])
                        return false;
                } else
                {
                    return false;
                }
                break;
            case ChessManName.Xiang:
                if (Mathf.Abs(clickCol - col) == 2 && Mathf.Abs(clickRow - row) == 2)
                {
                    if (GameManager.instance.chessmanPut[(clickRow + row) / 2, (clickCol + col) / 2])
                    {
                        return false;
                    }
                    if (this.black)
                    {
                        if (clickRow < 5) return false;
                    }
                    else
                    {
                        if (clickRow > 4) return false;
                    }
                } else
                {
                    return false;
                }

                break;
            case ChessManName.Shi:
                if (Mathf.Abs(clickCol - col) == 1 && Mathf.Abs(clickRow - row) == 1)
                {
                    if (this.black)
                    {
                        if (clickCol > 5 || clickCol < 3 || clickRow < 7) return false;
                    }
                    else
                    {
                        if (clickCol > 5 || clickCol < 3 || clickRow > 2) return false;
                    }
                }
                else
                {
                    return false;
                }
                break;
            case ChessManName.Pao:
                if (this.col != clickCol && this.row != clickRow)
                {
                    return false;
                }
                else if (this.row == clickRow)
                {
                    int counter = 0;
                    for (int i = this.col > clickCol ? clickCol + 1 : this.col + 1; i < (this.col < clickCol ? clickCol  : this.col ); i++)
                    {

                        if (GameManager.instance.chessmanPut[row, i])
                            counter ++;

                    }
                    // counter 为零是直接移动
                    Debug.Log(counter);
                    Debug.Log((counter == 0 && !GameManager.instance.chessmanPut[clickRow, clickCol]));
                    Debug.Log((counter == 1 && GameManager.instance.chessmanPut[clickRow, clickCol]));
                    if (!((counter == 0 && !GameManager.instance.chessmanPut[clickRow, clickCol]) || (counter == 1 && GameManager.instance.chessmanPut[clickRow, clickCol])))
                        return false;
                }
                else if (this.col == clickCol)
                {

                    int counter = 0;
                    for (int i = this.row > clickRow ? clickRow + 1 : this.row + 1; i < (this.row < clickRow ? clickRow : this.row ); i++)
                    {
                        if (GameManager.instance.chessmanPut[i, this.col]) 
                            counter ++;
                    }
                    Debug.Log(counter);
                    Debug.Log((counter == 0 && !GameManager.instance.chessmanPut[clickRow, clickCol]));
                    Debug.Log((counter == 1 && GameManager.instance.chessmanPut[clickRow, clickCol]));
                    if (!((counter == 0 && !GameManager.instance.chessmanPut[clickRow, clickCol]) || (counter == 1 && GameManager.instance.chessmanPut[clickRow, clickCol])))
                        return false;
                }
                break;
            case ChessManName.Bing:
                if(this.black)
                {
                    if (this.row == clickRow + 1 && this.col == clickCol && this.row > 4)
                    {

                    } else if ((this.row == clickRow + 1 || Mathf.Abs(this.col - clickCol) == 1) && this.row < 5)
                    {

                    } else
                    {
                        return false;
                    }
                } else
                {
                    if (this.row == clickRow - 1 && this.col == clickCol && this.row < 5)
                    {

                    }
                    else if ((this.row == clickRow - 1 || Mathf.Abs(this.col - clickCol) == 1) && this.row >4)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
                break;

        }
        return true;
    }

}
