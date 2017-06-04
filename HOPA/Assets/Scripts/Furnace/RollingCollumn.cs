using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class RollingCollumn : MonoBehaviour
{
    #region Fields

    #endregion

    #region Properties

    public bool CorrectAndLocked
    {
        get
        {
            return _locked;
        }
    }

    #endregion

    #region Public Events

    public class UnityEventRollingCollumnCorrect : UnityEvent<RollingCollumn> { }
    public UnityEventRollingCollumnCorrect EventCorrect = new UnityEventRollingCollumnCorrect();

    #endregion

    #region Protected

    RollingPart[] _parts;
    bool _locked = false;
    ParticleSystem _winParticle;
    DisappearableObject _dp;

    #endregion

    #region MonoBehaviours

    // Use this for initialization
    void Start ()
    {
        _parts = GetComponentsInChildren<RollingPart>();
        for(int i = 0; i < _parts.Length; ++i)
        {
            _parts[i].EventChanged.AddListener(new UnityAction<RollingPart>(OnRollingPartChanged));
        }
        _winParticle = GetComponentInChildren<ParticleSystem>();

        // pre-check
        OnRollingPartChanged(null);

        _dp = GetComponentInChildren<DisappearableObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    #endregion

    #region Functions

    protected void OnRollingPartChanged(RollingPart part)
    {
        // check all parts at once

        for (int i = 0; i < _parts.Length; ++i)
        {
            if (_parts[i].CurrentIndex != _parts[i].CorrectIndex)
            {
                return;
            }
        }

        // all parts are correct! Lock Collumn.
        for (int i = 0; i < _parts.Length; ++i)
        {
            _parts[i].EventChanged.RemoveAllListeners();        // !!!
            _parts[i].enabled = false;
        }
        if(_winParticle != null)
        {
            _winParticle.Stop();
            _winParticle.Play();
        }
        _locked = true;
        EventCorrect.Invoke(this);

        if (_dp != null)
            _dp.Activate();
    }
    #endregion
}
