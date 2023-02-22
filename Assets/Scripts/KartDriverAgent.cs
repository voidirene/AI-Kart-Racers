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

    [SerializeField] private KartMovement movement;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.DiscreteActions[0];
        float moveZ = actions.DiscreteActions[1];

        Debug.Log("Action 0: " + actions.DiscreteActions[0]);
        Debug.Log("Action 1: " + actions.DiscreteActions[1]);

        float moveSpeed = 5f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+1f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        //reset position to a random point near the starting area
        int xPos = (int)Random.Range(startingTransform.localPosition.x - 2.5f, startingTransform.localPosition.x + 2.5f);
        int zPos = (int)Random.Range(startingTransform.localPosition.z - 2.5f, startingTransform.localPosition.z + 2.5f);
        transform.localPosition = new Vector3(xPos, 1, zPos);

        // targetTransform.localPosition = new Vector3(Random.Range(-7, 7), Random.Range(-3, 3), 0);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = (int) Input.GetAxisRaw("Horizontal");
        discreteActions[1] = (int) Input.GetAxisRaw("Vertical");
    }
}
