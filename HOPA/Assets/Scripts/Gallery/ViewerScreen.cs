using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ViewerScreen : MonoBehaviour
{
    #region Public Static Const

    public const int MAX_BUTTON_LINES = 3;
    public const int MAX_BUTTONS_PER_LINE = 4;
    public const float BUTTON_SQUARE_SIZE = 425.0f;
    public const float BUTTON_WIDE_SIZE_X = 569.0f;

    #endregion

    #region Fields

    [SerializeField]
    protected GameObject _changeButtonPrefab;

    #endregion

    #region Properties

    #endregion

    #region Protected

    protected List<ViewerImageButton> _iButtons = new List<ViewerImageButton>();
    protected ViewerFullscreenImage _fullscreenImage;

    #endregion

    #region MonoBehaviours

    void Awake()
    {

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

    public void Initialize(List<Viewer.ImageMiniaturePair> images, ViewerFullscreenImage vfs)
    {
        _fullscreenImage = vfs;
        int currentRowCount = 0;
        for (int i = 0; i < images.Count; ++i)
        {
            ViewerImageButton btn = SpawnButton();

            // adjust size if it is wide image
            if (images[i].IsWide)
            {
                MakeWide(btn);
            }

            // initialize
            btn.Initialize(images[i], _fullscreenImage);

            btn.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>(), false);

            // calculate current position in grid and offset
            if (i == 0)
            {
                Vector2 currentPosition = Vector2.zero;
                btn.GetComponent<RectTransform>().anchoredPosition = currentPosition;
                ++currentRowCount;
            }
            else if(currentRowCount == 4 || (currentRowCount == 3 && images[i - 1].IsWide))
            {
                Vector3 currentPosition = _iButtons[i - 1].GetComponent<RectTransform>().anchoredPosition;
                currentPosition.x = 0.0f;
                currentPosition.y -= _iButtons[i - 1].GetComponent<RectTransform>().sizeDelta.y;
                btn.GetComponent<RectTransform>().anchoredPosition = currentPosition;
                currentRowCount = 1;
            }
            else
            {
                Vector3 currentPosition = _iButtons[i - 1].GetComponent<RectTransform>().anchoredPosition;
                currentPosition.x += _iButtons[i - 1].GetComponent<RectTransform>().sizeDelta.x;
                btn.GetComponent<RectTransform>().anchoredPosition = currentPosition;
                ++currentRowCount;
            }

            btn.GetComponent<RectTransform>().localScale = Vector3.one;
            _iButtons.Add(btn);
        }
    }

    public void Show()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(Utility.FadeCoroutineUI(GetComponent<CanvasGroup>(), 0.0f, 1.0f, 0.5f, true));
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(Utility.FadeCoroutineUI(GetComponent<CanvasGroup>(), 1.0f, 0.0f, 0.5f, false));
    }

    public int GetButtonCount()
    {
        return _iButtons.Count;
    }

    #endregion

    #region Functions Protected

    protected ViewerImageButton SpawnButton()
    {
        return (Instantiate(_changeButtonPrefab.gameObject)).GetComponent<ViewerImageButton>();
    }

    protected void MakeWide(ViewerImageButton vcb)
    {
        vcb.GetComponent<RectTransform>().sizeDelta = new Vector2(BUTTON_WIDE_SIZE_X, BUTTON_SQUARE_SIZE);
    }

    #endregion
}
