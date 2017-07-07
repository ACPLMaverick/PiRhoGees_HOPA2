using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DifferencesProgressManager : MinigameProgressManager<DifferentObject> {

    #region Public

    public static DifferencesProgressManager Instance;
    public Text ProgressCounterText;

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

        ProgressCounterText.text = string.Format("{0}/7", ElementsCompleted);
	}

    public override void ResetGame()
    {
        base.ResetGame();

        foreach (DifferentObject puzzle in Elements)
        {
            puzzle.Reset();
        }
    }

    #endregion
}
