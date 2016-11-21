using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ViewerFullscreenImage : MonoBehaviour
{
    #region Fields

    #endregion

    #region Properties

    #endregion

    #region Protected

    protected CanvasGroup _cGrp;
    protected Image _image;

    protected const float _MAX_TIME_BETWEEN_DOUBLE_TOUCH = 0.7f;
    protected const float _MAX_ZOOM = 3.0f;
    protected const float _MIN_ZOOM = 1.0f;

    protected Vector3 _imageBaseScale;
    protected Vector2 _imageBaseSizeDelta;
    protected Vector2 _imageBasePosition;
    protected Vector2 _moveBoundMin;
    protected Vector2 _moveBoundMax;
    protected Vector2 _lastTouchPositon = Vector2.zero;
    protected Vector2 _lastTouchPositon2 = Vector2.zero;
    protected float _lastPressTime = 0.0f;
    protected float _diffPinchHelper = 0.0f;
    protected float _zoomHelper = 1.0f;

    #endregion

    #region MonoBehaviours

    void Awake()
    {
        _cGrp = GetComponent<CanvasGroup>();
        _image = GetComponentsInChildren<Image>()[GetComponentsInChildren<Image>().Length - 2];
        _imageBasePosition = _image.rectTransform.anchoredPosition;
        _imageBaseScale = _image.rectTransform.localScale;
        _imageBaseSizeDelta = new Vector2(_image.rectTransform.rect.width, _image.rectTransform.rect.height);

        RecalculateImageBounds();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateControls();
    }

    #endregion

    #region Functions Public

    public void Show()
    {
        _zoomHelper = 1.0f;
        //_lastPressTime = 0.0f;
        _lastTouchPositon = Vector2.zero;
        _lastTouchPositon2 = Vector2.zero;

        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(Utility.FadeCoroutineUI(_cGrp, 0.0f, 1.0f, 0.5f, true));
    }

    public void Show(Sprite sprite)
    {
        if(_image.sprite != sprite)
        {
            ChangeSpriteImmediately(sprite);
        }
        Show();
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(Utility.FadeCoroutineUI(_cGrp, 1.0f, 0.0f, 0.5f, false));
    }

    #endregion

    #region Functions Protected

    protected void UpdateControls()
    {
        _lastPressTime += Time.deltaTime;

        Vector2 currentPosition = Vector2.zero;
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
                currentPosition = Input.GetTouch(0).position;
        }
        else
        {
            currentPosition = Input.mousePosition;
        }

        // exit - tap twice
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetMouseButtonDown(0)))
        {
            if(_lastPressTime <= _MAX_TIME_BETWEEN_DOUBLE_TOUCH)
            {
                OnTapTwice();
            }
            _lastPressTime = 0.0f;
        }

        // move photo - move
        if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || (Input.GetMouseButton(1) && Input.mousePosition != new Vector3(_lastTouchPositon.x, _lastTouchPositon.y, Input.mousePosition.z)))
        {
            OnMove(currentPosition - _lastTouchPositon);
        }

        // zoom - pinch
        if(Application.isMobilePlatform)
        {
            if (Input.touchCount == 2)
            {
                Touch cTouch1 = Input.GetTouch(0);
                Touch cTouch2 = Input.GetTouch(1);

                if (cTouch1.phase == TouchPhase.Moved && cTouch2.phase == TouchPhase.Moved)
                {
                    Vector2 pos1 = cTouch1.position;
                    Vector2 pos2 = cTouch2.position;
                    Vector2 delta1 = pos1 - _lastTouchPositon;
                    Vector2 delta2 = pos2 - _lastTouchPositon2;

                    float dot = Vector2.Dot(delta1, delta2);

                    if (dot < 0.7f)
                    {
                        float fl = delta1.magnitude;
                        float sl = delta2.magnitude;
                        float mp = -0.05f;
                        float diff = (pos1 - pos2).magnitude;
                        if (diff - _diffPinchHelper < 0)
                        {
                            mp *= -1.0f;
                        }
                        _diffPinchHelper = diff;
                        float amount = (fl + sl) * mp;

                        OnZoom(amount);
                    }
                }
                _lastTouchPositon2 = cTouch2.position;
            }
        }
        else
        {
            if(Input.mouseScrollDelta.y != 0.0f)
            {
                OnZoom(Input.mouseScrollDelta.y);
            }
        }

        _lastTouchPositon = currentPosition;
    }


    protected void OnTapTwice()
    {
        Hide();
    }

    protected void OnMove(Vector2 delta)
    {
        RectTransform rt = _image.rectTransform;
        Vector2 oldPosition = rt.position;
        rt.position += new Vector3(delta.x, delta.y, 0.0f);

        FixImagePosition();
    }

    protected void OnZoom(float val)
    {
        _zoomHelper = Mathf.Clamp(_zoomHelper + val, _MIN_ZOOM, _MAX_ZOOM);
        _image.rectTransform.localScale = new Vector3(_zoomHelper * _imageBaseScale.x, _zoomHelper * _imageBaseScale.y, _imageBaseScale.z);

        FixImagePosition();
        RecalculateImageBounds();
    }

    protected void FixImagePosition()
    {
        RectTransform rt = _image.rectTransform;
        // fix move boundaries (picture can't have any of its corners between _moveboundmin and max
        // bl, tl, tr, br
        Vector3[] points = new Vector3[4];
        rt.GetWorldCorners(points);

        if (
            (points[0].x > _moveBoundMin.x) ||
            (points[1].y < _moveBoundMax.y) ||
            (points[2].x < _moveBoundMax.x) ||
            (points[3].y > _moveBoundMin.y)
            )
        {
            Vector2 adjustment = new Vector2
                (
                    -(Mathf.Max(points[0].x - _moveBoundMin.x, 0.0f)) + Mathf.Max(_moveBoundMax.x - points[2].x, 0.0f),
                    -(Mathf.Max(points[3].y - _moveBoundMin.y, 0.0f)) + Mathf.Max(_moveBoundMax.y - points[1].y, 0.0f)
                );
            rt.position += new Vector3(adjustment.x, adjustment.y, 0.0f);
        }
    }

    protected void ChangeSprite(Sprite newSprite)
    {
        ChangeSpriteImmediately(newSprite);
    }

    protected void ChangeSpriteImmediately(Sprite newSprite)
    {
        _image.rectTransform.localScale = _imageBaseScale;
        _image.rectTransform.anchoredPosition = _imageBasePosition;

        RectTransform rt = _image.rectTransform;
        Vector2 newSizeDelta = new Vector2(newSprite.texture.width, newSprite.texture.height);
        float ratio = newSizeDelta.x / newSizeDelta.y;
        float baseRatio = _imageBaseSizeDelta.x / _imageBaseSizeDelta.y;

        _image.sprite = newSprite;
        _image.SetNativeSize();

        rt.sizeDelta /= (rt.sizeDelta.y / _imageBaseSizeDelta.y);

        RecalculateImageBounds();
    }

    protected void RecalculateImageBounds()
    {
        Vector2 parentBoundMin = _image.rectTransform.parent.GetComponent<RectTransform>().TransformPoint(_image.rectTransform.parent.GetComponent<RectTransform>().rect.min);
        Vector2 parentBoundMax = _image.rectTransform.parent.GetComponent<RectTransform>().TransformPoint(_image.rectTransform.parent.GetComponent<RectTransform>().rect.max);
        Vector2 imageBoundMin = _image.rectTransform.TransformPoint(_image.rectTransform.rect.min);
        Vector2 imageBoundMax = _image.rectTransform.TransformPoint(_image.rectTransform.rect.max);

        _moveBoundMin.x = Mathf.Max(parentBoundMin.x, imageBoundMin.x);
        _moveBoundMin.y = Mathf.Max(parentBoundMin.y, imageBoundMin.y);
        _moveBoundMax.x = Mathf.Min(parentBoundMax.x, imageBoundMax.x);
        _moveBoundMax.y = Mathf.Min(parentBoundMax.y, imageBoundMax.y);
    }

    #endregion
}
