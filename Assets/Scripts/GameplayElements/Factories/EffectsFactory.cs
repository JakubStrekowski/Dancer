using MIDIparser.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MIDIparser.Models.VisualEventsSubclasses;
using System.Collections.ObjectModel;
using System;
using UnityEngine.Networking;

public class EffectsFactory : MonoBehaviour
{
    [SerializeField]
    GameObject visualEffectObject;

    private string MUSIC_PATH;
    List<VisualEffectSprite> visualObjects;

    private void Awake()
    {
        MUSIC_PATH = Application.dataPath + "/Resources/Music/";
    }

    public List<VisualEventBase> GenerateVisualEffectObjects(DancerEvents dancerEvents, float ticksPerSpeed)
    {
        visualObjects = new List<VisualEffectSprite>();

        List<VisualEventBase> creationEvents = dancerEvents.visualEvents.Where(x => x.eventType == VisualEventTypeEnum.CreateObject).ToList();

        List<VisualEventBase> orderedList = dancerEvents.visualEvents.OrderBy(x => x.startTime).ToList();
        dancerEvents.visualEvents = new Collection<VisualEventBase>(orderedList);

        foreach (VisualEventBase visualEvent in creationEvents)
        {
            GameObject newObject = Instantiate(visualEffectObject,
                new Vector3(float.Parse(visualEvent.paramsList[(int)CreateParamsEnum.posX]), float.Parse(visualEvent.paramsList[(int)CreateParamsEnum.posY]), 0),
                Quaternion.identity);
            DownloadImage(MUSIC_PATH + GameMaster.Instance.musicLoader.DancerSongParsed.title + "/" + visualEvent.paramsList[(int)CreateParamsEnum.spritePath],newObject);
            newObject.GetComponent<VisualEffectSprite>().SetParameters(ticksPerSpeed);

            visualObjects.Add(newObject.GetComponent<VisualEffectSprite>());
        }

        return dancerEvents.visualEvents.ToList();
    }

    public void ResolveEffect(VisualEventBase effect)
    {
        switch(effect.eventType)
        {
            case VisualEventTypeEnum.CreateObject:
                {
                    visualObjects[effect.objectId].Activate();
                    break;
                }
            case VisualEventTypeEnum.DeleteObject:
                {
                    visualObjects[effect.objectId].Deactivate();
                    break;
                }
            case VisualEventTypeEnum.ChangeColorObjectLinear:
                {
                    ChangeColorLinearVisualEffect parsedFx = (ChangeColorLinearVisualEffect)effect;
                    ArgbColor argb = new ArgbColor(Byte.Parse(parsedFx.paramsList[(int)VisualEventsSubclassesParamsEnum.alpha]),
                        Byte.Parse(parsedFx.paramsList[(int)VisualEventsSubclassesParamsEnum.red]),
                        Byte.Parse(parsedFx.paramsList[(int)VisualEventsSubclassesParamsEnum.green]),
                        Byte.Parse(parsedFx.paramsList[(int)VisualEventsSubclassesParamsEnum.blue]));
                    visualObjects[effect.objectId].ChangeColorLinear(argb.ToUnityColor(),float.Parse(parsedFx.paramsList[(int)VisualEventsSubclassesParamsEnum.timeToReach]));
                    break;
                }
            case VisualEventTypeEnum.ChangeColorObjectArc:
                {
                    ChangeColorArcVisualEffect parsedFx = (ChangeColorArcVisualEffect)effect;
                    ArgbColor argb = new ArgbColor(Byte.Parse(parsedFx.paramsList[(int)VisualEventsSubclassesParamsEnum.alpha]),
                        Byte.Parse(parsedFx.paramsList[(int)VisualEventsSubclassesParamsEnum.red]),
                        Byte.Parse(parsedFx.paramsList[(int)VisualEventsSubclassesParamsEnum.green]),
                        Byte.Parse(parsedFx.paramsList[(int)VisualEventsSubclassesParamsEnum.blue]));
                    visualObjects[effect.objectId].ChangeColorArc(argb.ToUnityColor(), float.Parse(parsedFx.paramsList[(int)VisualEventsSubclassesParamsEnum.timeToReach]));
                    break;
                }
            case VisualEventTypeEnum.ChangePosObjectLinear:
                {
                    ChangePositionLinearVisualEffect parsedFx = (ChangePositionLinearVisualEffect)effect;
                    Vector2 newPos = new Vector2(float.Parse(parsedFx.paramsList[(int)ChangePositionLinearParamsEnum.posX]),
                        float.Parse(parsedFx.paramsList[(int)ChangePositionLinearParamsEnum.posY]));
                    visualObjects[effect.objectId].MoveTowardsLinear(newPos, float.Parse(parsedFx.paramsList[(int)ChangePositionLinearParamsEnum.duration]));
                    break;
                }
            case VisualEventTypeEnum.ChangePosObjectArc:
                {
                    ChangePositionDampingVisualEffect parsedFx = (ChangePositionDampingVisualEffect)effect;
                    Vector2 newPos = new Vector2(float.Parse(parsedFx.paramsList[(int)ChangePositionLinearParamsEnum.posX]),
                        float.Parse(parsedFx.paramsList[(int)ChangePositionLinearParamsEnum.posY]));
                    visualObjects[effect.objectId].MoveTowardsDamping(newPos, float.Parse(parsedFx.paramsList[(int)ChangePositionLinearParamsEnum.duration]));
                    break;
                }
            case VisualEventTypeEnum.ChangeRotObjectLinear:
                {
                    ChangeRotationLinearVisualEffect parsedFx = (ChangeRotationLinearVisualEffect)effect;
                    visualObjects[effect.objectId].RotateLinear(float.Parse(parsedFx.paramsList[(int)ChangePotationLinearParamsEnum.rotation]), float.Parse(parsedFx.paramsList[(int)ChangePositionLinearParamsEnum.duration]));
                    break;
                }
            case VisualEventTypeEnum.ChangeRotObjectArc:
                {
                    ChangeRotationArcVisualEffect parsedFx = (ChangeRotationArcVisualEffect)effect;
                    visualObjects[effect.objectId].RotateArc(float.Parse(parsedFx.paramsList[(int)ChangePotationLinearParamsEnum.rotation]), float.Parse(parsedFx.paramsList[(int)ChangePositionLinearParamsEnum.duration]));
                    break;
                }
            default:break;
        }
    }

    private void DownloadImage(string url, GameObject visualEffect)
    {
        StartCoroutine(GameMaster.Instance.musicLoader.ImageRequest(url, (UnityWebRequest www) =>
        {
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log($"{www.error}: {www.downloadHandler.text}");
            }
            else
            {
                // Get the texture out using a helper downloadhandler
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                // Save it into the Image UI's sprite
                visualEffect.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }));
    }
}
