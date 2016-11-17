using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class RollingPart : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected int _beginIndex = 0;

    [SerializeField]
    protected int _correctIndex = 1;

    [SerializeField]
    protected List<Sprite> _sprites;

    [SerializeField]
    protected bool _canWrap = true;

    #endregion

    #region Properties

    public int CurrentIndex
    {
        get
        {
            return _currentSpriteIndex;
        }
    }

    public int CorrectIndex
    {
        get
        {
            return _correctIndex;
        }
    }

    public Sprite CurrentSprite
    {
        get
        {
            return _sprites[_currentSpriteIndex];
        }
    }

    #endregion

    #region Protected

    protected SpriteRenderer _sr;
    protected int _currentSpriteIndex = 0;

    #endregion

    #region Events Public

    public class UnityEventRollingPartChanged : UnityEvent<RollingPart> { }

    public UnityEvent<RollingPart> EventChanged = new UnityEventRollingPartChanged();

    #endregion

    #region MonoBehaviours

    // Use this for initialization
    void Start ()
    {
        _currentSpriteIndex = _beginIndex;
        _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = _sprites[_beginIndex];
        //InputManager.Instance.OnInputSwipe.AddListener(new UnityEngine.Events.UnityAction<Vector2, InputManager.SwipeDirection, float, Collider2D>(OnSwipe));
        InputManager.Instance.OnInputClickUp.AddListener(new UnityEngine.Events.UnityAction<Vector2, Collider2D>(OnClickUp));
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    #endregion

    #region Functions Public



    #endregion

    #region Functions Protected

    protected void OnClickUp(Vector2 pos, Collider2D col)
    {
        // always roll collumn to the right (i.e. rotate CW)
        if(enabled && col == GetComponent<Collider2D>())
        {
            _currentSpriteIndex = (_currentSpriteIndex + 1) % _sprites.Count;
            SetSprite(_sprites[_currentSpriteIndex]);
            EventChanged.Invoke(this);
        }
    }

    protected void OnSwipe(Vector2 pos, InputManager.SwipeDirection dir, float diffLength, Collider2D col)
    {
        if((((_currentSpriteIndex == 0 && dir == InputManager.SwipeDirection.LEFT) || 
            (_currentSpriteIndex == _sprites.Count - 1) && dir == InputManager.SwipeDirection.RIGHT) && !_canWrap) ||
            !enabled ||
            col != GetComponent<Collider2D>())
        {
            return;
        }

        int indexAddition = 1;
        if(dir == InputManager.SwipeDirection.LEFT)
        {
            indexAddition = -1;
        }

        _currentSpriteIndex = (_currentSpriteIndex + indexAddition) % _sprites.Count;
        if(_currentSpriteIndex < 0)
        {
            _currentSpriteIndex = _sprites.Count + _currentSpriteIndex;
        }
        SetSprite(_sprites[_currentSpriteIndex]);
        EventChanged.Invoke(this);
    }

    protected void SetSprite(Sprite nextSpr)
    {
        _sr.sprite = nextSpr;
    }

    #endregion
}
