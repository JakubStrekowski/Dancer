using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectsManager : MonoBehaviour
{
    private SetCustomArrowColor[] movementArrows;

    void Awake()
    {
        movementArrows = new SetCustomArrowColor[4];
        movementArrows[(int)MoveTypeEnum.Up] = GameObject.Find("NoteUpUI").GetComponent<SetCustomArrowColor>();
        movementArrows[(int)MoveTypeEnum.Down] = GameObject.Find("NoteDownUI").GetComponent<SetCustomArrowColor>();
        movementArrows[(int)MoveTypeEnum.Left] = GameObject.Find("NoteLeftUI").GetComponent<SetCustomArrowColor>();
        movementArrows[(int)MoveTypeEnum.Right] = GameObject.Find("NoteRightUI").GetComponent<SetCustomArrowColor>();
    }

    public void SetArrowColor(MoveTypeEnum moveTypeEnum, Color color)
    {
        movementArrows[(int)moveTypeEnum].SetColor(color);
    }

    
}
