using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler<KartCheckpointEventArgs> OnKartCorrectCheckpoint;
    public event EventHandler<KartCheckpointEventArgs> OnKartWrongCheckpoint;

    private List<Transform> kartTransformList;
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

        kartTransformList = new List<Transform>();
        GameObject[] karts = GameObject.FindGameObjectsWithTag("AIKart");
        foreach (GameObject kart in karts)
        {
            kartTransformList.Add(kart.transform);
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

            KartCheckpointEventArgs args = new KartCheckpointEventArgs { kartTransform = kartTransform };
            OnKartCorrectCheckpoint?.Invoke(this, args);
        }
        else
        {
            KartCheckpointEventArgs args = new KartCheckpointEventArgs { kartTransform = kartTransform };
            OnKartWrongCheckpoint?.Invoke(this, args);

            Checkpoint correctCheckpoint = checkpointList[nextCheckpointIndex];
            correctCheckpoint.Show();
        }
    }
    public void ResetCheckpoint(Transform kartTransform)
    {
        nextCheckpointIndexList[kartTransformList.IndexOf(kartTransform)] = 0;
    }

    public GameObject GetNextCheckpoint(Transform kartTransform)
    {
        int nextCheckpointIndex = nextCheckpointIndexList[kartTransformList.IndexOf(kartTransform)];
        Checkpoint nextCheckpoint = checkpointList[nextCheckpointIndex];
        return nextCheckpoint.gameObject;
    }

    public class KartCheckpointEventArgs : EventArgs
    {
        public Transform kartTransform { get; set; }
    }
}
