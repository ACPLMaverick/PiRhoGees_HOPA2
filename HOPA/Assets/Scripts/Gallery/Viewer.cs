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
    }

    #endregion

    #region Fields

    [SerializeField]
    protected ViewerScreen _viewerScreenPrototype;

    [SerializeField]
    protected ViewerFullscreenImage _fullscreenImage;

    [SerializeField]
    protected Button _changeButtonPrototype;

    [SerializeField]
    protected List<ImageMiniaturePair> _resources;

    #endregion

    #region Properties

    #endregion

    #region Protected

    protected List<ViewerScreen> _viewerScreens = new List<ViewerScreen>();
    protected List<Button> _changeButtons = new List<Button>();
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

        int maxButtonsPerScreen = ViewerScreen.MAX_BUTTONS_PER_LINE * ViewerScreen.MAX_BUTTON_LINES;

        ViewerScreen vs = _viewerScreenPrototype;
        int totalImgsCount = 0;
        int wideImgCounter = 0;
        List<ImageMiniaturePair> tempImages = new List<ImageMiniaturePair>();
        for (int i = 0; i < _resources.Count; ++i)
        {
            tempImages.Add(_resources[i]);
            ++totalImgsCount;
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

    public void ChangeScreen(Button btn)
    {

    }

    #endregion

    #region Functions Protected

    protected ViewerScreen SpawnViewerScreen()
    {
        ViewerScreen vs = (Instantiate(_viewerScreenPrototype.gameObject)).GetComponent<ViewerScreen>();
        return vs;
    }

    #endregion
}
