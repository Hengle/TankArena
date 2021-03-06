﻿using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Entities;
using SOReferences.GameObjectListReference;
using SOReferences.GameObjectReference;
using SOReferences.MatchReference;
using UnityEngine;
using Utils;

namespace Framework {
    public class TankSpawner : MonoBehaviour {
    
        [Header("Prefabs")] 
        public GameObject TankPrefab;
    
        [Header("Internal References")] 
        public List<Transform> TeamPositions;
        public Transform TankContent;
    
        [Header("SO References")] 
        public MatchReference CurrentMatchReference;
        public GameObjectListReference TanksReference;

        [Header("Parameters")] 
        public int LookAtSpeed;
        
        private Transform _targetLookAt;
        
        private void Start() {
            TanksReference.Value = new List<GameObject>();
            for (int i = 0; i < CurrentMatchReference.Value.Teams.Count; i++) {
                GenerateTankTeam(CurrentMatchReference.Value.Teams[i], TeamPositions[i]);
            }
        }

        private void GenerateTankTeam(Team team, Transform centerPosition) {
            // Generate
            List<GameObject> tanks = new List<GameObject>();
            foreach (TankSetting tankSetting in team.TankSettings) {
                GameObject instantiate = Instantiate(TankPrefab, centerPosition.position, Quaternion.identity, TankContent);
                instantiate.GetComponent<TankEntity>().Init(tankSetting, team);
                tanks.Add(instantiate);
            }
            // Setup position
            List<Vector3> skirmished = FormationUtils.Skirmished(tanks, centerPosition.transform.position, 2);
            for (int i = 0; i < tanks.Count; i++) {
                tanks[i].transform.position = centerPosition.position + skirmished[i];
            }
            TanksReference.Value.AddRange(tanks);
        }

    }
}