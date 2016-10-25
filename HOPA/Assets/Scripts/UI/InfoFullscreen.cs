using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoFullscreen : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected string _initTitleText;

    [SerializeField]
    protected string _initMainText;

    #endregion

    #region Properties

    /// <summary>
    /// After this field is set, its value remains until forward or backward button is pressed
    /// When it happens it is nulled again.
    /// </summary>
    public CanvasGroup GroupForward
    {
        get
        {
            return _grpForward;
        }
        set
        {
            _grpForward = value;
        }
    }

    /// <summary>
    /// After this field is set, its value remains until forward or backward button is pressed
    /// When it happens it is nulled again.
    /// </summary>
    public CanvasGroup GroupBackward
    {
        get
        {
            return _grpBackward;
        }
        set
        {
            _grpBackward = value;
        }
    }

    #endregion

    #region Protected

    protected Text _title;
    protected Text _text;

    protected CanvasGroup _grpForward = null;
    protected CanvasGroup _grpBackward = null;

    #endregion

    #region MonoBehaviour

    // Use this for initialization
    void Start ()
    {
        _title = GetComponentsInChildren<Text>()[0];
        _text = GetComponentsInChildren<Text>()[1];

        _title.text = _initTitleText;
        _text.text = _initMainText;
    }

    // Update is called once per frame
    void Update ()
    {
	
	}

    #endregion

    #region Functions Public

    public void SetData(string title, string text)
    {
        _title.text = title;
        _text.text = text;
    }

    public void ForwardButtonClick()
    {
        GetComponent<Switchable>().SwitchOff();
        if(_grpForward != null)
        {
            _grpForward.GetComponent<Switchable>().SwitchOn();
            _grpForward = null;
        }
        if (_grpBackward != null)
        {
            _grpBackward.GetComponent<Switchable>().SwitchOff();
            _grpBackward = null;
        }
    }

    public void BackButtonClick()
    {
        GetComponent<Switchable>().SwitchOff();
        if (_grpForward != null)
        {
            _grpForward.GetComponent<Switchable>().SwitchOff();
            _grpForward = null;
        }
        if (_grpBackward != null)
        {
            _grpBackward.GetComponent<Switchable>().SwitchOn();
            _grpBackward = null;
        }
    }

    #endregion
}
