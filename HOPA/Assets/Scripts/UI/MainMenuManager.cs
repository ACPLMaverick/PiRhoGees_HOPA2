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

    #region Monobehaviour

    // Use this for initialization
    void Start ()
    {
        SwitchToScreen(_screenMainMenu);
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
        _infoFullscreenJadalna.GetComponent<Switchable>().SwitchOn();
    }

    public void ButtonClickMenuLustrzana()
    {
        SwitchToScreen(_screenLustrzana);
        _infoFullscreenLustrzana.GetComponent<Switchable>().SwitchOn();
    }

    public void ButtonClickMenuGallery()
    {
        SceneChangeManager.Instance.ChangeScene(4);
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
        SceneChangeManager.Instance.ChangeScene(7);     // TO BE CHANGED!
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
