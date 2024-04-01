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

        LoadImageOfCurrentCharacter();
    }

    void LoadImageOfCurrentCharacter()
    {
        currentCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        characterPreviewDisplay.sprite = characterPreviewSprites[currentCharacterIndex];
    }

    void DisplayCurrentCharacter()
    {
        characterPreviewDisplay.sprite = characterPreviewSprites[currentCharacterIndex];
    }

    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentCharacterIndex);
        PlayerPrefs.Save();
    }

    void OnLeftArrowPressed()
    {
        currentCharacterIndex = currentCharacterIndex > 0 ? currentCharacterIndex - 1 : characterPreviewSprites.Length - 1;
        DisplayCurrentCharacter();
    }

    void OnRightArrowPressed()
    {
        currentCharacterIndex = currentCharacterIndex < characterPreviewSprites.Length - 1 ? currentCharacterIndex + 1 : 0;
        DisplayCurrentCharacter();
    }
}
