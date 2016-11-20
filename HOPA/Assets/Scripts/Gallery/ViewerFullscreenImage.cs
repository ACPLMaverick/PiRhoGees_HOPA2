using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ViewerFullscreenImage : MonoBehaviour
{
    #region Fields

    #endregion

    #region Properties

    #endregion

    #region Protected

    protected CanvasGroup _cGrp;
    protected Button _fullscreenButton;
    protected Image _image;

    #endregion

    #region MonoBehaviours

    void Awake()
    {
        _cGrp = GetComponent<CanvasGroup>();
        _fullscreenButton = GetComponentInChildren<Button>();
        _fullscreenButton.onClick.AddListener(new UnityEngine.Events.UnityAction(Hide));
        _image = GetComponentInChildren<Image>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Functions Public

    public void Show()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(Utility.FadeCoroutineUI(_cGrp, 0.0f, 1.0f, 0.5f, true));
    }

    public void Show(Sprite sprite)
    {
        if(_image.sprite != sprite)
        {
            _image.sprite = sprite;
            Show();
        }
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(Utility.FadeCoroutineUI(_cGrp, 1.0f, 0.0f, 0.5f, false));
    }

    #endregion

    #region Functions Protected

    #endregion
}
