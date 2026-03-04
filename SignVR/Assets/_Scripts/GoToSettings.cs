using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;

public class GoToSettings : MonoBehaviour
{
    public SceneSwitch sceneSwitch;
    public void ToSettings()
    {
        StartCoroutine(WaitAndChangeScene());
    }
    private IEnumerator WaitAndChangeScene()
    {
        yield return new WaitForSeconds(1.5f);
        sceneSwitch.ChangeScene("Settings");
    }
}