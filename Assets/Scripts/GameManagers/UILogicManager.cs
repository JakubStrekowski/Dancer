using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UILogicManager : MonoBehaviour
{
    public TextMeshProUGUI missesTxt;

    public void UpdateMissesUI(int currentMisses, int totalMoves)
    {
        missesTxt.text = "Misses: " + currentMisses + '/' + totalMoves;
    }
}
