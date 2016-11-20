using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ViewerImageButton : MonoBehaviour
{
    #region Fields

    #endregion

    #region Properties

    #endregion

    #region Protected

    protected Button _buttonEnlarge;
    protected Image _img;
    protected RectTransform _myRectTransform;
    protected RectTransform _childRectTransform;

    protected Sprite _spriteBig;
    protected Sprite _spriteMini;
    protected ViewerFullscreenImage _vfs;

    #endregion

    #region MonoBehaviours

    void Awake()
    {
        _buttonEnlarge = GetComponentInChildren<Button>();
        _img = GetComponentInChildren<Image>();
        _myRectTransform = GetComponentsInChildren<RectTransform>()[0];
        _childRectTransform = GetComponentsInChildren<RectTransform>()[1];
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

    public void Initialize(Viewer.ImageMiniaturePair pair, ViewerFullscreenImage vfs)
    {
        _spriteBig = pair.Image;
        _spriteMini = pair.Miniature;

        _buttonEnlarge.onClick.AddListener(new UnityEngine.Events.UnityAction(OnButtonClick));
        _img.sprite = _spriteMini;
        _vfs = vfs;
    }

    #endregion

    #region Functions Protected

    protected void OnButtonClick()
    {
        _vfs.Show(_spriteBig);
    }

    #endregion
}
