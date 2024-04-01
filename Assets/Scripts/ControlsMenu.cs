using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour
{
     public Slider controlSlider;
    public Slider duckSlider;
    public Slider jumpSlider;
    public Slider lsSlider;
        public Button backButton;
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(LoadMainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
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

     public void UpdateMenu()
    {
        controlSlider.value = PlayerPrefs.GetInt("ControlSlider", 1);
        duckSlider.value = PlayerPrefs.GetInt("DuckSlider", 1);
        jumpSlider.value = PlayerPrefs.GetInt("JumpSlider", 1);
        lsSlider.value = PlayerPrefs.GetInt("LsSlider", 1);
    }

    public void LoadMainMenu(){
        SceneManager.LoadScene("Menu");
    }
}
