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

    #endregion

    protected CanvasGroup _currentScreen;

    #endregion

    protected const string PP_LAST_SCREEN = "MenuLastScreen";
    protected enum MenuScreen
    {
        Main,
        Jadalna,
        Lustrzana,
        Galeria
    }


    #region Monobehaviour

    // Use this for initialization
    void Start ()
    {
        // load last screen state from playerprefs and set it here
        MenuScreen screen = (MenuScreen)PlayerPrefs.GetInt(PP_LAST_SCREEN, (int)MenuScreen.Main);
        switch (screen)
        {
            case MenuScreen.Jadalna:
                SwitchToScreen(_screenJadalna);
                break;
            case MenuScreen.Lustrzana:
                SwitchToScreen(_screenLustrzana);
                break;
            case MenuScreen.Galeria:
                //SwitchToScreen(_);
                break;
            default:
                SwitchToScreen(_screenMainMenu);
                break;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    protected virtual void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(PP_LAST_SCREEN, (int)MenuScreen.Main);
    }

    #endregion

    #region Button Handlers

    public void ButtonClickMenuJadalna()
    {
        SwitchToScreen(_screenJadalna);
        _infoFullscreenJadalna.GetComponent<Switchable>().SwitchOn();
        PlayerPrefs.SetInt(PP_LAST_SCREEN, (int)MenuScreen.Jadalna);
    }

    public void ButtonClickMenuLustrzana()
    {
        SwitchToScreen(_screenLustrzana);
        _infoFullscreenLustrzana.GetComponent<Switchable>().SwitchOn();
        PlayerPrefs.SetInt(PP_LAST_SCREEN, (int)MenuScreen.Lustrzana);
    }

    public void ButtonClickMenuGallery()
    {
        SceneChangeManager.Instance.ChangeScene(4);
        PlayerPrefs.SetInt(PP_LAST_SCREEN, (int)MenuScreen.Jadalna);
    }

    public void ButtonClickMenuLanguage()
    {
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
        _infoFullscreenAdditionalJadalna.GetComponent<Switchable>().SwitchOn();
    }

    public void ButtonClickLustrzanaHOPA()
    {
        SceneChangeManager.Instance.ChangeScene(1);     // TO BE CHANGED!
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
        _infoFullscreenAdditionalLustrzana.GetComponent<Switchable>().SwitchOn();
    }

    public void ButtonClickBack()
    {
        PlayerPrefs.SetInt(PP_LAST_SCREEN, (int)MenuScreen.Main);
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
