using Unity.MLAgents.Actuators;
using Unity.MLAgents.Integrations.Match3;
using UnityEngine;

public class Mactch3ActuatorPresenter : Match3ActuatorComponent
{
    public override IActuator[] CreateActuators()
    {
        var board = GetComponent<Match3Board>();
        var seed = RandomSeed == -1 ? gameObject.GetInstanceID() : RandomSeed;
        return new IActuator[] { new Match3ExampleActuator(board, ForceHeuristic, seed, ActuatorName) };

    }
}
