using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCheck : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] bool debugFirstLaunch;

    void Update()
    {
        if (!IsAnimationPlaying())
        {
            LoadNextScene();
        }
    }

    bool IsAnimationPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
    }

    void LoadNextScene()
    {
        bool isFirstLaunch = PlayerPrefs.GetInt("IsFirstLaunch", 1) == 1;

        if (isFirstLaunch || debugFirstLaunch)
        {
            PlayerPrefs.SetInt("IsFirstLaunch", 0);
            PlayerPrefs.Save(); 

            SceneManager.LoadScene("Tutorial");
        }
        else 
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
}