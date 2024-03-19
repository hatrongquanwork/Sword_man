using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunction : MonoBehaviour
{
    public GameObject FadeOut;
    public GameObject LoadingText;
    public AudioSource ButtonClick;

    void Start()
    {
        // Hiển thị con trỏ chuột
        Cursor.visible = true;
        // Cho phép click chuột
        Cursor.lockState = CursorLockMode.None;
    }


    public void NewGameButton()
    {
        StartCoroutine(NewGameStart());
    }
    
    public void ExitButton()
    {
        Application.Quit();
    }

    IEnumerator NewGameStart()
    {
        ButtonClick.Play();
        FadeOut.SetActive(true);
        
        yield return new WaitUntil(() => FadeOut.activeSelf);
        LoadingText.SetActive(true);

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(2);
    }


}
