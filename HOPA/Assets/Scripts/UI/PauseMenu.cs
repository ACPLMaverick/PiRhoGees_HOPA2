using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    #region private

    [SerializeField]
    CanvasGroup _fadeImage;

    private CanvasGroup _grp;
    private Button _btnExit;
    private Button _btnReturn;

    #endregion

    #region functions

    protected virtual void Awake()
    {
        _grp = GetComponent<CanvasGroup>();
        Button[] btns = GetComponentsInChildren<Button>();

        foreach (Button b in btns)
        {
            if (b.name.Contains("Exit"))
            {
                _btnExit = b;
            }
            else if (b.name.Contains("Return"))
            {
                _btnReturn = b;
            }
        }

        _btnExit.onClick.AddListener(new UnityEngine.Events.UnityAction(OnExitButtonClick));
        if (_btnReturn != null)
        {
            _btnReturn.onClick.AddListener(new UnityEngine.Events.UnityAction(OnReturnButtonClick));
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            StartCoroutine(FadeCoroutine(0.0f, 1.0f, 0.5f, true));
        }
    }

    public void Hide()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(FadeCoroutine(1.0f, 0.0f, 0.5f, false));
        }
    }

    private IEnumerator FadeCoroutine(float fadeStart, float fadeTarget, float timeSec, bool active)
    {
        float currentTime = Time.time;

        _grp.alpha = fadeStart;

        while (Time.time - currentTime <= timeSec)
        {
            float lerp = (Time.time - currentTime) / timeSec;

            _grp.alpha = Mathf.Lerp(fadeStart, fadeTarget, lerp);
            yield return null;
        }
        _grp.alpha = fadeTarget;
        gameObject.SetActive(active);

        yield return null;
    }

    private void OnExitButtonClick()
    {
        StartCoroutine(ExitToMenuCoroutine());
    }

    private void OnReturnButtonClick()
    {
        Hide();
    }

    private IEnumerator ExitToMenuCoroutine()
    {
        _btnExit.enabled = false;
        _fadeImage.gameObject.SetActive(true);
        StartCoroutine(Utility.FadeCoroutineUI(_fadeImage, 0.0f, 1.0f, 2.0f, true));

        yield return new WaitForSeconds(2.0f);

        SceneChangeManager.Instance.ChangeScene(0);

        yield return null;
    }

    #endregion
}
