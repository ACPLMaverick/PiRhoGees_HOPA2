using UnityEngine;
using System.Collections;

public class DifferencesProgressManager : MinigameProgressManager<DifferentObject> {

    #region Public

    public static DifferencesProgressManager Instance;

    #endregion

    #region Functions

    // Use this for initialization
    void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
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
