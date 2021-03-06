﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;
using SimpleJSON;

public class TextListViewUI : MonoBehaviour
{

    #region events

    #endregion

    #region public

    public TextAsset JSONFile;
    public GameObject ButtonSource;

    #endregion

    #region private

    private const float _canvasHeight = 1440.0f;

    private Image _background;

    private Vector2 _upPosition;
    private Vector2 _downPosition;

    private bool _isSliding;
    private float _buttonHeight;
    private int _textsCount;

    #endregion

    #region functions

    protected virtual void Awake()
    {
        _background = GetComponent<Image>();
        GameObject.Find("BackButton").GetComponent<Button>().onClick.AddListener(() => SceneChangeManager.Instance.ChangeScene(0));

        _buttonHeight = ButtonSource.GetComponent<RectTransform>().rect.height;

        //Load text count from some JSON or other shit
        //TextsCount = 1;
    }

    // Use this for initialization
    void Start ()
    {
        RectTransform rt = _background.GetComponent<RectTransform>();

        InputManager.Instance.OnInputMove.AddListener(Slide);

        _upPosition = rt.anchoredPosition;
        GetDownPosition(rt);
        _downPosition -= new Vector2(0, _canvasHeight);

        GenerateListView();
        //gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void Show(string header, string context, string secondSideContext, bool twoSided)
    {
        if(!gameObject.activeSelf)
        {
            CameraManager.Instance.Enabled = false;

            gameObject.SetActive(true);

            GetComponent<RectTransform>().anchoredPosition = _upPosition;

            StartCoroutine(Utility.FadeCoroutineUI(_background.GetComponent<CanvasGroup>(), 0.0f, 1.0f, 0.5f, true));
        }
    }

    public void Hide()
    {
        if(gameObject.activeSelf)
        {
            CameraManager.Instance.Enabled = true;

            StartCoroutine(Utility.FadeCoroutineUI(_background.GetComponent<CanvasGroup>(), 1.0f, 0.0f, 0.5f, false));
        }
    }

    private void Slide(Vector2 origin, Vector2 direction, Collider2D col)
    {
        if (gameObject.activeSelf)
        {
            RectTransform rt = GetComponent<RectTransform>();
            Vector2 newPosition = rt.anchoredPosition;
            newPosition += direction;
            newPosition.y = Mathf.Max(Mathf.Min(newPosition.y, _downPosition.y), _upPosition.y);

            newPosition.x = _upPosition.x;

            rt.anchoredPosition = newPosition;
        }
    }

    private IEnumerator SlideToPositionCoroutine(Vector2 position, float timeSec)
    {
        float currentTime = Time.time;
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 startPos = rt.anchoredPosition;

        while (Time.time - currentTime <= timeSec)
        {
            float lerp = (Time.time - currentTime) / timeSec;
            Vector2 pos = Vector2.Lerp(startPos, position, lerp);
            rt.anchoredPosition = pos;

            yield return null;
        }
        rt.anchoredPosition = position;

        yield return null;
    }

    private void GetDownPosition(RectTransform field)
    {
        _downPosition = _upPosition;
        _downPosition.y += field.rect.height; // + 3.5f * field.anchoredPosition.y;
    }

    private void GenerateListView()
    {
        string titleLanguage = "";
        string textLanguage = "";
        switch((LanguageManager.Language)PlayerPrefs.GetInt("PP_LANGUAGE", 0))
        {
            case LanguageManager.Language.Polish:
                titleLanguage = "title_pl";
                textLanguage = "textPath_pl";
                break;
            case LanguageManager.Language.English:
                titleLanguage = "title_en";
                textLanguage = "textPath_en";
                break;
        }

        var N = JSON.Parse(JSONFile.text);
        if(N["articles"][0][textLanguage].Value == "None")
        {
            ButtonSource.GetComponent<TextButton>().Title = N["articles"][0][titleLanguage].Value + string.Format("{0}(Coming soon)", System.Environment.NewLine);
            ButtonSource.GetComponent<TextButton>().DisableMyButton();
        }
        else
        {
            ButtonSource.GetComponent<TextButton>().Title = N["articles"][0][titleLanguage].Value;
            ButtonSource.GetComponent<TextButton>().TextPath = N["articles"][0][textLanguage].Value;
        }
        ButtonSource.GetComponent<TextButton>().UpdateTitle();
        _textsCount = N["articles"].Count;
        
        if(_textsCount < 6)
        {
            _buttonHeight = _canvasHeight / _textsCount;
            ButtonSource.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _buttonHeight);
            ButtonSource.GetComponent<TextButton>().MainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _buttonHeight);
            ButtonSource.GetComponent<TextButton>().MiniImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _buttonHeight);
        }

        float x = ButtonSource.GetComponent<RectTransform>().rect.width;

        for (int i = 1; i < _textsCount; ++i)
        {
            GameObject copy = (GameObject)Instantiate(ButtonSource);

            copy.transform.SetParent(this.transform, false);
            float y = _buttonHeight * -i;

            if(_buttonHeight + Mathf.Abs(y) > _background.rectTransform.rect.height)
            {
                _background.rectTransform.sizeDelta = new Vector2(_background.rectTransform.sizeDelta.x, _buttonHeight * _textsCount);
                ButtonSource.GetComponent<TextButton>().MainImage.rectTransform.sizeDelta = 
                    new Vector2(GetComponent<TextButton>().MainImage.rectTransform.sizeDelta.x, _buttonHeight * _textsCount);
                ButtonSource.GetComponent<TextButton>().MiniImage.rectTransform.sizeDelta = 
                    new Vector2(GetComponent<TextButton>().MiniImage.rectTransform.sizeDelta.x, _buttonHeight * _textsCount);

                GetDownPosition(_background.rectTransform);
                _downPosition -= new Vector2(0, _canvasHeight);
            }

            copy.GetComponent<RectTransform>().localPosition = new Vector2(x / 2, y);

            copy.GetComponent<TextButton>().Title = N["articles"][i][titleLanguage].Value;
            if (N["articles"][i][textLanguage].Value == "None")
            {
                copy.GetComponent<TextButton>().Title += string.Format("{0}(Coming soon)", System.Environment.NewLine);
                copy.GetComponent<TextButton>().DisableMyButton();
            }
            else
            {
                copy.GetComponent<TextButton>().EnableMyButton();
                copy.GetComponent<TextButton>().Title = N["articles"][i][titleLanguage].Value;
                copy.GetComponent<TextButton>().TextPath = N["articles"][i][textLanguage].Value;
            }
            copy.GetComponent<TextButton>().MainImage.sprite = Resources.Load<Sprite>(N["articles"][i]["bgImage"].Value);
            copy.GetComponent<TextButton>().MiniImage.sprite = Resources.Load<Sprite>(N["articles"][i]["miniImage"].Value);
        }
    }

    #endregion
}
