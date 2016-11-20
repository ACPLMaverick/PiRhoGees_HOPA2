using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ViewerScreen : MonoBehaviour
{
    #region Public Static Const

    public const int MAX_BUTTON_LINES = 3;
    public const int MAX_BUTTONS_PER_LINE = 4;

    #endregion

    #region Fields

    #endregion

    #region Properties

    #endregion

    #region Protected

    protected List<ViewerImageButton> _iButtons;

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

    public void Initialize(List<Viewer.ImageMiniaturePair> images)
    {

    }

    #endregion

    #region Functions Protected

    #endregion
}
