using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeManager : Singleton<SceneChangeManager>
{
    #region constants

    private const float FADE_TIME = 0.3f;
    private const float ROTATE_SPEED = 100.0f;
    private const string PP_KEY = "LoadingScreenIn";
    private const float IMPOSSIBLE_ANGLE = 0.0f;

    #endregion

    #region public 

    public string LoadingString;
    public Image LoadingScreen;
    public Canvas CurrentCanvas;

    #endregion

    #region private

    private Image _animatedImage;
    private Text _loadingText;

    private Image _prevLoadingScreen;

    private AsyncOperation _loadOperation = null;
    private int _loadedSceneIndex = -1;

    #endregion

    #region functions

    protected override void Awake()
    {
        _destroyOnLoad = true;
        _animatedImage = LoadingScreen.GetComponentsInChildren<Image>()[1];
        _loadingText = LoadingScreen.GetComponentInChildren<Text>();
        _loadingText.text = LoadingString;
        base.Awake();
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_loadOperation != null)
        {
            if(!_loadOperation.isDone)
            {
                UpdateLoading();
            }
            else
            {
                FinishLoading();
            }
        }
    }

    public virtual void OnDestroy()
    {
    }

    public void ChangeScene(int index)
    {
        _prevLoadingScreen = LoadingScreen;
        _loadingText = LoadingScreen.GetComponentInChildren<Text>();
        _animatedImage.GetComponent<Animator>().Stop();

        LoadingScreen.gameObject.SetActive(true);
        StartCoroutine(FadeCoroutineUI(LoadingScreen.GetComponent<CanvasGroup>(), 0.0f, 1.0f, FADE_TIME, true, index));
    }

    private void ChangeSceneIndeed(int index)
    {
        _loadOperation = SceneManager.LoadSceneAsync(index);
        _loadOperation.allowSceneActivation = true;
        _loadedSceneIndex = index;
    }

    private void FinishLoading()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(_loadedSceneIndex));
        SceneManager.UnloadScene(currentScene);
        _loadOperation = null;
    }

    private void UpdateLoading()
    {
        _animatedImage.GetComponent<Animator>().Play("LoadingScreenAnimation", 0, _loadOperation.progress);
    }

    private IEnumerator FadeCoroutineUI(CanvasGroup grp, float fadeStart, float fadeTarget, float timeSec, bool active, int nextSceneIndex)
    {
        float currentTime = Time.time;

        grp.alpha = fadeStart;

        while (Time.time - currentTime <= timeSec)
        {
            float lerp = (Time.time - currentTime) / timeSec;

            grp.alpha = Mathf.Lerp(fadeStart, fadeTarget, lerp);
            yield return null;
        }
        grp.alpha = fadeTarget;
        grp.gameObject.SetActive(active);

        if(active)
        {
            ChangeSceneIndeed(nextSceneIndex);
        }
        else
        {
            
        }

        yield return null;
    }

    #endregion
}
