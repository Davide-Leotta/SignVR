using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ImmersiveTutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text tutorialText;
    public GameObject tutorialCanvasPanel;
    public Button playButton;    
    public Button prevButton;     
    public Button nextButton;     

    [System.Serializable]
    public class TutorialStep
    {
        [TextArea(3, 5)]
        public string spiegazione;
        
        [Header("Trascinare gli oggetti da inserire per lo step del tutorial")]
        public GameObject[] oggettiDaAttivare; 
    }

    [Header("Configurazione Passaggi")]
    public TutorialStep[] passiDelTutorial;

    private int indiceCorrente = 0;

    void Start()
    {
        if (passiDelTutorial.Length == 0) return;

        if (tutorialCanvasPanel != null) tutorialCanvasPanel.SetActive(true);

        playButton.onClick.AddListener(IniziaTutorial);
        prevButton.onClick.AddListener(VaiIndietro);
        nextButton.onClick.AddListener(VaiAvanti);

        // Assicura che tutti gli oggetti di tutti gli step siano nascosti all'avvio
        foreach (var passo in passiDelTutorial)
        {
            foreach(var obj in passo.oggettiDaAttivare)
            {
                if(obj != null) obj.SetActive(false);
            }
        }

        indiceCorrente = 0;
        MostraPassoCorrente();

        playButton.gameObject.SetActive(true);
        prevButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    void IniziaTutorial()
    {
        playButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        AggiornaBottoni();
        VaiAvanti();
    }

    void VaiAvanti()
    {
        if (indiceCorrente < passiDelTutorial.Length - 1)
        {
            indiceCorrente++;
            MostraPassoCorrente();
            AggiornaBottoni();
        }
        else
        {
            ChiudiTutorial();
        }
    }

    void VaiIndietro()
    {
        if (indiceCorrente > 0)
        {
            indiceCorrente--;
            MostraPassoCorrente();
            AggiornaBottoni();
        }
    }

    void MostraPassoCorrente()
    {

        tutorialText.text = passiDelTutorial[indiceCorrente].spiegazione;

        //Disattiva preventivamente tutti gli oggetti per pulire la scena
        foreach (var passo in passiDelTutorial)
        {
            foreach (var obj in passo.oggettiDaAttivare)
            {
                if (obj != null) obj.SetActive(false);
            }
        }

        // Attiva solo gli oggetti dello step corrente
        foreach (var obj in passiDelTutorial[indiceCorrente].oggettiDaAttivare)
        {
            if (obj != null) obj.SetActive(true);
        }
    }

    void AggiornaBottoni()
    {
        prevButton.gameObject.SetActive(indiceCorrente > 0);
        nextButton.gameObject.SetActive(true);
    }

    void ChiudiTutorial()
    {   
        // Disattivazione finale degli oggetti rimasti attivi dall'ultimo step
        foreach (var obj in passiDelTutorial[indiceCorrente].oggettiDaAttivare)
        {
            if (obj != null) obj.SetActive(false);
        }

        nextButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);

        Debug.Log("Tutorial Finito");
    }
}