using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FurnaceGameManager : MonoBehaviour
{
    #region Const

    protected const float WAIT_FOR_ENDING_PHOTO_SECONDS = 3.0f;

    #endregion

    #region Fields

    [SerializeField]
    List<RollingCollumn> _collumns;

    [SerializeField]
    protected PauseMenu _pauseMenuGroup;

    [SerializeField]
    protected Switchable _infoFullscreenGroup;

    [SerializeField]
    protected SpriteRenderer _fireImage;

    [SerializeField]
    protected SpriteRenderer _glowImage;

    #endregion

    #region Protected

    int _correctCollumnCtr = 0;
    Color _fireImageColor;
    Color _glowImageColor;

    #endregion

    #region Monobehaviours

    // Use this for initialization
    void Start ()
    {
        _infoFullscreenGroup.gameObject.SetActive(false);
        _fireImageColor = _fireImage.color;
        _glowImageColor = _glowImage.color;

        for (int i = 0; i < _collumns.Count; ++i)
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
        StartCoroutine(ColumnFinishedCoroutine());

        if (_correctCollumnCtr == _collumns.Count)
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

    private IEnumerator ColumnFinishedCoroutine()
    {
        float cTime = Time.time;

        while (Time.time - cTime <= 1f)
        {
            float lerpValue = (Time.time - cTime) / (1f);
            _glowImageColor.a = lerpValue;
            _glowImage.color = _glowImageColor;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        cTime = Time.time;

        while (Time.time - cTime <= 1f)
        {
            float lerpValue = (Time.time - cTime) / (1f);
            _glowImageColor.a = 1 - lerpValue;
            _glowImage.color = _glowImageColor;
            yield return null;
        }

        yield return null;
    }

    private IEnumerator RoomFinishedCoroutine()
    {
        float cTime = Time.time;

        while (Time.time - cTime <= 1.5f)
        {
            float lerpValue = (Time.time - cTime) / (1.5f);
            _fireImageColor.a = lerpValue;
            _fireImage.color = _fireImageColor;
            yield return null;
        }

        yield return new WaitForSeconds(WAIT_FOR_ENDING_PHOTO_SECONDS);

        if (_infoFullscreenGroup != null)
        {
            _infoFullscreenGroup.GetComponent<Image>().sprite = Resources.Load<Sprite>("Jadalna/ekran_wygranej_jadalna");
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
