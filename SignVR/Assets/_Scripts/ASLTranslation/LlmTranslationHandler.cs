using UnityEngine;
using Meta.XR.BuildingBlocks.AIBlocks;
using TMPro;
using System.Threading.Tasks;

public class LlmTranslationHandler : MonoBehaviour
{
    [Header("LLM Integration")]
    [SerializeField] private LlmAgent llmAgent;

    [Header("Animator Handler")]
    [SerializeField] private AvatarAnimatorHandler animatorHandler;

    [Header("UI Display")]
    [SerializeField] private TMP_Text aslGlossText;

// Prompt di sistema per istruire il modello sulle regole grammaticali ASL (Gloss) e formattazione
    private const string SYSTEM_PROMPT = @"You are an expert American Sign Language (ASL) translator. Your task is to translate standard English text into ASL Gloss notation. 
                            ### GLOSSING RULES:
                            1. FORMAT: Output ONLY the translated glosses in UPPERCASE. No explanations, no punctuation, no question marks, no numbers.
                            2. GRAMMAR STRUCTURE: Use the order: TIME + OBJECT + SUBJECT + VERB + QUESTION-WORD/NEGATION.
                            3. OMISSIONS: Remove all articles (a, an, the), remove all 'to be' verbs (am, is, are, was, were), and remove prepositions (to, of) unless necessary for direction.
                            4. VERBS & NOUNS: Use the root form of words. Remove '-ing', '-ed', and plural 's'. (e.g., 'cats' -> 'CAT', 'walking' -> 'WALK').
                            5. ADJECTIVES: Place adjectives AFTER the noun (e.g., 'Red car' -> 'CAR RED').
                            6. NEGATION: Place 'NOT' or 'NONE' at the END of the sentence or immediately before the verb.
                            7. QUESTIONS: Place WH-words (WHO, WHAT, WHERE, WHEN, WHY, HOW) at the VERY END.
                            8. PRONOUNS (CRITICAL): Preserve the exact point of view. If input is 'YOU', output is 'YOU'. Do NOT answer the question. Translate exactly what is written.
                            9. GREETINGS: Do not remove greetings such as hi or hello.

                            ### EXAMPLES (Few-Shot Learning):
                            Input: I am going to the store tomorrow.
                            Output: TOMORROW STORE I GO

                            Input: What is your name?
                            Output: YOUR NAME WHAT

                            Input: The blue car stopped.
                            Output: CAR BLUE STOP

                            Input: I have two dogs.
                            Output: DOG TWO I HAVE

                            Input: I dont have any cats
                            Output: CAT I HAVE NONE

                            ### TASK:
                            Translate the following English text to ASL Gloss strictly following the rules above.

                            Input: ";

    // Costruisce il prompt completo e invia la richiesta asincrona all'agente AI.
    public async void SendTranslationRequest(string englishText)
    {
        if (llmAgent == null)
        {
            Debug.LogError("LlmTranslationHandler: LlmAgent non assegnato.");
            return;
        }

        // Unisce le istruzioni base con l'input utente, ripulendo eventuali caratteri di formattazione
        string fullPrompt = (SYSTEM_PROMPT + englishText).Replace("\r", "");

        try
        {
            await llmAgent.SendTextOnlyAsync(fullPrompt);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Errore durante la richiesta LLM: {e.Message}");
            if (aslGlossText != null) aslGlossText.text = "Error translating.";
        }
    }

    // Gestisce la risposta testuale: aggiorna la UI e delega l'animazione all'animation handler
    private void HandleLlmResponse(string response)
    {
        response = response.Trim();

        if (aslGlossText != null)
        {
            aslGlossText.text = response;
        }

        if (animatorHandler != null)
        {
            animatorHandler.ProcessAndAnimate(response);
        }
    }
}