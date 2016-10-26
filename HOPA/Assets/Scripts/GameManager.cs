using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    #region const

    private const float WAIT_FOR_ENDING_PHOTO_SECONDS = 2.0f;

    #endregion

    #region public

    public Room CurrentRoom;

    public Image FadeImage;
    public ItemInfo ItemInfoGroup;
    public PauseMenu PauseMenuGroup;
    public Switchable EndingInfoFullscreen;

    public float RoomTransitionTime = 2.0f;

    #endregion

    #region private

    private Room _nextRoom = null;
    private bool _moveInProgress = false;
    private Coroutine _moveCoroutine = null;
    private Coroutine _endCoroutine = null;

    #endregion

    // Use this for initialization
    void Start ()
    {
        CameraManager.Instance.Enabled = CurrentRoom.CameraEnabled;
        CurrentRoom.Initialize();
        CurrentRoom.Enter();
        CurrentRoom.FinishedEvent.AddListener(OnRoomCommonPickablesCollected);
        AudioManager.Instance.PlayMusic(CurrentRoom.AmbientTheme, 0.5f);
        ItemInfoGroup.gameObject.SetActive(false);
        PauseMenuGroup.gameObject.SetActive(false);

        if(EndingInfoFullscreen != null)
        {
            EndingInfoFullscreen.gameObject.SetActive(false);
            EndingInfoFullscreen.GetComponent<InfoFullscreen>().ButtonTotal.onClick.AddListener(new UnityEngine.Events.UnityAction(EndRoom));
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void TransitionToRoom(Room room)
    {
        _nextRoom = room;
        int t = 7;
        if(room is RoomPuzzleAssign)
        {
            t = 11;
        }

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);
        if (_endCoroutine != null)
            StopCoroutine(_endCoroutine);

        _moveCoroutine = StartCoroutine(StartMoveCoroutine(t));
        if (CurrentRoom.ParentRoom == null && _nextRoom.ParentRoom != CurrentRoom)
        {
            AudioManager.Instance.PlayMusic(room.AmbientTheme, RoomTransitionTime);
        }
    }

    public void TogglePauseMenu()
    {
        if(PauseMenuGroup.gameObject.activeSelf)
        {
            PauseMenuGroup.Hide();
        }
        else
        {
            PauseMenuGroup.Show();
        }
    }

    public void ShowPauseMenu()
    {
        PauseMenuGroup.Show();
    }

    public void ExitGame()
    {
        StartCoroutine(ExitGameCoroutine(0));
    }

    private IEnumerator ExitGameCoroutine(int sceneIndex)
    {
        float cTime = Time.time;
        FadeImage.gameObject.SetActive(true);
        FadeImage.canvasRenderer.SetAlpha(0.0f);

        while (Time.time - cTime <= RoomTransitionTime * 0.5f)
        {
            float lerpValue = (Time.time - cTime) / (RoomTransitionTime * 0.5f);
            FadeImage.canvasRenderer.SetAlpha(lerpValue);
            yield return null;
        }
        FadeImage.canvasRenderer.SetAlpha(1.0f);

        SceneChangeManager.Instance.ChangeScene(0);
    }

    private IEnumerator StartMoveCoroutine(int tutorialMsg)
    {
        _moveInProgress = true;
        float cTime = Time.time;
        FadeImage.gameObject.SetActive(true);
        FadeImage.canvasRenderer.SetAlpha(0.0f);

        while (Time.time - cTime <= RoomTransitionTime * 0.5f)
        {
            float lerpValue = (Time.time - cTime) / (RoomTransitionTime * 0.5f);
            FadeImage.canvasRenderer.SetAlpha(lerpValue);
            yield return null;
        }
        FadeImage.canvasRenderer.SetAlpha(1.0f);
        MoveToCurrentRoom();

        //Thanks to this, tutorial message will appear when screen fades out
        TutorialManager.Instance.GoStepFurther(tutorialMsg);
        _moveInProgress = false;

        yield return null;
    }

    private IEnumerator EndMoveCoroutine()
    {
        float cTime = Time.time;

        while (Time.time - cTime <= RoomTransitionTime * 0.5f)
        {
            float lerpValue = (Time.time - cTime) / (RoomTransitionTime * 0.5f);
            FadeImage.canvasRenderer.SetAlpha(1.0f - lerpValue);
            yield return null;
        }
        FadeImage.canvasRenderer.SetAlpha(0);
        FadeImage.gameObject.SetActive(false);
        yield return null;
    }

    private void MoveToCurrentRoom()
    {
        CurrentRoom.Leave();
        Room tmpCurrentRoom = CurrentRoom;
        CurrentRoom = _nextRoom;
        //PickableHintManager.Instance.Flush();
        //_nextRoom = CurrentRoom.PuzzleRoom;

        if (tmpCurrentRoom.ParentRoom == null && _nextRoom.ParentRoom != tmpCurrentRoom)
        {
            EquipmentManager.Instance.FlushOnNextRoom();
        }

        CurrentRoom.Initialize();   // nothing will happen if already initialized
        CurrentRoom.Enter();

        CameraManager.Instance.RecalculateToCurrentRoom();
        CameraManager.Instance.Enabled = CurrentRoom.CameraEnabled;

        if (_endCoroutine != null)
            StopCoroutine(_endCoroutine);
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);
        _endCoroutine = StartCoroutine(EndMoveCoroutine());
    }

    private void OnRoomCommonPickablesCollected(Room r)
    {
        ItemInfoGroup.GetComponentInChildren<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(EndRoomOnInfoButtonClicked));
    }

    private void EndRoomOnInfoButtonClicked()
    {
        ItemInfoGroup.GetComponentInChildren<Button>().onClick.RemoveAllListeners();          // !!!
        StartCoroutine(RoomFinishedCoroutine());
    }

    private void EndRoom()
    {
        SceneChangeManager.Instance.ChangeScene(0);
    }

    private IEnumerator RoomFinishedCoroutine()
    {
        yield return new WaitForSeconds(WAIT_FOR_ENDING_PHOTO_SECONDS);

        if (EndingInfoFullscreen != null)
        {
            EndingInfoFullscreen.SwitchOn();
        }
        else
        {
            EndRoom();
        }

        yield return null;
    }
}
