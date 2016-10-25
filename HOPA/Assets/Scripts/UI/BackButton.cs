using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackButton : MonoBehaviour
{
    #region public

    #endregion

    #region private


    #endregion

    #region functions

    protected virtual void Awake()
    {
    }

    // Use this for initialization
    void Start ()
    {
        GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(OnClick));
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void UpdateOnCurrentRoom()
    {
        if(GameManager.Instance.CurrentRoom.ParentRoom != null)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected void OnClick()
    {
        if (GameManager.Instance.CurrentRoom.ParentRoom != null)
        {
            GameManager.Instance.TransitionToRoom(GameManager.Instance.CurrentRoom.ParentRoom);
        }
    }

    #endregion
}
