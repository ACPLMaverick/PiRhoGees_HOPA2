using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FurnaceGameManager : MonoBehaviour
{
    #region Const

    protected const float WAIT_FOR_ENDING_PHOTO_SECONDS = 1.0f;

    #endregion

    #region Fields

    [SerializeField]
    List<RollingCollumn> _collumns;

    [SerializeField]
    protected PauseMenu _pauseMenuGroup;

    [SerializeField]
    protected Switchable _infoFullscreenGroup;

    #endregion

    #region Protected

    int _correctCollumnCtr = 0;

    #endregion

    #region Monobehaviours

    // Use this for initialization
    void Start ()
    {
        _infoFullscreenGroup.gameObject.SetActive(false);

        for(int i = 0; i < _collumns.Count; ++i)
        {
            _collumns[i].EventCorrect.AddListener(new UnityEngine.Events.UnityAction<RollingCollumn>(OnCollumnCorrect));
            if(_collumns[i].CorrectAndLocked)
            {
                ++_correctCollumnCtr;
            }
        }

        if(_correctCollumnCtr == _collumns.Count)
        {
            Finish();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    #endregion

    #region Functions Public

    public void TogglePauseMenu()
    {
        if (_pauseMenuGroup.gameObject.activeSelf)
        {
            _pauseMenuGroup.Hide();
        }
        else
        {
            _pauseMenuGroup.Show();
        }
    }

    #endregion

    #region Functions Protected

    protected void OnCollumnCorrect(RollingCollumn collumn)
    {
        ++_correctCollumnCtr;

        if(_correctCollumnCtr == _collumns.Count)
        {
            Finish();
        }
    }

    protected void Finish()
    {
        if (_infoFullscreenGroup != null)
        {
            _infoFullscreenGroup.GetComponent<InfoFullscreen>().ButtonTotal.onClick.AddListener(new UnityEngine.Events.UnityAction(BackToMenu));
        }
        StartCoroutine(RoomFinishedCoroutine());
    }

    protected void BackToMenu()
    {
        SceneChangeManager.Instance.ChangeScene(0);
    }

    private IEnumerator RoomFinishedCoroutine()
    {
        yield return new WaitForSeconds(WAIT_FOR_ENDING_PHOTO_SECONDS);

        if (_infoFullscreenGroup != null)
        {
            _infoFullscreenGroup.SwitchOn();
        }
        else
        {
            BackToMenu();
        }

        yield return null;
    }

    #endregion
}
