using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Viewer : MonoBehaviour
{
    #region Classes

    [System.Serializable]
    public class ImageMiniaturePair
    {
        public Sprite Image;
        public Sprite Miniature;
        public bool IsWide;
    }

    #endregion

    #region Fields

    [SerializeField]
    protected GameObject _viewerScreenPrefab;

    [SerializeField]
    protected GameObject _viewerChangeButtonPrefab;

    [SerializeField]
    protected ViewerFullscreenImage _fullscreenImage;

    [SerializeField]
    protected Canvas _currentCanvas;

    [SerializeField]
    protected List<ImageMiniaturePair> _resources;

    #endregion

    #region Properties

    #endregion

    #region Protected

    protected List<ViewerScreen> _viewerScreens = new List<ViewerScreen>();
    protected List<ViewerChangeButton> _changeButtons = new List<ViewerChangeButton>();
    protected int _currentViewerScreenPosition = 0;

    #endregion

    #region MonoBehaviours

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        _fullscreenImage.gameObject.SetActive(false);

        int maxImagesPerScreen = ViewerScreen.MAX_BUTTONS_PER_LINE * ViewerScreen.MAX_BUTTON_LINES;

        ViewerScreen vs = SpawnAndPlaceViewerScreen();
        int totalImgsCount = 0;
        int wideImgCounter = 0;
        List<ImageMiniaturePair> tempImages = new List<ImageMiniaturePair>();
        for (int i = 0; i < _resources.Count; ++i)
        {
            if (_resources[i].IsWide)
            {
                // it is wide miniature
                ++wideImgCounter;
            }
            if (wideImgCounter >= 2)
            {
                wideImgCounter = 0;
                ++totalImgsCount;
            }

            if (totalImgsCount > maxImagesPerScreen)
            {
                wideImgCounter = 0;
                totalImgsCount = 0;

                vs.Initialize(tempImages, _fullscreenImage);

                tempImages.Clear();
                _viewerScreens.Add(vs);
                vs = SpawnAndPlaceViewerScreen();
            }

            tempImages.Add(_resources[i]);
            ++totalImgsCount;
        }

        _viewerScreens.Add(vs);
        vs.Initialize(tempImages, _fullscreenImage);

        if(_viewerScreens.Count > 1)
        {
            for (int i = 1; i < _viewerScreens.Count; ++i)
            {
                _viewerScreens[i].gameObject.SetActive(false);
            }
        }

        // create button for each viewer screen
        for (int i = 0; i < _viewerScreens.Count; ++i)
        {
            int offset = _viewerScreens.Count - i - 1;
            int numberFirst = 0, numberLast = 0;
            if(i > 0)
            {
                for (int j = i - 1; j >= 0; --j)
                {
                    numberFirst += _viewerScreens[j].GetButtonCount();
                }
                ++numberFirst;
            }
            else
            {
                numberFirst = 1;
            }

            numberLast = numberFirst + _viewerScreens[i].GetButtonCount() - 1;
            _changeButtons.Add(SpawnAndPlaceViewerChangeButton(_viewerScreens[i], numberFirst, numberLast, offset));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Functions Public

    public void ChangeScreen(int direction)
    {

    }

    public void ChangeScreen(ViewerScreen screen)
    {
        for(int i = 0; i < _viewerScreens.Count; ++i)
        {
            if(_viewerScreens[i] == screen)
            {
                if(!_viewerScreens[i].gameObject.activeSelf)
                {
                    _viewerScreens[i].Show();
                }
                _currentViewerScreenPosition = i;
            }
            else if(_viewerScreens[i].gameObject.activeSelf)
            {
                _viewerScreens[i].Hide();
            }
        }
    }

    #endregion

    #region Functions Protected

    protected ViewerScreen SpawnAndPlaceViewerScreen()
    {
        ViewerScreen vs = (Instantiate(_viewerScreenPrefab.gameObject)).GetComponent<ViewerScreen>();
        vs.GetComponent<RectTransform>().SetParent(GetComponent<Transform>(), false);
        //// calculate and apply offset
        //float offset = _viewerScreens.Count * _currentCanvas.pixelRect.width * 2.0f;
        //Vector3 currentPos = Vector3.zero;
        //vs.GetComponent<RectTransform>().anchoredPosition = new Vector3(currentPos.x + offset, currentPos.y + 80.0f, currentPos.z);
        vs.GetComponent<RectTransform>().localScale = Vector3.one;

        return vs;
    }

    protected ViewerChangeButton SpawnAndPlaceViewerChangeButton(ViewerScreen associatedScreen, int numberBegin, int numberEnd, int offsetMplier)
    {
        ViewerChangeButton vcb = (Instantiate(_viewerChangeButtonPrefab.gameObject)).GetComponent<ViewerChangeButton>();
        vcb.Screen = associatedScreen;
        vcb.ViewerRef = this;
        vcb.GetComponent<RectTransform>().SetParent(GetComponent<Transform>(), false);
        float offset = offsetMplier * vcb.GetComponent<RectTransform>().sizeDelta.x;
        Vector3 currentPos = vcb.GetComponent<RectTransform>().anchoredPosition;
        currentPos.x -= offset;
        vcb.GetComponent<RectTransform>().anchoredPosition = currentPos;

        Text txt = vcb.GetComponentInChildren<Text>();
        txt.text = string.Format("{0} - {1}", numberBegin, numberEnd);

        return vcb;
    }

    #endregion
}
