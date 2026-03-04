using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

// Gestisce la logica del menu impostazioni, salvataggio e caricamento dati tramite PlayerPrefs.
public class SettingsMenuLogic : MonoBehaviour
{
    [Header("UI Toggles")]
    public Toggle avatarToggle;
    public Toggle aslPanelToggle;
    public Toggle liveTranscriptToggle;
    public Toggle headTrackingToggle;

    [Header("Avatar Selection")]
    public ToggleGroup avatarSelection; 

    void Start()
    {
        InitializeBooleanSettings();
        InitializeAvatarSelection();
    }

    private void InitializeBooleanSettings()
    {
        // Carica gli stati salvati: default ON (1) per core features, OFF (0) per HeadTrack
        avatarToggle.isOn = PlayerPrefs.GetInt("Setting_Avatar", 1) == 1;
        aslPanelToggle.isOn = PlayerPrefs.GetInt("Setting_ASL", 1) == 1;
        liveTranscriptToggle.isOn = PlayerPrefs.GetInt("Setting_Live", 1) == 1;
        headTrackingToggle.isOn = PlayerPrefs.GetInt("Setting_HeadTrack", 0) == 1;

        // Binding eventi onValueChanged
        avatarToggle.onValueChanged.AddListener(SaveAvatar);
        aslPanelToggle.onValueChanged.AddListener(SaveASL);
        liveTranscriptToggle.onValueChanged.AddListener(SaveLive);
        headTrackingToggle.onValueChanged.AddListener(SaveHeadTrack);
    }

    // Gestisce l'inizializzazione dinamica del ToggleGroup per la scelta dell'avatar.
    
    private void InitializeAvatarSelection()
    {
        int savedAvatarID = PlayerPrefs.GetInt("Setting_AvatarID", 0);
        Toggle[] dropdownToggles = avatarSelection.GetComponentsInChildren<Toggle>();

        for (int i = 0; i < dropdownToggles.Length; i++)
        {
            Toggle t = dropdownToggles[i];
            int index = i;

            // Ripristino stato salvato
            if (i == savedAvatarID)
            {
                t.isOn = true;
            }

            // Listener per il salvataggio dell'ID alla selezione
            t.onValueChanged.AddListener((isSelected) => {
                if (isSelected) 
                {
                    SaveAvatarID(index);
                }
            });
        }
    }

    #region Persistence Methods

    public void SaveAvatar(bool value)
    {
        PlayerPrefs.SetInt("Setting_Avatar", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveASL(bool value)
    {
        PlayerPrefs.SetInt("Setting_ASL", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveLive(bool value)
    {
        PlayerPrefs.SetInt("Setting_Live", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveHeadTrack(bool value)
    {
        PlayerPrefs.SetInt("Setting_HeadTrack", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveAvatarID(int id)
    {
        PlayerPrefs.SetInt("Setting_AvatarID", id);
        PlayerPrefs.Save();
        Debug.Log($"[Settings] Avatar ID saved: {id}");
    }

    #endregion
}