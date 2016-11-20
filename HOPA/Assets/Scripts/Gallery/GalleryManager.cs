using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GalleryManager : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected Button _backButton;
    [SerializeField]
    protected Viewer _viewer;

    #endregion

    #region Properties

    #endregion

    #region Protected

    #endregion

    #region MonoBehaviours

    void Awake()
    {
        _backButton.onClick.AddListener(new UnityEngine.Events.UnityAction(Exit));
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

    #endregion

    #region Functions Protected

    protected void Exit()
    {
        SceneChangeManager.Instance.ChangeScene(0);
    }

    #endregion
}
