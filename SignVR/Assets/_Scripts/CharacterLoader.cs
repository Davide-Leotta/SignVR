using UnityEngine;
using System.Collections.Generic;

public class CharacterLoader : MonoBehaviour
{

    [Header("Riferimenti")]
    public Animator mainAnimator;
    public Transform modelHolder;
    [Header("Lista Modelli")]
    public List<GameObject> characterPrefabs;

    void Start()
    {
        LoadCharacter();
    }

    public void LoadCharacter()
    {
        // Recupera l'indice salvato nelle preferenze (fallback a 0)
        int characterIndex = PlayerPrefs.GetInt("Setting_AvatarID", 0);

        // Validazione dell'indice per evitare errori out-of-bounds
        if (characterIndex < 0 || characterIndex >= characterPrefabs.Count)
        {
            Debug.LogError("Indice personaggio non valido! Carico il default (0).");
            characterIndex = 0;
        }

        // Rimuove eventuali modelli precedenti presenti nel contenitore
        if (modelHolder.childCount > 0)
        {
            foreach (Transform child in modelHolder)
            {
                Destroy(child.gameObject);
            }
        }

        // Istanzia il nuovo modello e resetta la trasformazione locale
        GameObject newModel = Instantiate(characterPrefabs[characterIndex], modelHolder);
        
        newModel.transform.localPosition = Vector3.zero;
        newModel.transform.localRotation = Quaternion.identity;

        // Aggiornamento dell'Animator principale con il nuovo Avatar
        Animator prefabAnimator = newModel.GetComponent<Animator>();

        if (prefabAnimator != null && prefabAnimator.avatar != null)
        {
            // Assegna il nuovo Avatar all'Animator che gestisce la logica
            mainAnimator.avatar = prefabAnimator.avatar;
            
            // Rebind è necessario per ricollegare la State Machine alle ossa del nuovo modello
            mainAnimator.Rebind();
        }
        else
        {
            Debug.LogWarning("Il prefab istanziato non ha un componente Animator o un Avatar assegnato. Le animazioni potrebbero non funzionare.");
        }
    }
}