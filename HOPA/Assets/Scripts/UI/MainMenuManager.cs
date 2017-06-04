using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    #region Protected

    #region Fields

    [SerializeField]
    protected CanvasGroup _screenMainMenu;

    [SerializeField]
    protected CanvasGroup _screenJadalna;

    [SerializeField]
    protected CanvasGroup _screenLustrzana;


    [SerializeField]
    protected CanvasGroup _infoFullscreenJadalna;

    [SerializeField]
    protected CanvasGroup _infoFullscreenLustrzana;

    [SerializeField]
    protected CanvasGroup _infoFullscreenAdditionalJadalna;

    [SerializeField]
    protected CanvasGroup _infoFullscreenAdditionalLustrzana;


    [SerializeField]
    protected Text _screenJadalnaInfoComingSoonText;

    [SerializeField]
    protected Image _screenJadalnaInfoImage;

    [SerializeField]
    protected Text _screenLustrzanaInfoComingSoonText;

    [SerializeField]
    protected Image _screenLustrzanaInfoImage;

    #endregion

    protected CanvasGroup _currentScreen;
    protected LanguageManager _myLanguage;

    #endregion

    #region Monobehaviour

    // Use this for initialization
    void Start ()
    {
        SwitchToScreen(_screenMainMenu);
        _myLanguage = GetComponent<LanguageManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    protected virtual void OnApplicationQuit()
    {

    }

    #endregion

    #region Button Handlers

    public void ButtonClickMenuJadalna()
    {
        SwitchToScreen(_screenJadalna);
        //_infoFullscreenJadalna.GetComponent<Switchable>().SwitchOn();
    }

    public void ButtonClickMenuLustrzana()
    {
        SwitchToScreen(_screenLustrzana);
        if (_myLanguage.CurrentLanguage == LanguageManager.Language.English)
        {
            _screenLustrzanaInfoComingSoonText.gameObject.SetActive(true);
            _screenLustrzanaInfoComingSoonText.GetComponentInParent<Button>().interactable = false;
            _screenLustrzanaInfoImage.sprite = Resources.Load<Sprite>("Lustrzana/lustrzana_0030_info_l_CS");
        }
        else
        {
            _screenLustrzanaInfoComingSoonText.gameObject.SetActive(false);
            _screenLustrzanaInfoComingSoonText.GetComponentInParent<Button>().interactable = true;
            _screenLustrzanaInfoImage.sprite = Resources.Load<Sprite>("Lustrzana/lustrzana_0023_info_l");
        }
        //_infoFullscreenLustrzana.GetComponent<Switchable>().SwitchOn();
    }

    public void ButtonClickMenuHirszenberg()
    {
        SceneChangeManager.Instance.ChangeScene(8);
    }

    public void ButtonClickMenuLanguage()
    {
    }

    public void ButtonClickJadalnaGallery()
    {
        SceneChangeManager.Instance.ChangeScene(4);
    }

    public void ButtonClickJadalnaHOPA()
    {
        SceneChangeManager.Instance.ChangeScene(1);
    }

    public void ButtonClickJadalnaConservation()
    {
        SceneChangeManager.Instance.ChangeScene(2);
    }

    public void ButtonClickJadalnaFurnace()
    {
        SceneChangeManager.Instance.ChangeScene(3);
    }

    public void ButtonClickJadalnaInfo()
    {
        SceneChangeManager.Instance.ChangeScene(9);
        //_infoFullscreenAdditionalJadalna.GetComponent<Switchable>().SwitchOn();
    }

    public void ButtonClickLustrzanaGallery()
    {
        SceneChangeManager.Instance.ChangeScene(7);
    }

    public void ButtonClickLustrzanaHOPA()
    {
        SceneChangeManager.Instance.ChangeScene(8);     // TO BE CHANGED!
    }

    public void ButtonClickLustrzanaReconstruction()
    {
        SceneChangeManager.Instance.ChangeScene(5);
    }

    public void ButtonClickLustrzanaDifferences()
    {
        SceneChangeManager.Instance.ChangeScene(6);
    }

    public void ButtonClickLustrzanaInfo()
    {
        SceneChangeManager.Instance.ChangeScene(10);
        //_infoFullscreenAdditionalLustrzana.GetComponent<Switchable>().SwitchOn();
    }

    public void ButtonClickBack()
    {
        SwitchToScreen(_screenMainMenu);
    }

    #endregion

    #region FunctionsInternal

    protected void SwitchToScreen(CanvasGroup grp)
    {
        if(_currentScreen != null)
        {
            _currentScreen.GetComponent<Switchable>().SwitchOff();
        }
        grp.GetComponent<Switchable>().SwitchOn();
        _currentScreen = grp;
    }

    #endregion
}
