using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Oculus.Voice.Dictation;

public class VoiceDictationManager : MonoBehaviour
{
    [Header("Voice Engine")]
    [SerializeField] private AppDictationExperience dictationExperience;

    [Header("UI Interface")]
    [SerializeField] private TMP_Text transcriptionText;
    [SerializeField] private Image microphoneStatusImage;

    [Header("Debug")]
    [SerializeField] private string debugInput;

    [Header("LLM Integration")]
    [SerializeField] private LlmTranslationHandler llmHandler;

    private bool _isRecording = false;

    private void Start()
    {
        if (dictationExperience == null)
        {
            Debug.LogError("VoiceDictationManager: Dictation Experience non assegnato!");
            return;
        }

        // Iscrizione agli eventi del motore di dettatura
        dictationExperience.DictationEvents.OnFullTranscription.AddListener(OnFullTranscriptionReceived);
        dictationExperience.DictationEvents.OnPartialTranscription.AddListener(OnPartialTranscriptionReceived);
        dictationExperience.DictationEvents.OnStoppedListening.AddListener(ResetVoiceUI);
    }

    private void Update()
    {
        // Debug da tastiera
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrWhiteSpace(debugInput))
        {
            SendDebugInput();
        }
    }

    private void OnDestroy()
    {
        if (dictationExperience != null)
        {
            dictationExperience.DictationEvents.OnFullTranscription.RemoveListener(OnFullTranscriptionReceived);
            dictationExperience.DictationEvents.OnPartialTranscription.RemoveListener(OnPartialTranscriptionReceived);
            dictationExperience.DictationEvents.OnStoppedListening.RemoveListener(ResetVoiceUI);
        }
    }

    // Simula una dettatura usando la stringa di debug
    // ContextMenu permette di invocarlo tramite l'Inspector.
    [ContextMenu("Simula Dettatura Debug")]
    public void SendDebugInput()
    {
        if (string.IsNullOrWhiteSpace(debugInput))
        {
            Debug.LogWarning("VoiceDictationManager: Il campo Debug Input è vuoto.");
            return;
        }

        Debug.Log($"[DEBUG] Simulazione input vocale: {debugInput}");
        
        // Inietta l'input simulato nel flusso standard
        OnFullTranscriptionReceived(debugInput);
    }

    public void ToggleDictation()
    {
        if (_isRecording)
            StopRecording();
        else
            StartRecording();
    }

    private void StartRecording()
    {
        // Feedback visivo (Verde) e attivazione motore
        UpdateUIState(Color.green);
        _isRecording = true;
        if (dictationExperience == null) return;

        dictationExperience.Activate();
    }

    private void StopRecording()
    {
        // Feedback visivo (Bianco) e disattivazione motore
        UpdateUIState(Color.white);
        _isRecording = false;
        if (dictationExperience == null) return;

        dictationExperience.Deactivate();
    }

    private void OnPartialTranscriptionReceived(string text)
    {
        if (transcriptionText != null)
            transcriptionText.text = text + "...";
    }

    private void OnFullTranscriptionReceived(string text)
    {
        if (transcriptionText != null)
            transcriptionText.text = text;

        Debug.Log($"Testo ricevuto (Voce o Debug): {text}");

        // Inoltra il testo trascritto al gestore LLM per la traduzione
        if (llmHandler != null && !string.IsNullOrWhiteSpace(text))
        {
            llmHandler.SendTranslationRequest(text);
        }
    }

    private void ResetVoiceUI()
    {
        _isRecording = false;
        UpdateUIState(Color.white);
    }

    private void UpdateUIState(Color stateColor)
    {
        if (microphoneStatusImage != null)
            microphoneStatusImage.color = stateColor;

        if (transcriptionText != null)
            transcriptionText.faceColor = stateColor; 
    }
}