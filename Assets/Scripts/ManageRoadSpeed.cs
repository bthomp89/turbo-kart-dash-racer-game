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
    [SerializeField] private float maxGameSpeed = -20f; 

    private bool isGameStarted = false;
    private float gameStartTime;
    private bool isMenuOpen = false;

    private Player_Movement motor;
    private GameObject startText;

    public Text scoreText, highScoreText, speedText, skillPointsText;
    private float score, highScore;

    private int skillPoints;

    private float lateralSpeed, jumpPower;
    private float duckTime;

    private int controlSlider, duckSlider, jumpSlider, lsSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            motor = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>();
            startText = GameObject.FindGameObjectWithTag("StartText");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //load PlayerPrefs at the start of the game
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        skillPoints = PlayerPrefs.GetInt("SkillPoints", 0);
        controlSlider = PlayerPrefs.GetInt("ControlSlider", 1);
        duckSlider = PlayerPrefs.GetInt("DuckSlider", 1);
        jumpSlider = PlayerPrefs.GetInt("JumpSlider", 1);
        lsSlider = PlayerPrefs.GetInt("LsSlider", 1);

        updateAttributes(controlSlider, duckSlider, jumpSlider, lsSlider);
        UpdateScores();
    }

    private void Update()
    {
        //starting game logic
        if (Input.GetKeyDown(KeyCode.Space) && !isGameStarted)
        {
            isGameStarted = true;
            if (startText != null)
                startText.SetActive(false); //hide startText
            motor.startDriving();
            CurrentSpeed = initialSpeed;
            gameStartTime = Time.time; //record game start time
            

        } //logic for open/closing menu
        else if (Input.GetKeyDown(KeyCode.M) &&!isGameStarted && !isMenuOpen)
        {

            StartCoroutine(LoadMenuScene()); //opening menu logic
        }
        else if (Input.GetKeyDown(KeyCode.M) && !isGameStarted && isMenuOpen)
        {
            MenuManager menuManager = FindObjectOfType<MenuManager>();
            if (menuManager != null)
            {
                menuManager.UpdateSliders(); //updates values from menu sliders
            }

            SceneManager.UnloadSceneAsync("Menu");
            isMenuOpen = false;
            startText.SetActive(true);
            motor.LoadAndUpdateAttributes();

        }
        else if (isGameStarted)
        {
            //gradually increase speed over game life
            if(CurrentSpeed > maxGameSpeed)
            {
                CurrentSpeed += acceleration * Time.deltaTime;
            } else
            {
                CurrentSpeed = maxGameSpeed;
            }
           

            //score calculator logic
            score = (Time.time - gameStartTime) * 10;

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

    public void UpdateScores()
    {
        //update on screen texts
        scoreText.text = "Score: " + score.ToString("F2");
        highScoreText.text = "High Score: " + highScore.ToString("F2");
        speedText.text = "Speed: " + (-10 * CurrentSpeed).ToString("F2") + "mph";
        skillPointsText.text = "Skill Points: " + skillPoints.ToString(); //change onscreen text

    }

    //game reset logic
    public void ResetGame()
    {
        isGameStarted = false;
        if (startText != null)
            startText.SetActive(true);
    }

    public void updateAttributes(int controlSlider, int duckSlider, int jumpSlider, int lsSlider) 
    {

        float[] accelerationMapping = { -0.7f, -0.5f, -0.4f, -0.3f, -0.1f };
        float[] duckTimeMapping = { 0.5f, 1f, 1.5f, 2f, 3f };
        float[] jumpPowerMapping = { 4f, 4.5f, 5.25f, 6f, 6.5f };
        float[] lateralSpeedMapping = { 3f, 3.5f, 4f, 4.5f, 5.5f };

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

    IEnumerator LoadMenuScene()
    {
        //load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);

        //wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //now that the scene is loaded, find the MenuManager
        MenuManager menuManager = FindObjectOfType<MenuManager>();
        if (menuManager != null)
        {
            menuManager.UpdateMenu(); //update value of sliders in menu
        }

        isMenuOpen = true;
        startText.SetActive(false);
    }

    void isHighScoreEligble(float highscore){

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
