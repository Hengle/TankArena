using NodeUtilityAi;
using NodeUtilityAi.Framework;
using NodeUtilityAi.Nodes;
using UnityEngine;

namespace AI.SimpleActionNodes {
    public class SetTarget : DataActionNode {

        public override void Execute(AbstractAIComponent context, AIData aiData) {
            TankAIComponent tankAiComponent = (TankAIComponent) context;
            tankAiComponent.TankEntity.Target = aiData.GetData<GameObject>();
        }
        
    }

}