using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour
{
    [SerializeField] private Image playImage = null;// Play image on pause button reference
    [SerializeField] private Text pauseText = null;// Pause text on pause button reference

    private AudioSource audioSource = null;// Audio source reference
    private AudioManager audioManager = null;// AudioManager reference

    private float gameSpeedChangeValue = 0.5f;// Value game speed changes
    private float currentGameSpeed = 1.0f;// Current game speed
    private float minGameSpeedLimit = 0.2f;// Minimum game speed limit
    private float maxGameSpeedLimit = 1.0f;// Maximum game speed limit
    private bool isGamePaused = false;// If game is paused

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioSource = GetComponent<AudioSource>();

        playImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Pause / Unpause the game
    /// </summary>
    public void PauseGame()
    {
        audioSource.clip = audioManager.ButtonClick;
        audioSource.Play();

        if (isGamePaused)
        {
            Time.timeScale = currentGameSpeed;
            pauseText.gameObject.SetActive(true);
            playImage.gameObject.SetActive(false);
            audioSource.Play();
        }
        else
        {
            Time.timeScale = 0;
            playImage.gameObject.SetActive(true);
            pauseText.gameObject.SetActive(false);
            audioSource.Pause();
        }

        isGamePaused = !isGamePaused;
    }

    /// <summary>
    /// Increase game speed by 0.5
    /// </summary>
    public void IncreaseGameSpeed()
    {
        if (isGamePaused == false)
        {
            audioSource.clip = audioManager.ButtonClick;
            audioSource.Play();

            if (currentGameSpeed + gameSpeedChangeValue <= maxGameSpeedLimit)
            {
                currentGameSpeed += gameSpeedChangeValue;
                Time.timeScale = currentGameSpeed;
            }
        }

        else if (isGamePaused)
        {
            audioSource.clip = audioManager.ActionDenied;
            audioSource.Play();
        }
    }

    /// <summary>
    /// Decrease game speed by 0.5
    /// </summary>
    public void DecreaseGameSpeed()
    {
        if (isGamePaused == false)
        {
            audioSource.clip = audioManager.ButtonClick;
            audioSource.Play();

            if (currentGameSpeed - gameSpeedChangeValue >= minGameSpeedLimit)
            {
                currentGameSpeed -= gameSpeedChangeValue;
                Time.timeScale = currentGameSpeed;
            }

        }
        else if(isGamePaused)
        {
            audioSource.clip = audioManager.ActionDenied;
            audioSource.Play();
        }
    }

    /// <summary>
    /// Restart current level
    /// </summary>
    public void RestartGame()
    {
        audioSource.clip = audioManager.ButtonClick;
        audioSource.Play();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
