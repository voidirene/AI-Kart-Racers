using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler OnKartCorrectCheckpoint;
    public event EventHandler OnKartWrongCheckpoint;

    [SerializeField] private List<Transform> kartTransformList;
    private List<Checkpoint> checkpointList;
    private List<int> nextCheckpointIndexList;

    private void Awake()
    {
        Transform checkpointsTransform = transform;

        checkpointList = new List<Checkpoint>();
        foreach (Transform checkpointTransform in checkpointsTransform) 
        {
            Checkpoint checkpoint = checkpointTransform.GetComponent<Checkpoint>();

            checkpoint.SetTrackCheckpoints(this);

            checkpointList.Add(checkpoint);
        }

        nextCheckpointIndexList = new List<int>();
        foreach (Transform kartTransform in kartTransformList)
        {
            nextCheckpointIndexList.Add(0);
        }
    }

    public void KartThroughCheckpoint(Checkpoint checkpoint, Transform kartTransform)
    {
        int nextCheckpointIndex = nextCheckpointIndexList[kartTransformList.IndexOf(kartTransform)];

        if (checkpointList.IndexOf(checkpoint) == nextCheckpointIndex)
        {
            nextCheckpointIndexList[kartTransformList.IndexOf(kartTransform)] = (nextCheckpointIndex + 1) % checkpointList.Count; // the reason we're doing this instead of 'nextCheckpointIndex++' is to allow for running multiple laps

            Checkpoint correctCheckpoint = checkpointList[nextCheckpointIndex];
            correctCheckpoint.Hide();

            OnKartCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnKartWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            Checkpoint correctCheckpoint = checkpointList[nextCheckpointIndex];
            correctCheckpoint.Show();
        }
    }
}
