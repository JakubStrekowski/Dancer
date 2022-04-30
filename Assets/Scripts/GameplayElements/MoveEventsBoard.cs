using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveEventsBoard : MonoBehaviour
{
    private readonly float INITIAL_POS_X = 14f;

    [SerializeField]
    public Slider songProgress;

    public void OnProgressUpdate()
    {
        transform.localPosition = new Vector3(
            INITIAL_POS_X  + (-songProgress.value * Constants.BASE_SPEED),
            0, 
            0);
    }

    public void ClearAllMoves()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
