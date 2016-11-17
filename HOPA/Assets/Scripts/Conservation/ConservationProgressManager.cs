using UnityEngine;
using System.Collections;

public class ConservationProgressManager : MinigameProgressManager<Dust> {

    public static ConservationProgressManager Instance;

    public Dust DustyImage;

    [SerializeField]
    private float _targetWinCondition = 0.9f;

    private float _winCondition;

    // Use this for initialization
    public override void Start () {
        base.Start();

        Instance = this;
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}

    public override void CheckProgress()
    {
        _winCondition = (float)ElementsCompleted / (float)DustyImage.GetPixelsCount();
        if (_winCondition >= _targetWinCondition)
        {
            DustyImage.IsClear = true;
            WinGame();
        }
    }

    public override void ResetGame()
    {
        base.ResetGame();

        DustyImage.Reset();

    }
}
