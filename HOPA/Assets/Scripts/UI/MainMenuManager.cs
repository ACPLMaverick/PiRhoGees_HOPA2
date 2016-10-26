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
        PlayerPrefs.SetInt(PP_LAST_SCREEN, (int)MenuScreen.Galeria);
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

    }

    public void ButtonClickJadalnaFurnace()
    {

    }

    public void ButtonClickJadalnaInfo()
    {

    }

    public void ButtonClickLustrzanaHOPA()
    {

    }

    public void ButtonClickLustrzanaReconstruction()
    {

    }

    public void ButtonClickLustrzanaDifferences()
    {

    }

    public void ButtonClickLustrzanaInfo()
    {

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
