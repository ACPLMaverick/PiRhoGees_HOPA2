using UnityEngine;
using System.Collections;

public class ConservationProgressManager : MinigameProgressManager<Dust> {

    public static ConservationProgressManager Instance;

    public Dust DustyImage;
    public Hole[] Holes;

    public bool ClearingFinished;
    public bool HolesFixingFinished;
    public int HolesFixedCount;

    [SerializeField]
    private float _targetDustClearance = 0.9f;
    private float _dustClearanceCondition;

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
        if (ClearingFinished && HolesFixingFinished)
        {
            WinGame();
        }

        //DUST CLEARING CHECK
        _dustClearanceCondition = (float)ElementsCompleted / (float)DustyImage.GetPixelsCount();
        if (!ClearingFinished && _dustClearanceCondition >= _targetDustClearance)
        {
            DustyImage.IsClear = true;
            DustyImage.DustDisappear();
            ClearingFinished = true;
            //WinGame();
        }

        //HOLE FIXING CHECK
        if(!HolesFixingFinished && HolesFixedCount == Holes.Length)
        {
            HolesFixingFinished = true;
        }
    }

    public override void ResetGame()
    {
        base.ResetGame();

        DustyImage.Reset();
        ClearingFinished = false;

        HolesFixedCount = 0;
        foreach(Hole h in Holes)
        {
            h.Reset();
        }
        HolesFixingFinished = false;
    }
}
