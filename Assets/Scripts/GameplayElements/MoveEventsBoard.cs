using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveEventsBoard : MonoBehaviour
{
    private readonly float INITIAL_POS_X = 14f;
    private readonly float INITIAL_POS_Y = 7.6f;

    private readonly float CAMERA_POS_Y = 3f;
    private readonly float CAMERA_POS_Z = -10f;

    private readonly float REVERSED_POS_X = 54f;

    [SerializeField]
    private Slider songProgress;
    [SerializeField]
    private GameObject gameplayCamera;
    [SerializeField]
    private EditorStateManager stateManager;

    public void Start()
    {
        transform.position = new Vector3(
            INITIAL_POS_X,
            INITIAL_POS_Y,
            0);
    }

    public void OnProgressUpdate()
    {
        if (stateManager is null)
        {
            gameplayCamera.transform.position = new Vector3(
                -INITIAL_POS_X + songProgress.value * Constants.BASE_SPEED,
                CAMERA_POS_Y,
                CAMERA_POS_Z);
            return;
        }

        if (stateManager.EditorState == EEditorStates.editMovesPlay)
        {
            gameplayCamera.transform.position = new Vector3(
                REVERSED_POS_X - songProgress.value * Constants.BASE_SPEED,
                CAMERA_POS_Y,
                CAMERA_POS_Z);
        }
        else
        {
            gameplayCamera.transform.position = new Vector3(
                -INITIAL_POS_X + songProgress.value * Constants.BASE_SPEED,
                CAMERA_POS_Y,
                CAMERA_POS_Z);
        }
    }

    public void ClearAllMoves()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void FlipObject(bool mirrored)
    {
        transform.localScale = new Vector3(
                    mirrored? -1 : 1,
                    1,
                    1);
    }
}
