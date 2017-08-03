using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DifferencesProgressManager : MinigameProgressManager<DifferentObject> {

    #region Constants

    private int TARGET_ELEMENTS = 7;
    private Vector3 POINTS_TEXT_SCALE_ENLARGED = new Vector3(2.0f, 2.0f, 2.0f);
    private Vector3 POINTS_TEXT_SCALE_NORMAL = Vector3.one;
    private float POINTS_TEXT_ENLARGEMENT_SPEED = 1.0f;

    #endregion

    #region Public

    public static DifferencesProgressManager Instance;
    public Text ProgressCounterText;

    #endregion

    #region Private

    private float _pointsEnlargementTimer = 1.0f;

    #endregion

    #region Functions

    // Use this for initialization
    public override void Start () {
        base.Start();

        Instance = this;
        Type = RoomType.MIRROR;
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();

        ProgressCounterText.text = string.Format("{0} / 7", ElementsCompleted);

        if(_pointsEnlargementTimer < 1.0f)
        {
            _pointsEnlargementTimer = Mathf.Clamp01(_pointsEnlargementTimer + Time.deltaTime * POINTS_TEXT_ENLARGEMENT_SPEED);
            float finalLerp = Mathf.Sin(_pointsEnlargementTimer * Mathf.PI * 0.5f);

            ProgressCounterText.rectTransform.localScale = Vector3.Lerp(
                POINTS_TEXT_SCALE_ENLARGED, POINTS_TEXT_SCALE_NORMAL, finalLerp);
        }
	}

    public override void ResetGame()
    {
        base.ResetGame();

        foreach (DifferentObject puzzle in Elements)
        {
            puzzle.Reset();
        }
    }

    public override void CheckProgress()
    {
        if (ElementsCompleted >= TARGET_ELEMENTS)
        {
            WinGame();
        }
    }

    public void AddElementCompleted(DifferentObject differentObj)
    {
        ElementsCompleted += 1;
        _pointsEnlargementTimer = 0.0f;
    }

    #endregion
}
