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

    #endregion

    #region Protected

    protected const int MAX_INDEX = 3;
    protected const string STAGE_NAME = "Stage";
    protected SpriteRenderer _sr;
    protected Animator _animator;
    protected int _currentSpriteIndex = 0;

    protected string[] _stateNames =
        {
            ("RollingPart01"),
            ("RollingPart02"),
            ("RollingPart03"),
            ("RollingPart04")
        };

    #endregion

    #region Events Public

    public class UnityEventRollingPartChanged : UnityEvent<RollingPart> { }

    public UnityEvent<RollingPart> EventChanged = new UnityEventRollingPartChanged();

    #endregion

    #region MonoBehaviours

    // Use this for initialization
    void Start ()
    {
        if(_beginIndex == -1 || _beginIndex == _correctIndex)
        {
            RandomizeBeginIndex();
        }
        else
        {
            _beginIndex = Mathf.Min(_beginIndex, MAX_INDEX);
        }

        _currentSpriteIndex = _beginIndex;
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _animator.Play(_stateNames[_currentSpriteIndex]);
        _animator.SetInteger(STAGE_NAME, _currentSpriteIndex);

        //InputManager.Instance.OnInputSwipe.AddListener(new UnityEngine.Events.UnityAction<Vector2, InputManager.SwipeDirection, float, Collider2D>(OnSwipe));
        InputManager.Instance.OnInputClickUp.AddListener(new UnityEngine.Events.UnityAction<Vector2, Collider2D>(OnClickUp));
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    #endregion

    #region Functions Public

    public void RandomizeBeginIndex()
    {
        do
        {
            _beginIndex = Random.Range(0, MAX_INDEX + 1);
        } while (_beginIndex == _correctIndex);
    }

    #endregion

    #region Functions Protected

    protected void OnClickUp(Vector2 pos, Collider2D col)
    {
        // always roll collumn to the right (i.e. rotate CW)
        if(enabled && col == GetComponent<Collider2D>())
        {
            _currentSpriteIndex = (_currentSpriteIndex + 1) % (MAX_INDEX + 1);
            _animator.SetInteger(STAGE_NAME, _currentSpriteIndex);
            EventChanged.Invoke(this);
        }
    }

    protected void OnSwipe(Vector2 pos, InputManager.SwipeDirection dir, float diffLength, Collider2D col)
    {
        int indexAddition = 1;
        if(dir == InputManager.SwipeDirection.LEFT)
        {
            indexAddition = -1;
        }

        _currentSpriteIndex = (_currentSpriteIndex + indexAddition) % (MAX_INDEX + 1);
        if(_currentSpriteIndex < 0)
        {
            _currentSpriteIndex = MAX_INDEX + _currentSpriteIndex;
        }
        _animator.SetInteger(STAGE_NAME, _currentSpriteIndex);
        EventChanged.Invoke(this);
    }

    #endregion
}
