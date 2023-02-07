using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PositionScript : MonoBehaviour, IPointerClickHandler
{
    // this is to give it a row and column
    public int row, column;
    public void OnPointerClick(PointerEventData eventData)
       
    {
        GameObject position = GameManager.instance.selected;
        if (position != null)
        position.GetComponent<ChessMan>().stateGoes(this.column, this.row, null);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void intPosition(int row, int col)
    {
        this.row = row;
        this.column = col;
    }
}
