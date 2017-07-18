using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public enum RoomType
{
    DINNER,
    MIRROR,
    NONE
}

public class MinigameProgressManager<T> : MonoBehaviour
{
    #region Const

    public const float WAIT_FOR_ENDING_PHOTO_SECONDS = 3.0f;

    #endregion

    public T[] Elements;
    public int ElementsCompleted;
    public bool HasWon;
    public RoomType Type;

    public PauseMenu PauseMenuGroup;
    public Switchable InfoFullscreenGroup;

    // Use this for initialization
    public virtual void Start()
    {
        InfoFullscreenGroup.gameObject.SetActive(false);
        Type = RoomType.NONE;
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
            SetInfoVisuals();
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

    private void SetInfoVisuals()
    {
        switch(Type)
        {
            case RoomType.DINNER:
                InfoFullscreenGroup.GetComponent<Image>().sprite = Resources.Load<Sprite>("Jadalna/ekran_wygranej_jadalna");
                break;
            case RoomType.MIRROR:
                InfoFullscreenGroup.GetComponent<Image>().sprite = Resources.Load<Sprite>("Lustrzana/ekran_wygranej_lustrzana");
                break;
            case RoomType.NONE:
                break;
        }
    }
}
