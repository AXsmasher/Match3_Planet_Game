using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioSource audioSource;
    public TMPro.TMP_Text text;
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            GlobalSceneVariables.paused = pauseMenu.activeSelf;
            animator.SetBool("Paused", GlobalSceneVariables.paused);
        }
    }

    public void Close()
    {
        pauseMenu.SetActive(false);
        GlobalSceneVariables.paused = pauseMenu.activeSelf;
        animator.SetBool("Paused", GlobalSceneVariables.paused);
    }

    public void SoundOffOn()
    {
        audioSource.mute = !audioSource.mute;
        if (audioSource.mute)
        {
            text.text = "Sound OFF";
        }
        else
        {
            text.text = "Sound ON";
        }
    }
}
