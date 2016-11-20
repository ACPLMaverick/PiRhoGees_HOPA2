using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextReadingPanel : MonoBehaviour {

    public Text Title;
    public Text Content;

    private Image _background;

    private Vector2 _upPosition;
    private Vector2 _downPosition;

    protected virtual void Awake()
    {
        _background = GetComponent<Image>();
    }

    // Use this for initialization
    void Start () {
        RectTransform rt = _background.GetComponent<RectTransform>();

        InputManager.Instance.OnInputMove.AddListener(Slide);

        _upPosition = rt.anchoredPosition;
        GetDownPosition(rt);
        _downPosition -= new Vector2(0, rt.rect.height);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show(string header, string context, string secondSideContext, bool twoSided)
    {
        if (!gameObject.activeSelf)
        {
            CameraManager.Instance.Enabled = false;

            gameObject.SetActive(true);

            GetComponent<RectTransform>().anchoredPosition = _upPosition;

            StartCoroutine(Utility.FadeCoroutineUI(_background.GetComponent<CanvasGroup>(), 0.0f, 1.0f, 0.5f, true));
        }
    }

    public void Hide()
    {
        if (gameObject.activeSelf)
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
}
