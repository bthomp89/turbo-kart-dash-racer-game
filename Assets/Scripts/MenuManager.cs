using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public Slider controlSlider;
    public Slider duckSlider;
    public Slider jumpSlider;
    public Slider lsSlider;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Image characterPreviewDisplay;
    [SerializeField] private Sprite[] characterPreviewSprites; // Assign the sprites in the Inspector
    [SerializeField] private Button leftArrowButton;
    [SerializeField] private Button rightArrowButton;
    [SerializeField] private Button selectButton;

    private int currentCharacterIndex = 0;




    void Start()
    {
        level1Button.onClick.AddListener(LoadLevel1Scene);
        level2Button.onClick.AddListener(LoadLevel2Scene);
        level3Button.onClick.AddListener(LoadLevel3Scene);
        level4Button.onClick.AddListener(LoadLevel4Scene);

        leftArrowButton.onClick.AddListener(OnLeftArrowPressed);
        rightArrowButton.onClick.AddListener(OnRightArrowPressed);
        selectButton.onClick.AddListener(SelectCharacter);

        LoadImageofCurrentCharacter();
        //DisplayCurrentCharacter();

    }


    private void LoadImageofCurrentCharacter()
    {
       int index = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);//default to 0
       characterPreviewDisplay.sprite = characterPreviewSprites[index];

    }

    private void DisplayCurrentCharacter()
    {
        if (characterPreviewSprites.Length > 0)
        {
            characterPreviewDisplay.sprite = characterPreviewSprites[currentCharacterIndex];
        }
    }

    // Called when the select button is pressed
    public void SelectCharacter()
    {
        // Save the selected character index for later use in game
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentCharacterIndex);
        // Load the game scene or close the menu as needed
        Debug.Log("Character Selected: " + characterPrefabs[currentCharacterIndex].name);
    }
    // Called when the left arrow button is pressed
    public void OnLeftArrowPressed()
    {
        currentCharacterIndex = (currentCharacterIndex > 0) ? currentCharacterIndex - 1 : characterPreviewSprites.Length - 1;
        DisplayCurrentCharacter();
    }

    // Called when the right arrow button is pressed
    public void OnRightArrowPressed()
    {
        currentCharacterIndex = (currentCharacterIndex < characterPreviewSprites.Length - 1) ? currentCharacterIndex + 1 : 0;
        DisplayCurrentCharacter();
    }

    //call this method when closing the menu
    public void UpdateSliders()
    {

        //get the slider value as an int
        int controlSliderValue = (int)controlSlider.value;
        int duckSliderValue = (int)duckSlider.value;
        int jumpSliderValue = (int)jumpSlider.value;
        int lsSliderValue = (int)lsSlider.value;

        //update the attributes based on sliders
        ManageRoadSpeed.Instance.updateAttributes(controlSliderValue, duckSliderValue, jumpSliderValue, lsSliderValue);
    }

    //update value of sliders
    public void UpdateMenu()
    {
        controlSlider.value = PlayerPrefs.GetInt("ControlSlider", 1);
        duckSlider.value = PlayerPrefs.GetInt("DuckSlider", 1);
        jumpSlider.value = PlayerPrefs.GetInt("JumpSlider", 1);
        lsSlider.value = PlayerPrefs.GetInt("LsSlider", 1);
    }

    public void LoadLevel1Scene()
    {
        SceneManager.LoadScene("Racer_Proto");
    }
    public void LoadLevel2Scene()
    {
        SceneManager.LoadScene("Cam_Level");
    }
    public void LoadLevel3Scene()
    {
        SceneManager.LoadScene("Alex_Level");
    }
    public void LoadLevel4Scene()
    {
        SceneManager.LoadScene("Tara_Level");
    }
}