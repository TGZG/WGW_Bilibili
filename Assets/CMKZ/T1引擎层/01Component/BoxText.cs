using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public class BoxText : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
        private void OnMouseDown() {
            UnityEngine.Debug.Log("OnMouseDown");
            
        }
        public void OnMouseEnter() {
            SetCursor(AllT2D["挖掘光标"]);
        }
        public void OnMouseExit() {
            SetCursor(AllT2D["常规光标"]);
        }
    }
}