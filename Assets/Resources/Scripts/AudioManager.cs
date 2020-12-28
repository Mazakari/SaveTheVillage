using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // Sound files references
    [SerializeField] private AudioClip buttonClick = null;
    public AudioClip ButtonClick { get { return buttonClick; } }

    [SerializeField] private AudioClip actionDenied = null;
    public AudioClip ActionDenied { get { return actionDenied; } }

    [SerializeField] private AudioClip raidReady = null;
    public AudioClip RaidReady { get { return raidReady; } }

    [SerializeField] private AudioClip gameFailed = null;
    public AudioClip GameFailed { get { return gameFailed; } }

    [SerializeField] private AudioClip gameWon = null;
    public AudioClip GameWon { get { return gameWon; } }

    [SerializeField] private AudioClip eatCycleComplete = null;
    public AudioClip EatCycleComplete { get { return eatCycleComplete; } }

    [SerializeField] private AudioClip huntCycleComplete = null;
    public AudioClip HuntCycleComplete { get { return huntCycleComplete; } }

    [SerializeField] private AudioClip goblinHunterReady = null;
    public AudioClip GoblinHunterReady { get { return goblinHunterReady; } }

    [SerializeField] private AudioClip trollWarriorReady = null;
    public AudioClip TrollWarriorReady { get { return trollWarriorReady; } }

    private AudioSource audioSource = null;// AudioSource component reference

    private bool isSoundsOn = true;// If sounds are on
    [SerializeField] private AudioListener audioListener = null;// AudioListener reference
    [SerializeField] private Image mutedAudioImage = null;// Muted image on sound control button reference
    [SerializeField] private Image unmutedAudioImage = null;// Unmuted image on sound control button reference

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        unmutedAudioImage.gameObject.SetActive(true);
        mutedAudioImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Toggle sounds on and off
    /// </summary>
    public void ToggleGameMusic()
    {
        if (isSoundsOn)
        {
            audioSource.clip = ButtonClick;
            audioSource.Play();

            unmutedAudioImage.gameObject.SetActive(false);
            mutedAudioImage.gameObject.SetActive(true);
            audioListener.enabled = false;
        }
        else
        {
            audioSource.clip = ButtonClick;
            audioSource.Play();

            unmutedAudioImage.gameObject.SetActive(true);
            mutedAudioImage.gameObject.SetActive(false);
            audioListener.enabled = true;
        }

        isSoundsOn = !isSoundsOn;
    }
}
