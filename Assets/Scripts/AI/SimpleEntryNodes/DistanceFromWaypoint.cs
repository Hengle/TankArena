using System.Collections.Generic;
using System.Linq;
using Entities;
using NodeUtilityAi;
using NodeUtilityAi.Nodes;
using UnityEngine;

namespace AI.SimpleEntryNodes {
    public class DistanceFromWaypoint : SimpleEntryNode {
        
        public DistanceSorter DistanceSorter = DistanceSorter.Nearest;
        
        protected override int ValueProvider(AbstractAIComponent context) {
            TankAIComponent tankAiComponent = (TankAIComponent) context;
            List<GameObject> waypointGameObjects = tankAiComponent.TankEntity.SeekWaypointInRadius();
            float distance = 0;
            if (DistanceSorter == DistanceSorter.Nearest) {
                distance = Mathf.Infinity;
                if (waypointGameObjects.Count == 0) return (int) distance;
                distance = waypointGameObjects
                    .Select(waypoint => Vector3.Distance(waypoint.transform.position, tankAiComponent.transform.position))
                    .Concat(new[] {distance})
                    .Min();
            }
            if (DistanceSorter == DistanceSorter.Farthest) {
                distance = Mathf.NegativeInfinity;
                if (waypointGameObjects.Count == 0) return (int) distance;
                distance = waypointGameObjects
                    .Select(waypoint => Vector3.Distance(waypoint.transform.position, tankAiComponent.transform.position))
                    .Concat(new[] {distance})
                    .Max();
            }
            return (int) distance;
        }
        
    }
}