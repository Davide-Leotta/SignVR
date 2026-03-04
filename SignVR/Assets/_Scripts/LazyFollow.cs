using UnityEngine;

public class LazyFollow : MonoBehaviour
{
    [Header("Target & Settings")]
    public Transform target; // Target da seguire (HeadCenter)

    [Header("Deadzone Settings")]
    [Tooltip("L'angolo massimo prima che il menu inizi a seguire la testa.")]
    [Range(0f, 90f)]
    public float deadzoneAngle = 25.0f; 

    [Tooltip("La distanza massima prima che il menu ti segua")]
    public float deadzoneDistance = 0.5f;

    [Tooltip("Quanto velocemente il menu scatta quando esce dalla deadzone.")]
    public float followSpeedMultiplier = 1.0f;

    [Header("Position Settings")]
    public Vector3 positionOffset = new Vector3(0, 0, 1.0f);
    public float posSmoothTime = 0.3f;

    [Space(5)]
    public bool lockPosX = false; 
    public bool lockPosY = false; // Se TRUE, mantiene l'altezza originale ignorando il target
    public bool lockPosZ = false; 

    [Header("Rotation Settings")]
    public Vector3 rotationOffset = Vector3.zero;
    public float rotSpeed = 5.0f;
    
    [Space(5)]
    public bool rotateOnlyWhileMoving = true; // Se TRUE, ruota solo quando si riposiziona
    public bool lockRotX = true;  
    public bool lockRotY = false; 
    public bool lockRotZ = true;  

    private Vector3 velocity = Vector3.zero;
    private bool isFollowing = false;
    private Vector3 targetDestination;

    void Start()
    {
        if (target != null)
        {
            ForceUpdatePosition();
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Calcolo della posizione ideale relativa al target
        Vector3 idealPos = target.TransformPoint(positionOffset);

        // Applicazione dei blocchi (Locks) sugli assi specifici
        float destX = lockPosX ? transform.position.x : idealPos.x;
        float destY = lockPosY ? transform.position.y : idealPos.y;
        float destZ = lockPosZ ? transform.position.z : idealPos.z;
        
        Vector3 finalWantedPos = new Vector3(destX, destY, destZ);

        // 2. Controllo deadzone 
        // Calcoliamo la distanza e l'angolo rispetto alla posizione attuale dell'oggetto
        float distance = Vector3.Distance(transform.position, finalWantedPos);
        
        // Calcolo dell'angolo tra la direzione dello sguardo e la posizione dell'oggetto
        Vector3 dirToMenu = (transform.position - target.position).normalized;
        float angle = Vector3.Angle(target.forward, dirToMenu);

        if (!isFollowing)
        {
            // Attiva l'inseguimento solo se si superano le soglie di tolleranza
            if (angle > deadzoneAngle || distance > deadzoneDistance)
            {
                isFollowing = true;
            }
        }

        // 3. Movimento (Se attivo)
        if (isFollowing)
        {
            // Aggiorniamo la destinazione continuamente mentre seguiamo
            targetDestination = finalWantedPos;

            // SmoothDamp per un movimento fluido e naturale
            transform.position = Vector3.SmoothDamp(transform.position, targetDestination, ref velocity, posSmoothTime / followSpeedMultiplier);

            // Stop al raggiungimento quasi perfetto del target
            if (Vector3.Distance(transform.position, targetDestination) < 0.01f && angle < 1.0f)
            {
                isFollowing = false;
            }
        }

        // 4. Rotazione
        if (isFollowing || !rotateOnlyWhileMoving)
        {
            HandleRotation();
        }
    }

    void HandleRotation()
    {
        Vector3 targetEuler = target.eulerAngles;
        
        // Calcolo rotazione desiderata applicando offset e blocchi
        float finalRotX = lockRotX ? rotationOffset.x : targetEuler.x + rotationOffset.x;
        float finalRotY = lockRotY ? rotationOffset.y : targetEuler.y + rotationOffset.y;
        float finalRotZ = lockRotZ ? rotationOffset.z : targetEuler.z + rotationOffset.z;

        Quaternion desiredRotation = Quaternion.Euler(finalRotX, finalRotY, finalRotZ);

        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotSpeed);
    }

    // Riposizionamento istantaneo
    public void ForceUpdatePosition()
    {
        if(target == null) return;
        transform.position = target.TransformPoint(positionOffset);
        
        // Rispetta i lock anche nel force update
        if(lockPosY) 
        {
             Vector3 p = transform.position;
             p.y = target.position.y + positionOffset.y;
             transform.position = p;
        }
        
        HandleRotation();
        // Reset della velocità fisica simulata
        velocity = Vector3.zero;
        isFollowing = false;
    }

    //debug in Editor
    private void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            // Disegna la linea della deadzone
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, deadzoneDistance);
            
            Gizmos.color = isFollowing ? Color.green : Color.red;
            Gizmos.DrawLine(target.position, transform.position);
        }
    }
}