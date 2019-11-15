﻿using System.Collections.Generic;
using System.Linq;
using Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

namespace UI {
    public class TankLifeUI : MonoBehaviour {

        [Header("Prefabs")] 
        public GameObject TankPanel;
        public GameObject TitleText;
        public GameObject LifeBarImage;

        private Dictionary<TankSetting, Transform> _tankSettingTransform = new Dictionary<TankSetting, Transform>();
        
        public void UpdateUI(List<GameObject> tanks) {
            foreach (TankEntity tankEntity in tanks.Select(o => o.GetComponent<TankEntity>())) {
                Transform tankPanelTransform;
                if (_tankSettingTransform.ContainsKey(tankEntity.TankSetting)) {
                    tankPanelTransform = _tankSettingTransform[tankEntity.TankSetting];
                }
                else {
                    tankPanelTransform = Instantiate(TankPanel, transform).transform;
                    _tankSettingTransform.Add(tankEntity.TankSetting, tankPanelTransform);
                    Instantiate(TitleText, tankPanelTransform).GetComponent<TextMeshProUGUI>().text = tankEntity.TankSetting.TankName;
                }
                GameObject lifeBarImage = Instantiate(LifeBarImage, tankPanelTransform);
                tankEntity.OnLifeChanged += delegate(float i) { lifeBarImage.GetComponent<Image>().fillAmount = i; };
            }
        }

    }
}
