using UnityEngine;
using UnityEngine.UI;

public class MeatProductionTimer : MonoBehaviour
{
    [SerializeField] private float maxHuntingTime = 5.0f;// Maximum time for meat income production cycle

    private bool isMeatProductionComplete = false;// Is production cycle completed
    public bool IsMeatProductionComplete { get { return isMeatProductionComplete; } }

    private float currentHuntingTime;// Current hunting timer
    private Image image = null;// Meat image reference

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        currentHuntingTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        StartHunting();
    }
    /// <summary>
    /// Adds meat amount after the hunting timer ends
    /// </summary>
    public void StartHunting()
    {
        isMeatProductionComplete = false;
        currentHuntingTime += Time.deltaTime;
        SpriteFiller();

        if (currentHuntingTime >= maxHuntingTime)
        {
            currentHuntingTime = 0f;
            isMeatProductionComplete = true;
        }
    }

    /// <summary>
    /// Fills meat sprite along with timer
    /// </summary>
    private void SpriteFiller()
    {
        image.fillAmount = currentHuntingTime / maxHuntingTime;
    }
}
