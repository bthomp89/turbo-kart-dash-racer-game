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




    void Start()
    {
        level1Button.onClick.AddListener(LoadLevel1Scene);
        level2Button.onClick.AddListener(LoadLevel2Scene);
        level3Button.onClick.AddListener(LoadLevel3Scene);
        level4Button.onClick.AddListener(LoadLevel4Scene);
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