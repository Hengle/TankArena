using System.Collections.Generic;
using System.Linq;
using Entities;
using NodeUtilityAi;
using NodeUtilityAi.Nodes;
using UnityEngine;

namespace AI.CollectionEntryNodes {
    public class WaypointsNodes : CollectionEntryNode {

        protected override List<Object> CollectionProvider(AbstractAIComponent context) {
            TankAIComponent tankAiComponent = (TankAIComponent) context;
            return new List<Object>(tankAiComponent.TankEntity.SeekWaypointInRadius()
                .Select(entity => entity.gameObject));
        }
        
    }
}