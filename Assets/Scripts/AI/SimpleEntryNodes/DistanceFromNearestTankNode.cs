using System.Collections.Generic;
using System.Linq;
using Entities;
using NodeUtilityAi;
using NodeUtilityAi.Nodes;
using UnityEngine;

namespace AI.SimpleEntryNodes {
    public class DistanceFromNearestTankNode : SimpleEntryNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            TankAIComponent tankAiComponent = (TankAIComponent) context;
            List<TankEntity> tankEntities = GameManager.Instance.TankEntities;
            tankEntities.Remove(tankAiComponent.TankEntity);
            float distanceFromNearestTank = Mathf.Infinity;
            if (tankEntities.Count == 0) return (int) distanceFromNearestTank;
            distanceFromNearestTank = tankEntities
                .Select(tankEntity => Vector3.Distance(tankEntity.transform.position, tankAiComponent.transform.position))
                .Concat(new[] {distanceFromNearestTank})
                .Min();
            return (int) distanceFromNearestTank;
        }
        
    }
}