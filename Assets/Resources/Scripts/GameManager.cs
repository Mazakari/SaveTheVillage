using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text goblinHuntersCounterText = null;// Current goblin hunters amount text reference
    [SerializeField] private Text trollWarriorsCounterText = null;// Current troll warriors amount text reference
    [SerializeField] private Text totalMeatCounterText = null;// Current total meat amount text reference
    [SerializeField] private Text raidersCounterText = null;// Raid counter text reference
    [SerializeField] private Text meatIncomePerCycleText = null;// Current meat income per production cycle text reference
    [SerializeField] private Text meatOutcomePerCycleText = null;// Current meat outcome per production cycle text reference
    [SerializeField] private Text losePopupText = null;// Lose popup text reference
    [SerializeField] private Text winPopupText = null;// Win popup text reference
    [SerializeField] private Text levelGoalsText = null;// Level goals text reference



    [SerializeField] private Image goblinHunterImage = null;// Goblin hunter image reference
    [SerializeField] private Image trollWarriorImage = null;// Troll warrior image reference
    [SerializeField] private Image raidImage = null;// Raid image reference

    [SerializeField] private MeatProductionTimer meatIncomeTimer = null;// Meat income timer reference
    [SerializeField] private MeatProductionTimer meatOutcomeTimer = null;// Meat outcome timer reference

    [SerializeField] private int goblinHuntersAmount = 0;// Current goblin hunters amount
    [SerializeField] private int trollWarriorsAmount = 0;// Current troll warriors amount

    [SerializeField] private int raidIncreaseAmount = 1;// How much units would be added in the nex raid

    [SerializeField] private int meatIncomePerGoblinHumter = 1;// Meat income per goblin hunter
    [SerializeField] private int meatOutcomePerTrollWarrior = 2;// Meat outcome per troll warrior

    [SerializeField] private Button goblinHunterTrainingButton = null;// Goblin hunter training button
    [SerializeField] private Button trollWarriorTrainingButton = null;// Troll warrior training button

    [SerializeField] private int goblinHunterTrainingCost = 1;// Goblin hunter training cost
    [SerializeField] private int trollWarriorTrainingCost = 2;// Troll warrior training cost

    [SerializeField] private float goblinHunterTrainigTime = 2.0f;// Total goblin hunter training time
    [SerializeField] private float trollWarriorTrainigTime = 4.0f;// Total troll warrior training time
    [SerializeField] private float raidCooldownTime = 7.0f;// Time untill next raid
    

    [SerializeField] private GameObject winPopup = null;// Win popup reference
    [SerializeField] private GameObject losePopup = null;// Lose popup reference
    [SerializeField] private GameObject notEnoughMeatPopup = null;// NoEnoughMeatPopup reference
    [SerializeField] private GameObject safeTurnsLeft = null;// Safe turns text and background reference
    [SerializeField] private GameObject mainMenuPopup = null;// Main Menu popup reference


    [SerializeField] private int meatWinCondition = 100;// Level win condition

    // AudioSource references
    [SerializeField] private AudioSource goblinHunterAudioSource = null;// Goblin Hunter Audiosource reference
    [SerializeField] private AudioSource trollWarriorAudioSource = null;// Troll Warrior AudioSource reference
    [SerializeField] private AudioSource meatIncomeAudioSource = null;// Meat income AudioSource reference
    [SerializeField] private AudioSource meatOutcomeAudioSource = null;// Meat outcome AudioSource reference
    [SerializeField] private AudioSource villageRaidAudioSource = null;// Village raid AudioSource reference
    [SerializeField] private AudioSource gameWonAudioSource = null;// GameWonPopup AudioSource reference
    [SerializeField] private AudioSource gameFailedAudioSource = null;// GameFailedPopup AudioSource reference

    private int trollWariorsLoseCondition = 0;// Level lose condition
    private int meatAmount = 0;// Current meat amount
    private int raidersAmount = 0;// Amount of raid units in the next human raid
    private bool isGameOver = false;// If fail or win conditions are met
    private bool isPopupShowing = false;// Is popup showing at the moment
    private bool isRaidActive = false;// Is raid already activated
    private bool isMainMenuShowing = false;// Is main menu now is showing
    private int raidActivationCycles = 3;// Cycles untill raid will be activated
    private AudioManager audioManager = null;// Audio Manager reference
    private AudioSource audioSource = null;// GameManager AudioSource reference

    private float popupShowTime = 2.0f;// Time to show popup
    private float currentGoblinHunterTrainingTime;// Current goblin hunter training time
    private float currentTrollWarriorTrainingTime;// Current troll warrior training time
    private float currentRaidCooldownTime;// Current rain cooldown timer
    private float currentPopupShowTime;// Current show time of the popup

    private int meatIncomePerCycle = 0;// Total meat income per meat production cycle
    private int meatOutcomePerCycle = 0;// Total meat outcome per meat production cycle

    private int raidsSurvived = 0;// Amount of raids player survived
    private int totaTrolllWarriorsTrained = 0;// Total troll wariors were trained throu the level
    private int totalMeatCoollected = 0;// Total meat collected through the level

    // Start is called before the first frame update
    void Start()
    {
        GameInitializer();
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        MeatProcessor();
        UpdateText();
        TrainingTimerHandler();
        RaidTimerHandler();
        GameConditionsHandler();
        PopupShowTimerHandler();
        MainMenuHandler();

    }

    /// <summary>
    /// Initializes game on start
    /// </summary>
    private void GameInitializer()
    {
        Time.timeScale = 1;
        audioSource = GetComponent<AudioSource>();
        audioManager = FindObjectOfType<AudioManager>();
        isGameOver = false;

        winPopup.SetActive(false);
        losePopup.SetActive(false);
        notEnoughMeatPopup.SetActive(false);
        mainMenuPopup.SetActive(false);

        meatIncomePerCycle = goblinHuntersAmount * meatIncomePerGoblinHumter;
        meatOutcomePerCycle = trollWarriorsAmount * meatOutcomePerTrollWarrior;

        currentGoblinHunterTrainingTime = goblinHunterTrainigTime;
        currentTrollWarriorTrainingTime = trollWarriorTrainigTime;
        currentRaidCooldownTime = 0;
        currentPopupShowTime = 0;
    }

    /// <summary>
    /// Updates game counters text
    /// </summary>
    private void UpdateText()
    {
        goblinHuntersCounterText.text = goblinHuntersAmount.ToString();
        trollWarriorsCounterText.text = trollWarriorsAmount.ToString();
        totalMeatCounterText.text = meatAmount.ToString();
        meatIncomePerCycleText.text = $"+ {meatIncomePerCycle}";
        meatOutcomePerCycleText.text = $"- {meatOutcomePerCycle}";
        if (isRaidActive == false)
        {
            raidersCounterText.color = new Color(0.05f, 1, 0);
            raidersCounterText.text = raidActivationCycles.ToString();
        }
        else
        {
            raidersCounterText.color = Color.black;
            raidersCounterText.text = raidersAmount.ToString();
        }
        levelGoalsText.text = $"{meatAmount} / {meatWinCondition}";
    }

    /// <summary>
    /// Manage increase and decrease meat from the total meat amount
    /// </summary>
    private void MeatProcessor()
    {
        if (meatIncomeTimer.IsMeatProductionComplete)
        {
            meatIncomeAudioSource.pitch = Random.Range(0.7f, 1.1f);
            meatIncomeAudioSource.Play();

            meatAmount += goblinHuntersAmount * meatIncomePerGoblinHumter;
            totalMeatCoollected += meatAmount;
        }

        if (meatOutcomeTimer.IsMeatProductionComplete)
        {
            meatOutcomeAudioSource.pitch = Random.Range(0.7f, 1.1f);
            meatOutcomeAudioSource.Play();

            if (meatAmount - trollWarriorsAmount * meatOutcomePerTrollWarrior >= 0)
            {
                meatAmount -= trollWarriorsAmount * meatOutcomePerTrollWarrior;
            }
            else if (meatAmount - trollWarriorsAmount * meatOutcomePerTrollWarrior < 0)
            {
                meatAmount = 0;
            }
            
        }
    }

    private void MainMenuHandler()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameOver == false && isMainMenuShowing == false)
        {
            isMainMenuShowing = true;
            Time.timeScale = 0;
            mainMenuPopup.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isGameOver == false && isMainMenuShowing == true)
        {
            isMainMenuShowing = false;
            Time.timeScale = 1;
            mainMenuPopup.SetActive(false);
        }
    }

    /// <summary>
    /// Manages training time for units and their amount 
    /// </summary>
    private void TrainingTimerHandler()
    {
        // Goblin hunter timer
        if (currentGoblinHunterTrainingTime < goblinHunterTrainigTime)
        {
            currentGoblinHunterTrainingTime += Time.deltaTime;
            goblinHunterImage.fillAmount = currentGoblinHunterTrainingTime / goblinHunterTrainigTime;
        }
        else if (currentGoblinHunterTrainingTime > goblinHunterTrainigTime)
        {
            goblinHunterAudioSource.pitch = Random.Range(0.7f, 1.1f);
            goblinHunterAudioSource.Play();

            goblinHuntersAmount++;
            goblinHunterTrainingButton.interactable = true;
            currentGoblinHunterTrainingTime = goblinHunterTrainigTime;
            meatIncomePerCycle = goblinHuntersAmount * meatIncomePerGoblinHumter;
        }
       

        // Troll warrior timer
        if (currentTrollWarriorTrainingTime < trollWarriorTrainigTime)
        {
            currentTrollWarriorTrainingTime += Time.deltaTime;
            trollWarriorImage.fillAmount = currentTrollWarriorTrainingTime / trollWarriorTrainigTime;
        }
        else if (currentTrollWarriorTrainingTime > trollWarriorTrainigTime)
        {
            trollWarriorAudioSource.pitch = Random.Range(0.7f, 1.1f);
            trollWarriorAudioSource.Play();

            trollWarriorsAmount++;
            totaTrolllWarriorsTrained++;
            trollWarriorTrainingButton.interactable = true;
            currentTrollWarriorTrainingTime = trollWarriorTrainigTime;
            meatOutcomePerCycle = trollWarriorsAmount * meatOutcomePerTrollWarrior;
        }
    }

    /// <summary>
    /// Manages raids timer and raid mount
    /// </summary>
    private void RaidTimerHandler()
    {
        if (currentRaidCooldownTime < raidCooldownTime && isRaidActive == false)
        {
            safeTurnsLeft.SetActive(true);
            currentRaidCooldownTime += Time.deltaTime;
            raidImage.fillAmount = currentRaidCooldownTime / raidCooldownTime;
        }
        else if (currentRaidCooldownTime > raidCooldownTime && isRaidActive == false)
        {
            if (raidActivationCycles > 0)
            {
                raidActivationCycles--;
                currentRaidCooldownTime = 0;
            }
            else
            {
                safeTurnsLeft.SetActive(false);
                isRaidActive = true;
            }
        }

        if (currentRaidCooldownTime < raidCooldownTime && isRaidActive == true)
        {
            currentRaidCooldownTime += Time.deltaTime;
            raidImage.fillAmount = currentRaidCooldownTime / raidCooldownTime;
        }
        else if (currentRaidCooldownTime > raidCooldownTime && isRaidActive == true)
        {
            meatOutcomeAudioSource.pitch = Random.Range(0.7f, 1.1f);
            villageRaidAudioSource.Play();

            if (trollWarriorsAmount - raidersAmount >= 0)
            {
                raidsSurvived++;
            }
            currentRaidCooldownTime = 0;
            trollWarriorsAmount -= raidersAmount;
            raidersAmount += raidIncreaseAmount;
            raidCooldownTime += 0.9f;
        }
    }

    /// <summary>
    /// Shows popup for the preset amount of time
    /// </summary>
    private void PopupShowTimerHandler()
    {
        if (currentPopupShowTime < popupShowTime && isPopupShowing == true)
        {
            notEnoughMeatPopup.SetActive(true);
            currentPopupShowTime += Time.deltaTime;
        }
        else if (currentPopupShowTime > popupShowTime)
        {
            notEnoughMeatPopup.SetActive(false);
            currentPopupShowTime = popupShowTime;
            isPopupShowing = false;
        }
    }

    /// <summary>
    /// Controls game win and lose conditions
    /// </summary>
    private void GameConditionsHandler()
    {
        if (trollWarriorsAmount < trollWariorsLoseCondition && isGameOver == false)
        {
            isGameOver = true;
            Time.timeScale = 0;
            losePopupText.text = $"Pesky 'umies ransacked our village! You are bad Boss!\n\n- Raids survived: {raidsSurvived}\n- Meat collected: {totalMeatCoollected}\n- Goblin Hunters trained: {goblinHuntersAmount}\n- Troll Warriors trained: {totaTrolllWarriorsTrained}";
            losePopup.SetActive(true);

            gameFailedAudioSource.Play();
        }
        else if (meatAmount >= meatWinCondition && isGameOver == false)
        {
            isGameOver = true;
            Time.timeScale = 0;
            winPopupText.text = $"Well done, boss!\n\nYou've collected {meatWinCondition} meat to squash nearby pesky 'umies castle!";
            winPopup.SetActive(true);

            gameWonAudioSource.Play();
        }
    }

    /// <summary>
    /// Starts goblin hunter training
    /// </summary>
    
    public void TrainGoblinHunter()
    {
        if (meatAmount - goblinHunterTrainingCost > 0)
        {
            audioSource.clip = audioManager.ButtonClick;
            audioSource.Play();

            meatAmount -= goblinHunterTrainingCost;
            currentGoblinHunterTrainingTime = 0;
            goblinHunterTrainingButton.interactable = false;
        }
        else
        {
            audioSource.clip = audioManager.ActionDenied;
            audioSource.Play();

            currentPopupShowTime = 0;
            isPopupShowing = true;
        }
    }

    /// <summary>
    /// Starts troll warrior training
    /// </summary>
    public void TrainTrollWarrior()
    {
        if (meatAmount - trollWarriorTrainingCost > 0)
        {
            audioSource.clip = audioManager.ButtonClick;
            audioSource.Play();

            meatAmount -= trollWarriorTrainingCost;
            currentTrollWarriorTrainingTime = 0;
            trollWarriorTrainingButton.interactable = false;
        }
        else
        {
            audioSource.clip = audioManager.ActionDenied;
            audioSource.Play();

            currentPopupShowTime = 0;
            isPopupShowing = true;
        }
    }

    public void CountinueGame()
    {
        isMainMenuShowing = false;
        Time.timeScale = 1;
        mainMenuPopup.SetActive(false);
    }
    
}
