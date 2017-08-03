using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Switchable : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected float _appearTimeDelay = 0.0f;

    [SerializeField]
    protected float _appearTimeSec = 0.5f;

    [SerializeField]
    protected float _disappearTimeSec = 0.5f;

    [SerializeField]
    protected float _buttonClickableTimeDelay = 1.0f;

    #endregion

    #region Protected

    [SerializeField]
    List<Switchable> _connectedSwitchables;
    protected float _clickableTimer = 0.0f;

    #endregion

    #region Monobehaviour

    // Use this for initialization
    void Start ()
    {
        Button[] btns = GetComponentsInChildren<Button>();
        for (int i = 0; i < btns.Length; ++i)
        {
            btns[i].enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(gameObject.activeInHierarchy && _clickableTimer < _buttonClickableTimeDelay)
        {
            _clickableTimer += Time.deltaTime;

            if(_clickableTimer >= _buttonClickableTimeDelay)
            {
                Button[] btns = GetComponentsInChildren<Button>();
                for (int i = 0; i < btns.Length; ++i)
                {
                    btns[i].enabled = true;
                }
            }
        }
	}

    #endregion

    #region Functions Public

    public void SwitchOn()
    {
        if(!gameObject.activeSelf)
        {
            _clickableTimer = 0.0f;
            StopAllCoroutines();
            gameObject.SetActive(true);


            foreach (Switchable sw in _connectedSwitchables)
            {
                sw.SwitchOn();
            }

            if(_appearTimeDelay == 0.0f)
            {
                StartCoroutine(Utility.FadeCoroutineUI(GetComponent<CanvasGroup>(), 0.0f, 1.0f, _appearTimeSec, true));
            }
            else
            {
                StartCoroutine(Utility.FadeCoroutineUIDelay(GetComponent<CanvasGroup>(), _appearTimeDelay, 0.0f, 1.0f, _appearTimeSec, true));
            }
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

            foreach (Switchable sw in _connectedSwitchables)
            {
                sw.SwitchOff();
            }

            StopAllCoroutines();
            StartCoroutine(Utility.FadeCoroutineUI(GetComponent<CanvasGroup>(), 1.0f, 0.0f, _disappearTimeSec, false));
        }
    }

    #endregion
}
