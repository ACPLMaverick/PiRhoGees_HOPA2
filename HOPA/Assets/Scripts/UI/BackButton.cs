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
        Vector3 scaleOne = Vector3.one;
        Vector3 scaleTwo = scaleOne * 0.8f;

        float lerp = (Mathf.Sin(2.0f * Time.time) + 1.0f) * 0.5f;
        transform.localScale = Vector3.Lerp(scaleOne, scaleTwo, lerp);
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
