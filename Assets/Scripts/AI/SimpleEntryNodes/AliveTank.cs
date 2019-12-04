using NodeUtilityAi;
using NodeUtilityAi.Nodes;

namespace AI.SimpleEntryNodes {
    public class AliveTank : SimpleEntryNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            TankAIComponent tankAiComponent = (TankAIComponent) context;
            return tankAiComponent.TankEntity.TanksReference.Value.Count;
        }
        
    }
}