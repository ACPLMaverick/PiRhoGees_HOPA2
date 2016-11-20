using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;

public class TextListViewUI : MonoBehaviour
{

    #region events

    #endregion

    #region public

    public int TextsCount;
    public GameObject ButtonSource;

    #endregion

    #region private

    private Image _background;

    private Vector2 _upPosition;
    private Vector2 _downPosition;

    private const float _canvasHeight = 1920.0f;

    #endregion

    #region functions

    protected virtual void Awake()
    {
        _background = GetComponent<Image>();
        GameObject.Find("BackButton").GetComponent<Button>().onClick.AddListener(() => SceneChangeManager.Instance.ChangeScene(0));

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
            print(newPosition.y);
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
        float x = ButtonSource.GetComponent<RectTransform>().rect.width;

        for (int i = 1; i < TextsCount; ++i)
        {
            GameObject copy = (GameObject)Instantiate(ButtonSource);

            copy.transform.SetParent(this.transform, false);
            float y = 150 * -i;

            if(150 + Mathf.Abs(y) > _background.rectTransform.rect.height)
            {
                _background.rectTransform.sizeDelta = new Vector2(_background.rectTransform.sizeDelta.x, 150f * TextsCount);
                GetDownPosition(_background.rectTransform);
                _downPosition -= new Vector2(0, _canvasHeight);
            }

            copy.GetComponent<RectTransform>().localPosition = new Vector2(x / 2, y);
        }
    }

    #endregion
}
