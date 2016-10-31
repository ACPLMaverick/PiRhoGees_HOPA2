using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Switchable : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected float _appearTimeSec = 0.5f;

    [SerializeField]
    protected float _disappearTimeSec = 0.5f;

    #endregion

    #region Protected

    #endregion

    #region Monobehaviour

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

    public void SwitchOn()
    {
        if(!gameObject.activeSelf)
        {
            Button[] btns = GetComponentsInChildren<Button>();
            for(int i = 0; i < btns.Length; ++i)
            {
                btns[i].enabled = true;
            }
            StopAllCoroutines();
            gameObject.SetActive(true);
            StartCoroutine(Utility.FadeCoroutineUI(GetComponent<CanvasGroup>(), 0.0f, 1.0f, _appearTimeSec, true));
        }
    }

    public void SwitchOff()
    {
        if(gameObject.activeSelf)
        {
            Button[] btns = GetComponentsInChildren<Button>();
            for (int i = 0; i < btns.Length; ++i)
            {
                btns[i].enabled = false;
            }

            StopAllCoroutines();
            StartCoroutine(Utility.FadeCoroutineUI(GetComponent<CanvasGroup>(), 1.0f, 0.0f, _disappearTimeSec, false));
        }
    }

    #endregion
}
