using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoFullscreen : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected bool _buttonTotal = false;

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

    #region Properties

    public Button ButtonForward
    {
        get
        {
            return _btnFwd;
        }
    }

    public Button ButtonBackward
    {
        get
        {
            return _btnBwd;
        }
    }

    public Button ButtonTotal
    {
        get
        {
            return _btnTotal;
        }
    }

    #endregion

    #region Protected

    protected Text _title;
    protected Text _text;
    protected Button _btnFwd;
    protected Button _btnBwd;
    [SerializeField]
    protected Button _btnTotal;

    protected CanvasGroup _grpForward = null;
    protected CanvasGroup _grpBackward = null;

    #endregion

    #region MonoBehaviour

    protected virtual void Awake()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        Button[] buttons = GetComponentsInChildren<Button>();
        _title = texts[0];
        _text = texts[1];
        _btnBwd = buttons[0];
        _btnFwd = buttons[1];
        _btnTotal = buttons[2];
        _btnTotal.gameObject.SetActive(_buttonTotal);
        _btnFwd.gameObject.SetActive(!_buttonTotal);
        _btnBwd.gameObject.SetActive(!_buttonTotal);
        _btnTotal.onClick.AddListener(new UnityEngine.Events.UnityAction(ForwardButtonClick));
    }

    // Use this for initialization
    void Start ()
    {

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
