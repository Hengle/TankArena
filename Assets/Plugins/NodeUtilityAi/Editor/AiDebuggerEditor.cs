﻿using System;
using System.Collections.Generic;
using System.Linq;
using NodeUtilityAi;
using NodeUtilityAi.Framework;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeUtilityAi.Editor {
    public class AiDebuggerEditor : EditorWindow {
        
        private GameObject _currentGameObject;
        private Dictionary<AbstractAIBrain, List<AIOption>> _options = new Dictionary<AbstractAIBrain, List<AIOption>>();
        private Dictionary<AbstractAIBrain, AIOption> _selectedOptions = new Dictionary<AbstractAIBrain, AIOption>();
        private Gradient _weightGradiant;
        
        [MenuItem("Tool/AI Debugger")]
        private static void Init() {
            AiDebuggerEditor window = GetWindow<AiDebuggerEditor>("AI Debugger", true);
            window.Show();
        }

        private void Awake() {
            _weightGradiant = new Gradient {
                colorKeys = new [] {
                    new GradientColorKey(Color.green, 1),
                    new GradientColorKey(Color.red, 0) 
                },
                alphaKeys = new [] {
                    new GradientAlphaKey(0.25f, 1),
                    new GradientAlphaKey(0.25f, 0)
                }
            };
        }

        private void Update() {
            // New selection control
            if (Selection.activeGameObject != _currentGameObject) {
                // Component control
                if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<AbstractAIComponent>() == null) return;
                // Update debug data
                _currentGameObject = Selection.activeGameObject;
                _options = Selection.activeGameObject.GetComponent<AbstractAIComponent>().Options;
                _selectedOptions = Selection.activeGameObject.GetComponent<AbstractAIComponent>().SelectedOptions;
            }
        }

        private void OnInspectorUpdate() {
            Repaint();
        }
        
        private void OnGUI() {
            GUIStyle labelGuiStyle = new GUIStyle(GUI.skin.label) {
                alignment = TextAnchor.MiddleCenter
            };
            if (_options == null) {
                EditorGUI.LabelField(new Rect(0, 0, position.width, position.height), "Please select a GameObject with an AbstractAIBrain derived Component", labelGuiStyle);
            } else if (_currentGameObject == null) {
                EditorGUI.LabelField(new Rect(0, 0, position.width, position.height), "It seems that the last selected GameObject is dead, please select another", labelGuiStyle);
            } else if (_options.Count == 0) {
                EditorGUI.LabelField(new Rect(0, 0, position.width, position.height), "No signal received from the Brain of " + _currentGameObject.name + ", is he asleep ?", labelGuiStyle);
            }
            else {
                // Display options
                int columnNumber = _options.Count;
                float columnWidth = (position.width - 12) / columnNumber;
                float rowHeight = 20;
                int i = 0;
                EditorGUI.LabelField(new Rect(3, 3, position.width - 6, rowHeight), "Brain of " + _currentGameObject.name, labelGuiStyle);
                foreach (KeyValuePair<AbstractAIBrain,List<AIOption>> valuePair in _options) {
                    float weightMax = valuePair.Value.Max(option => option.Weight);
                    float weightMin = valuePair.Value.Min(option => option.Weight);
                    EditorGUI.LabelField(new Rect(3 + i * (columnWidth + 6), 3 + rowHeight, columnWidth, rowHeight), valuePair.Key.name, labelGuiStyle);
                    for (int j = 0; j < valuePair.Value.Count; j++) {
                        EditorGUI.ProgressBar(new Rect(3 + i * (columnWidth + 6), 3 + (j + 2) * rowHeight, columnWidth, rowHeight), 
                            valuePair.Value[j].Probability, valuePair.Value[j].Description + " with Weight " + valuePair.Value[j].Weight + " and Utility " + valuePair.Value[j].Utility);
                        Color weightColor;
                        if (valuePair.Value[j] == _selectedOptions[valuePair.Key]) {
                            weightColor = new Color(0, 0, 0, 0.5f);
                        } else if (valuePair.Value[j].Weight == 0) {
                            weightColor = new Color(1, 1, 1, 0.5f);
                        } else if (Math.Abs(weightMax - weightMin) <= 0) {
                            weightColor = new Color(0, 1, 0, 0.25f);
                        } else {
                            float weightAbs = valuePair.Value[j].Weight * (weightMax - weightMin) / weightMax;
                            float colorTime = weightAbs / (weightMax - weightMin);
                            weightColor = _weightGradiant.Evaluate(colorTime);
                        }
                        EditorGUI.DrawRect(new Rect(3 + i * (columnWidth + 6), 3 + (j + 2) * rowHeight, columnWidth, rowHeight), weightColor);
                    }
                    i++;
                }
            }
        }

    }
}
