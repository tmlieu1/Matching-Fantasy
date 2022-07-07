using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;

    // Trigger to fade out the scene when start button pressed
    public void FadeToGame() {
        animator.SetTrigger("FadeOut");
    }

    // Switches to the next scene in the build index (the game)
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        FindObjectOfType<AudioManager>().Stop("menu");
    }

    public void ReturnToMenu(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        FindObjectOfType<AudioManager>().Play("menu");
    }
}
