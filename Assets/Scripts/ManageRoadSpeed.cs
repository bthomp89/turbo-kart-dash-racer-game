using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageRoadSpeed : MonoBehaviour
{
    public static ManageRoadSpeed Instance { get; private set; }

    public float CurrentSpeed { get; private set; }
    [SerializeField] private float initialSpeed = -2.5f;
    [SerializeField] private float acceleration = -0.7f; //Driver Upgrade --> slows speed down giving driver more control
    [SerializeField] private float maxGameSpeed = -10f;

    private bool isGameStarted = false;
    private float gameStartTime;
    private bool isMenuOpen = false;
    private bool isControlsMenuOpen = false;
    private string activeLevelSceneName;


    private Player_Movement motor;
    private GameObject startText;

    public Text scoreText, highScoreText, speedText, skillPointsText, coinsText;
    private float score, highScore;
    private int coins;
    private int skillPoints;

    private float lateralSpeed, jumpPower;
    private float duckTime;

    private int controlSlider, duckSlider, jumpSlider, lsSlider;

    [SerializeField] private GameObject[] characterPrefabs; // Add this to assign your character prefabs in the Inspector


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            startText = GameObject.FindGameObjectWithTag("StartText");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //PlayerPrefs.SetInt("Coins", 0);
        //PlayerPrefs.SetInt("SkillPoints", 0);

        LoadPlayerPreferences();
        SpawnSelectedCharacter();
        UpdateScores();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isGameStarted)
        {
            StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            if (!isGameStarted)
            {
                if (!isMenuOpen && !isControlsMenuOpen)
                {
                    activeLevelSceneName = SceneManager.GetActiveScene().name;
                    StartCoroutine(LoadMenuScene("Menu", true));
                }
                else if (isMenuOpen && !isControlsMenuOpen)
                {
                    StartCoroutine(LoadMenuScene("Menu", false));
                    SceneManager.LoadScene(activeLevelSceneName);

                }
            }
        }
        else if (isGameStarted)
        {
            //gradually increase speed over game life
            if (CurrentSpeed > maxGameSpeed)
            {
                CurrentSpeed += acceleration * Time.deltaTime;
            }
            else
            {
                CurrentSpeed = maxGameSpeed;
            }


            //score calculator logic
            score = (Time.time - gameStartTime) * 10;
            coins = PlayerPrefs.GetInt("Coins", 0);

            //update highscore logic
            if (score > highScore)
            {
                highScore = score;

                //save highscore to PlayerPrefs
                PlayerPrefs.SetFloat("HighScore", highScore);
                //check if highscore is eligble for SP's
                isHighScoreEligble(highScore);
            }
            UpdateScores();
        }
    }

    public void updateCoins()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        coins += 10;
        PlayerPrefs.SetInt("Coins", coins);
    }

    public void StartGame()
    {
        isGameStarted = true;
        activeLevelSceneName = SceneManager.GetActiveScene().name; // Store the current scene

        if (startText != null) startText.SetActive(false);
        motor.startDriving();
        CurrentSpeed = initialSpeed;
        gameStartTime = Time.time;
    }

    public void OpenControlsMenu()
    {
        if (isMenuOpen && !isControlsMenuOpen)
        {
            StartCoroutine(LoadMenuScene("Controls", true));
        }
    }

    public void CloseControlsMenu()
    {
        if (isControlsMenuOpen)
        {
            StartCoroutine(LoadMenuScene("Controls", false));
        }
    }

    public void UpdateScores()
    {
        //update on screen texts
        scoreText.text = "Score: " + score.ToString("F2");
        highScoreText.text = "High Score: " + highScore.ToString("F2");
        speedText.text = "Speed: " + (-10 * CurrentSpeed).ToString("F2") + "mph";
        skillPointsText.text = "XP: " + skillPoints.ToString(); //change onscreen text
        coinsText.text = "Coins: " + coins.ToString(); //change onscreen text
    }

    private void LoadPlayerPreferences()
    {
        //load PlayerPrefs at the start of the game
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        skillPoints = PlayerPrefs.GetInt("SkillPoints", 0);
        controlSlider = PlayerPrefs.GetInt("ControlSlider", 1);
        duckSlider = PlayerPrefs.GetInt("DuckSlider", 1);
        jumpSlider = PlayerPrefs.GetInt("JumpSlider", 1);
        lsSlider = PlayerPrefs.GetInt("LsSlider", 1);
        coins = PlayerPrefs.GetInt("Coins", 0);


        updateAttributes(controlSlider, duckSlider, jumpSlider, lsSlider);
    }

    private void SpawnSelectedCharacter()
    {
        // Destroy the old character if there is one
        GameObject oldPlayer = GameObject.FindGameObjectWithTag("Player");
        if (oldPlayer != null) Destroy(oldPlayer);

        // Get the selected character index from PlayerPrefs
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0); // Default to 0

        // Define the start position
        Vector3 startPosition = new Vector3(0f, 0f, -10f); // The desired spawn position

        // Instantiate the selected character prefab
        GameObject playerPrefab = characterPrefabs[selectedCharacterIndex];
        GameObject playerInstance = Instantiate(playerPrefab, startPosition, Quaternion.identity);

        // Set the 'motor' reference to the new instance
        motor = playerInstance.GetComponent<Player_Movement>();

    }

    //game reset logic
    public void ResetGame()
    {
        isGameStarted = false;
        activeLevelSceneName = SceneManager.GetActiveScene().name; // Store the current scene

        if (startText != null)
            startText.SetActive(true);
    }

    public void updateAttributes(int controlSlider, int duckSlider, int jumpSlider, int lsSlider)
    {

        float[] accelerationMapping = { -0.7f, -0.5f, -0.4f, -0.3f, -0.1f };
        float[] duckTimeMapping = { 0.5f, 1f, 1.5f, 2f, 3f };
        float[] jumpPowerMapping = { 4f, 5f, 6f, 7.5f, 8.5f };
        float[] lateralSpeedMapping = { 4f, 5.5f, 7f, 8.5f, 10f };

        //update game settings based on slider values
        acceleration = accelerationMapping[controlSlider - 1]; //subtract 1 because array indexes start at 0
        duckTime = duckTimeMapping[duckSlider - 1];
        jumpPower = jumpPowerMapping[jumpSlider - 1];
        lateralSpeed = lateralSpeedMapping[lsSlider - 1];

        PlayerPrefs.SetInt("ControlSlider", controlSlider);
        PlayerPrefs.SetInt("DuckSlider", duckSlider);
        PlayerPrefs.SetInt("JumpSlider", jumpSlider);
        PlayerPrefs.SetInt("LsSlider", lsSlider);

        PlayerPrefs.SetFloat("Control", acceleration);
        PlayerPrefs.SetFloat("Duck", duckTime);
        PlayerPrefs.SetFloat("Jump", jumpPower);
        PlayerPrefs.SetFloat("Ls", lateralSpeed);
        PlayerPrefs.Save();
    }


    IEnumerator LoadMenuScene(string sceneName, bool loadScene)
    {
        if (loadScene)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!asyncLoad.isDone) yield return null;
        }
        else
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
            while (!asyncUnload.isDone) yield return null;
        }

        isMenuOpen = sceneName == "Menu" ? loadScene : isMenuOpen;
        isControlsMenuOpen = sceneName == "Controls" ? loadScene : isControlsMenuOpen;
    }


    void isHighScoreEligble(float highscore)
    {

        //calculate the number of times 150 fits into the current high score
        int targetSkillPointsAwarded = (int)(highScore / 150);

        //calculate how many new skill points need to be awarded
        int newSkillPoints = targetSkillPointsAwarded - skillPoints;

        if (newSkillPoints > 0)
        {

            skillPoints += newSkillPoints;

            //save the updated values back to PlayerPrefs
            PlayerPrefs.SetInt("SkillPoints", skillPoints);
            PlayerPrefs.Save();

        }
    }
}
