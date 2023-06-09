using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    //скрипт отвечающий за меню
    [Header("Объекты для активации при мерехоте в то или иное меню")]
    public GameObject Settings_menu, Main_menu, Skill_panel;
    [Header("Логика звуков")]
    public GameObject Sound_on, Sound_off;
    public AudioMixer audio_mix;
    public Slider valume_slider;
    private bool mute_logic = false;

    //метод для контроля звуков ползунком
    public void VolumeControl(float vol)
    {
        audio_mix.SetFloat("MasterVolume", Mathf.Log10(vol) * 20);

        if (mute_logic == true)
        {
            Sound_on.SetActive(true);
            Sound_off.SetActive(false);

            mute_logic = false;
        }
    }
    //логика кнопки "начать игру"
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        Time.timeScale = 1f;
    }
    //логика кнопки возврата в меню
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
    //логика возврата из настроек в главное меню
    public void ExitGame()
    {
        Application.Quit();
    }
    //логика кнопки продолжить
    public void Continue()
    {
        Settings_menu.SetActive(false);
        Skill_panel.SetActive(true);
        Time.timeScale = 1f;
    }
    //логика паузы во время игры
    public void Pause()
    {
        Settings_menu.SetActive(true);
        Skill_panel.SetActive(false);
        Time.timeScale = 0f;
    }
    //логика настроек
    public void Settings()
    {
        Settings_menu.SetActive(true);
        Main_menu.SetActive(false);
    }
    //логика перехода в главное меню из игры
    public void MainMenu()
    {
        Settings_menu.SetActive(false);
        Main_menu.SetActive(true);
        Time.timeScale = 1f;
    }
    //логика включения и выключения звуков
    public void SoundON()
    {
        Sound_on.SetActive(true);
        Sound_off.SetActive(false);

        valume_slider.value = 0.2f;

        mute_logic = false;
    }
    public void SoundOFF()
    {
        Sound_on.SetActive(false);
        Sound_off.SetActive(true);

        valume_slider.value = 0f;

        mute_logic = true;
    }
}
