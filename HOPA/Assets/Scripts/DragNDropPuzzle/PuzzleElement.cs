using UnityEngine;
using System.Collections;

public class PuzzleElement : MonoBehaviour {

    #region Public

    public GameObject TargetSlot;
    public float MinDistance;
    public bool IsOnRightSlot;

    #endregion

    #region Protected

    private Vector3 _initPosition;
    private SpriteRenderer _mySpriteRenderer;
    private ParticleSystem _myParticle;
    private float _currentDistance;

    #endregion

    #region Functions

    // Use this for initialization
    void Start () {
        _initPosition = this.transform.position;
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _myParticle = GetComponentInChildren<ParticleSystem>();
        
        InputManager.Instance.OnInputClickDown.AddListener(OnPickUp);
        InputManager.Instance.OnInputClickUp.AddListener(OnPutDown);
        InputManager.Instance.OnInputMoveExclusive.AddListener(OnDrag);
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void ResetElement()
    {
        this.transform.position = _initPosition;
        _mySpriteRenderer.sortingOrder = 0;
        IsOnRightSlot = false;
    }

    public void CountDistanceToSlot()
    {
        _currentDistance = Vector3.Distance(this.transform.position, TargetSlot.transform.position);

        if(_currentDistance <= MinDistance)
        {
            _myParticle.Play();
            this.transform.position = TargetSlot.transform.position;
            PuzzleProgressManager.Instance.ElementsCompleted += 1;
            IsOnRightSlot = true;
        }
    }

    protected void OnPickUp(Vector2 screenPos, Collider2D hitCollider2D)
    {

    }

    protected void OnPutDown(Vector2 screenPos, Collider2D hitCollider2D)
    {
        if(!IsOnRightSlot)
        {
            _mySpriteRenderer.sortingOrder = 0;
            CountDistanceToSlot();
        }
    }

    protected void OnDrag(Vector2 currentScreenPos, Vector2 direction, Collider2D hitCollider2D)
    {
        if (hitCollider2D != null && hitCollider2D.gameObject == gameObject && !IsOnRightSlot)
        {
            _mySpriteRenderer.sortingOrder = 1;

            Vector3 wp1 = Camera.main.ScreenToWorldPoint(currentScreenPos);
            Vector3 wp2 = Camera.main.ScreenToWorldPoint(currentScreenPos + direction);
            Vector3 deltaP = wp2 - wp1;
            deltaP.z = 0.0f;

            GetComponent<Transform>().position += deltaP;
        }
    }

    #endregion
}
