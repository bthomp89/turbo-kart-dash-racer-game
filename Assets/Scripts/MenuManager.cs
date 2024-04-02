using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button level1Button, level2Button, level3Button, level4Button, controlsButton;
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Image characterPreviewDisplay;
    [SerializeField] private Sprite[] characterPreviewSprites;
    [SerializeField] private Button leftArrowButton, rightArrowButton, selectButton;

    private int currentCharacterIndex = 0;
    private int unlockedCharacters;

    void Start()
    {
        level1Button.onClick.AddListener(() => SceneManager.LoadScene("Racer_Proto"));
        level2Button.onClick.AddListener(() => SceneManager.LoadScene("Cam_Level"));
        level3Button.onClick.AddListener(() => SceneManager.LoadScene("Alex_Level"));
        level4Button.onClick.AddListener(() => SceneManager.LoadScene("Tara_Level"));
        controlsButton.onClick.AddListener(() => ManageRoadSpeed.Instance.OpenControlsMenu());

        leftArrowButton.onClick.AddListener(OnLeftArrowPressed);
        rightArrowButton.onClick.AddListener(OnRightArrowPressed);
        selectButton.onClick.AddListener(SelectCharacter);

        UpdateUnlockedCharacters();
        LoadImageOfCurrentCharacter();
    }

    void UpdateUnlockedCharacters()
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        unlockedCharacters = Mathf.Min(coins / 10, 8); // Limit the number of unlocked characters to 6
        // Ensure currentCharacterIndex is within bounds of unlocked characters
        currentCharacterIndex = Mathf.Min(currentCharacterIndex, unlockedCharacters);
    }

    void LoadImageOfCurrentCharacter()
    {
       currentCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        // Clamp the selected index to the number of unlocked characters
        currentCharacterIndex = Mathf.Clamp(currentCharacterIndex, 0, unlockedCharacters);
        characterPreviewDisplay.sprite = characterPreviewSprites[currentCharacterIndex];
    }

    void DisplayCurrentCharacter()
    {
        characterPreviewDisplay.sprite = characterPreviewSprites[currentCharacterIndex];
    }

    public void SelectCharacter()
    {
        if (currentCharacterIndex <= unlockedCharacters)
        {
            PlayerPrefs.SetInt("SelectedCharacterIndex", currentCharacterIndex);
            PlayerPrefs.Save();
        }
    }

    void OnLeftArrowPressed()
    {
        if (currentCharacterIndex > 0)
        {
            currentCharacterIndex--;
            DisplayCurrentCharacter();
        }
    }

    void OnRightArrowPressed()
    {
        if (currentCharacterIndex < unlockedCharacters)
        {
            currentCharacterIndex++;
            DisplayCurrentCharacter();
        }
    }
}
