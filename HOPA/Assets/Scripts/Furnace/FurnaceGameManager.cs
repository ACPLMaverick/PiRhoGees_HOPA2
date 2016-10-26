using UnityEngine;
using System.Collections;

public class FurnaceGameManager : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected PauseMenu _pauseMenuGroup;

    [SerializeField]
    protected InfoFullscreen _infoFullscreenGroup;

    #endregion

    #region Protected

    #endregion

    #region Monobehaviours

    // Use this for initialization
    void Start ()
    {
        _infoFullscreenGroup.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    #endregion

    #region Functions Public

    public void TogglePauseMenu()
    {
        if (_pauseMenuGroup.gameObject.activeSelf)
        {
            _pauseMenuGroup.Hide();
        }
        else
        {
            _pauseMenuGroup.Show();
        }
    }

    #endregion
}
