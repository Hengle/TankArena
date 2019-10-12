﻿using System;
using Components;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Entities {
    public class TankEntity : MonoBehaviour {
        
        [Header("Data")]
        public PlayerSettings PlayerSettings;
        
        [Header("References")] 
        public Transform CanonOut;
        public Transform Turret;
        public AudioSource ShotFiring;
        public ParticleEmissionSetter SmokeSetter;
        public ParticleEmissionSetter FireSetter;
        public TextMeshProUGUI TankName;
        public MeshRenderer TurretMeshRenderer;
        public MeshRenderer HullMeshRenderer;
        public MeshRenderer RightTrackMeshRender;
        public MeshRenderer LeftTrackMeshRender;
        
        [Header("Prefabs")]
        public GameObject ShellPrefab;
        public GameObject CanonShotPrefab;
        public GameObject TankExplosionPrefab;

        [Header("Parameters")] 
        public int CanonDamage;
        public int CanonPower;
        public int TurretSpeed;
        public int MaxHP;
        public int ReloadTime;
        
        [Header("Variables")]
        public int CurrentHP;
        public bool IsShellLoaded = true;
        public GameObject Target;
        public GameObject Destination;
        
        private NavMeshAgent _navMeshAgent;

        private int _totalDamages => MaxHP - CurrentHP;
        private float _damagePercent => (float) _totalDamages / MaxHP;
        private bool _isAtDestination => _navMeshAgent.remainingDistance < Mathf.Infinity &&
                                          _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
                                          _navMeshAgent.remainingDistance <= 0;

        private void Awake() {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            TankName.text = PlayerSettings.TankName;
            TurretMeshRenderer.material.color = PlayerSettings.TurretColor;
            HullMeshRenderer.material.color = PlayerSettings.HullColor;
            RightTrackMeshRender.material.color = PlayerSettings.TracksColor;
            LeftTrackMeshRender.material.color = PlayerSettings.TracksColor;
            CurrentHP = MaxHP;
        }

        private void Update() {
            Debug.DrawRay(CanonOut.position, CanonOut.forward * 100, Color.red);
            if (Target) {
                Vector3 newDir = Vector3.RotateTowards(Turret.forward, Target.transform.position - Turret.position, TurretSpeed * Time.deltaTime, 0.0f);
                Turret.rotation = Quaternion.LookRotation(newDir);
                Turret.eulerAngles = new Vector3(0, Turret.eulerAngles.y, 0);
            }
            if (Destination) {
                Vector3[] pathCorners = _navMeshAgent.path.corners;
                if (pathCorners.Length > 0) {
                    Debug.DrawLine(transform.position, pathCorners[0], Color.green);
                    for (int i = 1; i < pathCorners.Length - 1; i++) {
                        Debug.DrawLine(pathCorners[i - 1], pathCorners[i], Color.green);
                    }
                }
                _navMeshAgent.SetDestination(Destination.transform.position);
                if (_isAtDestination) Destination = null;
            }
            SmokeSetter.SetEmissionPercent(_damagePercent);
            FireSetter.SetEmissionPercent(_damagePercent < 0.5 ? 0 : _damagePercent * 2);
        }

        public void Fire() {
            if (!IsShellLoaded) return;
            Instantiate(CanonShotPrefab, CanonOut.position, CanonShotPrefab.transform.rotation);
            ShotFiring.Play();
            GameObject instantiate = Instantiate(ShellPrefab, CanonOut.position, CanonOut.rotation);
            instantiate.GetComponent<Rigidbody>().AddForce(CanonOut.transform.forward * CanonPower, ForceMode.Impulse);
            instantiate.GetComponent<ShellEntity>().TankEntityOwner = this;
            instantiate.GetComponent<ShellEntity>().Damage = CanonDamage;
            IsShellLoaded = false;
            Invoke(nameof(Reload), ReloadTime);
        }

        public void Damage(int damage) {
            CurrentHP -= damage;
            if (CurrentHP > 0) return;
            Instantiate(TankExplosionPrefab, transform.position, TankExplosionPrefab.transform.rotation);
            Destroy(gameObject);
        }

        public void MoveTo(Vector3 position) {
            _navMeshAgent.SetDestination(position);
        }

        private void Reload() {
            IsShellLoaded = true;
        }
        
        public void Heal(int healing) {
            CurrentHP += healing;
            if (CurrentHP > MaxHP) CurrentHP = MaxHP;
        }

        public GameObject TankInRay() {
            RaycastHit hit;
            if (Physics.Raycast(CanonOut.position, CanonOut.forward, out hit, Mathf.Infinity)) {
                if (hit.transform.GetComponent<TankEntity>()) return hit.transform.gameObject;
            }
            return null;
        }
        
    }
}
