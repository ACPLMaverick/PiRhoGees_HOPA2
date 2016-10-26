using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MinigameProgressManager<T> : MonoBehaviour
{
    #region Const

    public const float WAIT_FOR_ENDING_PHOTO_SECONDS = 1.0f;

    #endregion

    public T[] Elements;
    public int ElementsCompleted;
    public bool HasWon;

    public PauseMenu PauseMenuGroup;
    public Switchable InfoFullscreenGroup;

    // Use this for initialization
    public virtual void Start()
    {
        InfoFullscreenGroup.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!HasWon)
        {
            CheckProgress();
        }

        if (Input.GetKey(KeyCode.R))
        {
            ResetGame();
        }
    }

    public virtual void CheckProgress()
    {
        if (ElementsCompleted >= Elements.Length)
        {
            WinGame();
        }
    }

    public virtual void WinGame()
    {
        HasWon = true;

        if (InfoFullscreenGroup != null)
        {
            InfoFullscreenGroup.GetComponent<InfoFullscreen>().ButtonTotal.onClick.AddListener(new UnityEngine.Events.UnityAction(BackToMenu));
        }
        StartCoroutine(RoomFinishedCoroutine());

        Debug.Log("Hooray! You won!");
    }

    public virtual void ResetGame()
    {
        ElementsCompleted = 0;
        HasWon = false;
    }

    public void TogglePauseMenu()
    {
        if (PauseMenuGroup.gameObject.activeSelf)
        {
            PauseMenuGroup.Hide();
        }
        else
        {
            PauseMenuGroup.Show();
        }
    }

    public void BackToMenu()
    {
        SceneChangeManager.Instance.ChangeScene(0);
    }

    private IEnumerator RoomFinishedCoroutine()
    {
        yield return new WaitForSeconds(WAIT_FOR_ENDING_PHOTO_SECONDS);

        if (InfoFullscreenGroup != null)
        {
            InfoFullscreenGroup.SwitchOn();
        }
        else
        {
            BackToMenu();
        }

        yield return null;
    }
}
