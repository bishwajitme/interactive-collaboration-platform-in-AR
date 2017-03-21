﻿using CollaborationEngine.Objects;
using UnityEngine;

namespace CollaborationEngine.Server
{
    public class ServerController : MonoBehaviour
    {
        public UnityEngine.Camera MainCamera;
        public Scene Scene;

        public GameObject ArrowPrefab;
        public GameObject RubiksPrefab;

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                var ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if(Physics.Raycast(ray, out hitInfo))
                {
                    var worldPosition = hitInfo.point;
                    var worldToLocalMatrix = Scene.transform.worldToLocalMatrix;
                    var localPosition = worldToLocalMatrix.MultiplyPoint(worldPosition);

                    Scene.CmdAdd(ArrowPrefab, localPosition, Quaternion.identity, 1, false);
                }
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                Scene.CmdAdd(RubiksPrefab, Vector3.zero, Quaternion.identity, 8, false);
            }
        }
    }
}
