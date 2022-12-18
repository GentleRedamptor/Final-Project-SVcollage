using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] PlayerAndCamera player;
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void WinLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SelectLevel(int levelNum)
    {
        SceneManager.LoadScene(levelNum);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetSensitivity(float sensitivity)
    {
        player.sensitivity = sensitivity;
    }


}
