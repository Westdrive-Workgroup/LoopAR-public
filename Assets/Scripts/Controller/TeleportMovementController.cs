using System;
using System.Collections;
using LoopAr.Internals;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace LoopAr.Controller
{
    //TODO: Delete this as an Example Controller
    public class TeleportMovementController : AbstractMovementController
    {
        [SerializeField] private int jumpTime;

        private Vector3 originPosition;
        private GameObject _gameObject;
        private CountDowner countDowner;

        private new void Awake()
        {
            base.Awake();
            _gameObject = gameObject;
            originPosition = _gameObject.transform.position;
            countDowner = _gameObject.AddComponent<CountDowner>();
            countDowner.SetMaximumTime(jumpTime);
        }

        private void Start()
        {
            countDowner.SetSpecificCountDownStatus(true);
        }

        private void Update()
        {
            ShowMovementBehavior();
        }

        internal override void SetToPosition(Vector3 updatePosition)
        {
            _gameObject.transform.position = updatePosition;
        }

        internal override void ShowMovementBehavior()
        {
            if (!countDowner.TimeErased())
                _gameObject.transform.position += (Vector3.right * Time.deltaTime);
            else
            {
                _gameObject.transform.position = originPosition;
                countDowner.ResetCoundDown();
            }
        }

        public override void RequestColorChange()
        {
            StartCoroutine(ChangeColorForSeconds(colorChangeSeconds));
        }

        private IEnumerator ChangeColorForSeconds(int waitForSeconds)
        {
            _objectMaterial.color = changeColor;
            yield return new WaitForSeconds(waitForSeconds);
            _objectMaterial.color = _originColor;
        }
    }
}