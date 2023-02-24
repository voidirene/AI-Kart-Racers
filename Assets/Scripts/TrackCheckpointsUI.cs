using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpointsUI : MonoBehaviour
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;

    private void Start()
    {
        trackCheckpoints.OnKartCorrectCheckpoint += TrackCheckpoints_OnKartCorrectCheckpoint;
        trackCheckpoints.OnKartWrongCheckpoint += TrackCheckpoints_OnKartWrongCheckpoint;

        Hide();
    }

    private void TrackCheckpoints_OnKartCorrectCheckpoint(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void TrackCheckpoints_OnKartWrongCheckpoint(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
