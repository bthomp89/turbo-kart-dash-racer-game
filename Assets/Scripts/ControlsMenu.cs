using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{
    public Slider controlSlider, duckSlider, jumpSlider, lsSlider;
    public Button backButton;

    void Start()
    {
        backButton.onClick.AddListener(() => ManageRoadSpeed.Instance.CloseControlsMenu());
        UpdateMenu();
    }

    public void UpdateMenu()
    {
        controlSlider.value = PlayerPrefs.GetInt("ControlSlider", 1);
        duckSlider.value = PlayerPrefs.GetInt("DuckSlider", 1);
        jumpSlider.value = PlayerPrefs.GetInt("JumpSlider", 1);
        lsSlider.value = PlayerPrefs.GetInt("LsSlider", 1);
    }

    // Method to update player preferences based on sliders' values
    void OnDisable()
    {
        PlayerPrefs.SetInt("ControlSlider", (int)controlSlider.value);
        PlayerPrefs.SetInt("DuckSlider", (int)duckSlider.value);
        PlayerPrefs.SetInt("JumpSlider", (int)jumpSlider.value);
        PlayerPrefs.SetInt("LsSlider", (int)lsSlider.value);
        PlayerPrefs.Save();
    }
}
