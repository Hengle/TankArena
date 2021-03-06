﻿using Entities;
using SOReferences.GameObjectListReference;
using UnityEngine;

namespace UI {
    public class CameraSwitchUI : MonoBehaviour {
    
        [Header("SO References")] 
        public GameObjectListReference TanksReference;
    
        private GameObject _tankCamera;
        private int _tankCameraIndex;

        public void NextTank() {
            _tankCameraIndex++;
            if (_tankCameraIndex >= TanksReference.Value.Count) _tankCameraIndex = 0;
            Switch(TanksReference.Value[_tankCameraIndex].GetComponent<TankEntity>().TurretCamera);
        }
        
        public void PreviousTank() {
            _tankCameraIndex++;
            if (_tankCameraIndex >= TanksReference.Value.Count) _tankCameraIndex = 0;
            Switch(TanksReference.Value[_tankCameraIndex].GetComponent<TankEntity>().TurretCamera);
        }

        public void MapCamera() {
            _tankCamera.SetActive(false);
        }
        
        private void Switch(GameObject newCamera) {
            if (_tankCamera)
                _tankCamera.SetActive(false);
            _tankCamera = newCamera;
            newCamera.SetActive(true);
        }
    
    }
}
