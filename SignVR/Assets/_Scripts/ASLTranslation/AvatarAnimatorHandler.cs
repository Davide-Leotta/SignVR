using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvatarAnimatorHandler : MonoBehaviour
{
    [Header("Avatar & Timing")]
    [SerializeField] private Animator avatarAnimator;
    
    [Tooltip("Frame rate delle clip di animazione")]
    public float frameRate = 60f;
    
    [Tooltip("Durata totale in frame delle clip")]
    public float totalFramesClip = 120f;
    
    public float crossFadeTime = 0.1f;

    // Coda FIFO per gestire sequenze di frasi senza interruzioni
    private Queue<string> animationQueue = new Queue<string>();
    private bool isAnimating = false;

    // Accoda la frase e avvia la coroutine di gestione coda se non è già in esecuzione
    public void ProcessAndAnimate(string phrase)
    {
        animationQueue.Enqueue(phrase);

        if (!isAnimating)
        {
            StartCoroutine(ProcessQueueRoutine());
        }
    }

    // Processa la coda delle frasi una alla volta, attendendo il completamento di ciascuna
    private IEnumerator ProcessQueueRoutine()
    {
        isAnimating = true;

        while (animationQueue.Count > 0)
        {
            string nextPhrase = animationQueue.Dequeue();
            
            yield return StartCoroutine(AnimatePhraseRoutine(nextPhrase));
        }

        // Ritorno allo stato Idle alla fine della sequenza completa di frasi
        if (avatarAnimator != null) 
            avatarAnimator.CrossFade("Idle", 0.2f);

        isAnimating = false;
    }

    private IEnumerator AnimatePhraseRoutine(string phrase)
    {
        phrase = phrase.ToUpper();
        string[] words = phrase.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];

            bool isFirstWord = (i == 0);
            bool isLastWord = (i == words.Length - 1);

            // Hash precalcolato per verifica performante nel controller
            int wordHash = Animator.StringToHash(word);
            
            // Verifica se esiste un'animazione specifica per l'intera parola
            bool fullWordExists = avatarAnimator.HasState(0, wordHash);

            if (fullWordExists)
            {
                yield return StartCoroutine(PlayWordAnimation(word, isFirstWord, isLastWord));
            }
            else
            {
                // Fallback: esegue lo spelling lettera per lettera se manca la parola intera
                yield return StartCoroutine(PlayWordSpelling(word));
            }
        }
    }

    private IEnumerator PlayWordAnimation(string stateName, bool isFirst, bool isLast)
    {
        float startFrame = 0;
        float durationFrames = 0;

        // Calcolo start/duration per gestire Entry (inizio), Hold (centro) ed Exit (fine)
        // ritagliando le porzioni di clip non necessarie in base alla posizione
        if (isFirst && isLast) // Frase di una sola parola
        { 
            startFrame = 0; 
            durationFrames = totalFramesClip; 
        }
        else if (isFirst) // Prima parola della frase (Entry + Hold)
        { 
            startFrame = 0; 
            durationFrames = 90; // Taglia l'uscita
        }
        else if (isLast) // Ultima parola della frase (Hold + Exit)
        { 
            startFrame = 30; // Salta l'entrata
            durationFrames = totalFramesClip - 30;
        }
        else // Parola centrale (Solo Hold)
        { 
            startFrame = 30; // Salta entrata
            durationFrames = 60; // Esegui solo parte centrale, taglia uscita
        }

        // Offset normalizzato (0-1) richiesto dal CrossFade per partire dal frame corretto
        float normalizedStartOffset = startFrame / totalFramesClip;
        
        avatarAnimator.CrossFade(stateName, crossFadeTime, 0, normalizedStartOffset);
        
        // Calcola il tempo di attesa basato sul taglio effettuato
        float waitTime = durationFrames / frameRate;
        
        yield return new WaitForSeconds(waitTime);
    }

    private IEnumerator PlayWordSpelling(string word)
    {
        char[] letters = word.ToCharArray();
        
        for (int i = 0; i < letters.Length; i++)
        {
            char currentLetter = letters[i];
            
            if (char.IsLetter(currentLetter))
            {
                string letterStateName = currentLetter.ToString();
                float startFrame = 0;
                float durationFrames = 0;
                
                bool isFirst = (i == 0);
                bool isLast = (i == letters.Length - 1);

                // Timing specifici per le lettere (diversi dalle parole intere)
                if (isFirst && isLast)
                {
                    startFrame = 0; 
                    durationFrames = totalFramesClip; 
                }
                else if (isFirst)
                { 
                    startFrame = 0; 
                    durationFrames = 80;
                }
                else if (isLast)
                { 
                    startFrame = 40; 
                    durationFrames = totalFramesClip - 40; 
                }
                else
                { 
                    startFrame = 40; 
                    durationFrames = 40;
                }

                float normalizedStartOffset = startFrame / totalFramesClip;
                
                avatarAnimator.CrossFade(letterStateName, crossFadeTime, 0, normalizedStartOffset);

                float waitTime = durationFrames / frameRate;
                yield return new WaitForSeconds(waitTime);
            }
        }
    }
}