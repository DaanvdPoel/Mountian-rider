using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour //Daan
{
    [Header("Score Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI distanceText;

    [Header("Menu Objects")]
    [SerializeField] private GameObject pauzeScreen;
    [SerializeField] private GameObject mainMenuScreen;

    [Header("Settings Screen")]
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private Slider volumeSlider;

    [Header("Death Screen Components")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TextMeshProUGUI scoreDeathText;
    [SerializeField] private TextMeshProUGUI highDeathText;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Volume") == true)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
            VolumeSlider(volumeSlider);
        }
        else
        {
            VolumeSlider(volumeSlider);
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
            AudioManager.instance.PlayBackGroundMusic(1);
        else if (SceneManager.GetActiveScene().name == "Game")
            AudioManager.instance.PlayBackGroundMusic(2);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauzeScreen();
    }

    public void VolumeSlider(Slider slider)
    {
        AudioManager.instance.SetVolumeLevel(slider.value);
        PlayerPrefs.SetFloat("Volume", slider.value);
    }

    /// <summary>
    /// updates the score UI and the high score UI.
    /// </summary>
    public void UpdateScoreUI()
    {
        if (scoreText != null || highScoreText != null)
        {
            scoreText.text = string.Format("Score: {0}", GameManager.instance.score);
            highScoreText.text = string.Format("HighScore: {0}", GameManager.instance.highScore);
            distanceText.text = string.Format("Distance: {0}m", GameManager.instance.distance);
        }
    }

    /// <summary>
    /// Closes the game with a small delay to play a click sound effect
    /// </summary>
    private IEnumerator ExitGame()
    {
        AudioManager.instance.PlaySoundEffect(0);
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    /// <summary>
    /// this will reset the highscore and also reset the saved highscore in player prefs
    /// </summary>
    public void ResetSavedData()
    {
        AudioManager.instance.PlaySoundEffect(0);
        PlayerPrefs.DeleteAll();
        volumeSlider.value = 0.2f;
        AudioManager.instance.SetVolumeLevel(volumeSlider.value);
        GameManager.instance.score = 0;
        GameManager.instance.highScore = 0;
        UpdateScoreUI();
    }

    /// <summary>
    /// sets the DeathScreen gameobject on 
    /// </summary>
    public void DeathScreen()
    {
        if(deathScreen != null)
        {
            deathScreen.SetActive(true);
            scoreDeathText.text = string.Format("Your Score: {0}", GameManager.instance.score);
            highDeathText.text = string.Format("Your HighScore: {0}", GameManager.instance.highScore);
        }
    }

    /// <summary>
    /// if the PauzeScreen is active it will be turned of and if PauzeScreen is inactive it wil be turned on;
    /// </summary>
    public void PauzeScreen()
    {
        if (pauzeScreen != null)
        {
            if (pauzeScreen.active == true)
            {
                pauzeScreen.SetActive(false);
                Time.timeScale = 1;
                AudioManager.instance.PlaySoundEffect(0);
            }
            else if (pauzeScreen.active == false)
            {
                pauzeScreen.SetActive(true);
                settingsScreen.SetActive(false);
                Time.timeScale = 0;
                AudioManager.instance.PlaySoundEffect(0);
            }
        }
    }

    /// <summary>
    /// if the SettingsScreen is active it will be turned of and if SettingsScreen is inactive it wil be turned on. als turns the mainmenu/pauzescreen off and on
    /// </summary>
    public void SettingsScreen()
    {
        if (mainMenuScreen != null)
        {
            if (settingsScreen.active == true)
            {
                settingsScreen.SetActive(false);
                mainMenuScreen.SetActive(true);
                AudioManager.instance.PlaySoundEffect(0);
            }
            else if (settingsScreen.active == false)
            {
                settingsScreen.SetActive(true);
                mainMenuScreen.SetActive(false);
                AudioManager.instance.PlaySoundEffect(0);
            }
        }else if(pauzeScreen != null)
        {
            if (settingsScreen.active == true)
            {
                settingsScreen.SetActive(false);
                pauzeScreen.SetActive(true);
                AudioManager.instance.PlaySoundEffect(0);
            }
            else if(settingsScreen.active == false)
            {
                settingsScreen.SetActive(true);
                pauzeScreen.SetActive(false);
                AudioManager.instance.PlaySoundEffect(0);
            }
            
        }
    }

    /// <summary>
    /// loads the mainmenu scene, plays a click sounds and sets background music
    /// </summary>
    public void LoadMainMenuButton()
    {
        AudioManager.instance.PlaySoundEffect(0);
        AudioManager.instance.PlayBackGroundMusic(1);
        GameManager.instance.gameRunning = false;
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// loads the game scene, plays a click sounds and sets background music 
    /// </summary>
    public void LoadGameButton()
    {
        AudioManager.instance.PlaySoundEffect(0);
        AudioManager.instance.PlayBackGroundMusic(2);
        GameManager.instance.gameRunning = true;
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Starts the ExitGame Coroutine need to be a public void for the button
    /// </summary>
    public void ExitGameButton()
    {
        StartCoroutine(ExitGame());
    }
}
