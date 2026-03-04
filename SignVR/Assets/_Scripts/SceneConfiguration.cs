using UnityEngine;

public class SceneConfiguration : MonoBehaviour
{
    public GameObject avatarObject;
    public GameObject aslPanelObject;
    public GameObject livePanelObject;
    public LazyFollow headTrackingScript;
    void Start()
    {
        ApplySettings();
    }

    void ApplySettings()
    {
        // 1. Gestione Avatar
        bool showAvatar = PlayerPrefs.GetInt("Setting_Avatar", 1) == 1;
        if (avatarObject != null) 
            avatarObject.SetActive(showAvatar);

        // 2. Gestione ASL Panel
        bool showASL = PlayerPrefs.GetInt("Setting_ASL", 1) == 1;
        if (aslPanelObject != null) 
            aslPanelObject.SetActive(showASL);

        // 3. Gestione Live Transcription
        bool showLive = PlayerPrefs.GetInt("Setting_Live", 1) == 1;
        if (livePanelObject != null) 
            livePanelObject.SetActive(showLive);

        // 4. Gestione Head Tracking
        bool useHeadTracking = PlayerPrefs.GetInt("Setting_HeadTrack", 0) == 1;
        if (headTrackingScript != null)
            headTrackingScript.enabled = useHeadTracking;
    }
}