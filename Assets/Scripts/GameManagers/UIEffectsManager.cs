using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEffectsManager : MonoBehaviour
{
    private SetCustomArrowColor[] movementArrows;
    private SpriteRenderer background;
    private Image panelUi;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI instructionsText;
    private TextMeshProUGUI titleText;
    private Image songProgress;
    private Image songProgressBg;

    void Awake()
    {
        movementArrows = new SetCustomArrowColor[4];
        movementArrows[(int)MoveTypeEnum.Up] = GameObject.Find("NoteUpUI").GetComponent<SetCustomArrowColor>();
        movementArrows[(int)MoveTypeEnum.Down] = GameObject.Find("NoteDownUI").GetComponent<SetCustomArrowColor>();
        movementArrows[(int)MoveTypeEnum.Left] = GameObject.Find("NoteLeftUI").GetComponent<SetCustomArrowColor>();
        movementArrows[(int)MoveTypeEnum.Right] = GameObject.Find("NoteRightUI").GetComponent<SetCustomArrowColor>();
        background = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        panelUi = GameObject.Find("UIScorePanel").GetComponent<Image>();
        scoreText = GameObject.Find("MissesTxt").GetComponent<TextMeshProUGUI>();
        instructionsText = GameObject.Find("ButtonInfo").GetComponent<TextMeshProUGUI>();
        titleText = GameObject.Find("TitleInfo").GetComponent<TextMeshProUGUI>();
        songProgress = GameObject.Find("ProgressFill").GetComponent<Image>();
        songProgressBg = GameObject.Find("ProgressBackground").GetComponent<Image>();
    }

    public void SetArrowColor(MoveTypeEnum moveTypeEnum, Color color)
    {
        movementArrows[(int)moveTypeEnum].SetColor(color);
    }

    public void SetBackgroundColor(Color color)
    {
        background.color = color;
    }

    public void SetPanelUiColor(Color color)
    {
        panelUi.color = color;
    }
    public void SetTextColor(Color color)
    {
        scoreText.color = color;
        instructionsText.color = color;
        titleText.color = color;
        songProgress.color = color;
        songProgressBg.color = new Color(color.r * 0.5f, color.g * 0.5f, color.b * 0.5f, color.a);
    }
}
