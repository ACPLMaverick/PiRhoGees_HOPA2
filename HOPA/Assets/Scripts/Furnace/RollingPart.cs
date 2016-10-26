using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RollingPart : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected int _firstElement = 0;

    [SerializeField]
    protected List<Sprite> _sprites;

    [SerializeField]
    protected bool _canWrap = true;

    #endregion

    #region Properties

    public int CurrentSpriteIndex
    {
        get
        {
            return _currentSpriteIndex;
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

    #region MonoBehaviours

    // Use this for initialization
    void Start ()
    {
        _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = _sprites[_firstElement];
        InputManager.Instance.OnInputSwipe.AddListener(new UnityEngine.Events.UnityAction<Vector2, InputManager.SwipeDirection, float, Collider2D>(OnSwipe));
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    #endregion

    #region Functions Public



    #endregion

    #region Functions Protected

    protected void OnSwipe(Vector2 pos, InputManager.SwipeDirection dir, float diffLength, Collider2D col)
    {
        if(((_currentSpriteIndex == 0 && dir == InputManager.SwipeDirection.LEFT) || 
            (_currentSpriteIndex == _sprites.Count - 1) && dir == InputManager.SwipeDirection.RIGHT) && !_canWrap)
        {
            return;
        }

        int indexAddition = 1;
        if(dir == InputManager.SwipeDirection.LEFT)
        {
            indexAddition = -1;
        }

        _currentSpriteIndex = (_currentSpriteIndex + indexAddition) % _sprites.Count;
        SetSprite(_sprites[_currentSpriteIndex]);
    }

    protected void SetSprite(Sprite nextSpr)
    {
        _sr.sprite = nextSpr;
    }

    #endregion
}
