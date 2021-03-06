﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.ClownTorch {
    public class ClownTorchTorchObject : MonoBehaviour {

        public GameObject fireEff;
        public ParticleSystem smokeEff;
        public ParticleSystem extinguishEff;
        public float requiredTime = 0.5f;
        float timer = 0.0f;
        bool isOnFire = false;

        bool countedThisFrame = false;
        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
            if (isOnFire) {
                timer += Time.deltaTime;
            }

            if (timer >= requiredTime) {
                fireEff.SetActive(true);
                smokeEff.Stop();
            }
        }

        public bool IsLit() {
            return fireEff.activeSelf;
        }

        public void TurnOn() {
            fireEff.SetActive(true);
        }

        public void TurnOff() {
            if (IsLit()) {
                fireEff.SetActive(false);
                timer = 0.0f;
                extinguishEff.Play();
            }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            var tag = col.GetComponentInParent<ClownTorchTag>().type;
            switch (tag) {
                case ClownTorchTag.Type.Pyre:
                    if (GetComponent<ClownTorchTag>().type != ClownTorchTag.Type.ClownTorch) {
                        smokeEff.Play();
                        isOnFire = true;
                    }
                    break;
                case ClownTorchTag.Type.Water:
                    if (GetComponent<ClownTorchTag>().type != ClownTorchTag.Type.ClownTorch) {
                        isOnFire = false;
                        TurnOff();
                    }
                    break;
                case ClownTorchTag.Type.PlayerTorch:
                    var obj = col.GetComponentInParent<ClownTorchTorchObject>();
                    if (obj.IsLit() && !IsLit()) {
                        smokeEff.Play();
                        isOnFire = true;
                    }
                    break;
                case ClownTorchTag.Type.ClownTorch:
                    break;
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            smokeEff.Stop();
            isOnFire = false;
        }
    }
}