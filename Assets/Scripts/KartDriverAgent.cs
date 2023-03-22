using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class KartDriverAgent : Agent
{
    [SerializeField] private Transform startingTransform;
    [SerializeField] private Transform targetTransform;

    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private KartMovement movement;

    private void Start()
    {
        trackCheckpoints.OnKartCorrectCheckpoint += TrackCheckpoints_OnKartCorrectCheckpoint;
        trackCheckpoints.OnKartWrongCheckpoint += TrackCheckpoints_OnKartWrongCheckpoint;
    }

    private void TrackCheckpoints_OnKartCorrectCheckpoint(object sender, TrackCheckpoints.KartCheckpointEventArgs e)
    {
        if (e.kartTransform == transform)
        {
            AddReward(+1f);
        }
    }
    private void TrackCheckpoints_OnKartWrongCheckpoint(object sender, TrackCheckpoints.KartCheckpointEventArgs e)
    {
        if (e.kartTransform == transform)
        {
            AddReward(-1f);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 nextCheckpointForward = trackCheckpoints.GetNextCheckpoint(transform).transform.forward;
        float directionDot = Vector3.Dot(transform.forward, nextCheckpointForward);
        sensor.AddObservation(directionDot);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float accelerate = 0f;
        float steer = 0f;
        float shouldBreak = 0f;

        switch (actions.DiscreteActions[0])
        {
            case 0: accelerate = 0f; break;
            case 1: accelerate = 1f; break;
        }
        switch (actions.DiscreteActions[1])
        {
            case 0: steer = 0f; break;
            case 1: steer = 1f; break;
            case 2: steer = -1f; break;
        }
        switch (actions.DiscreteActions[2])
        {
            case 0: shouldBreak = 0f; break;
            case 1: shouldBreak = 1f; break;
        }
        print(steer);
        movement.SetInputs(accelerate, steer, shouldBreak);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+10f);
            EndEpisode();
        }
        if (other.TryGetComponent<Checkpoint>(out Checkpoint checkpoint))
        {
            SetReward(+1f);
        }
        if (other.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-0.5f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-0.1f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Grass>(out Grass grass))
        {
            SetReward(-5f);
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startingTransform.position;
        trackCheckpoints.ResetCheckpoint(transform);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        discreteActions[0] = (int)Input.GetAxisRaw("Accelerating");

        int verticalInputs = (int)Input.GetAxisRaw("Steering");
        if (verticalInputs == 0)
        {
            discreteActions[1] = 0;
        }
        else if (verticalInputs == 1)
        {
            discreteActions[1] = 1;
        }
        else if (verticalInputs == -1)
        {
            discreteActions[1] = 2;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            discreteActions[2] = 1;
        }
        else
        {
            discreteActions[2] = 0;
        }
    }
}
