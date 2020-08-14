using Tobii.XR;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataTransparencyPopup : MonoBehaviour
{
#pragma warning disable 649 // Field is never assigned
    [SerializeField] private GameObject _dataTransparencyUiCanvas;
    [SerializeField] private float _maxViewAngle = 20f;
    [SerializeField] private float PreferredDistance = 2f;
    [SerializeField] private float ClampBottomY = .74f;
    [SerializeField] private float _slerpRate = .1f;
#pragma warning restore 649

    private Quaternion _targetRot;
    private Vector3 _targetPos;

    private Transform _camera;

    private void Start()
    {
        _camera = CameraHelper.GetCameraTransform();
    }

    private void Update()
    {
        float angleBetweenHeadsetAndPopup = Vector3.Angle(_dataTransparencyUiCanvas.transform.position - _camera.transform.position, _camera.transform.forward);
        if (angleBetweenHeadsetAndPopup > _maxViewAngle) // if too far from center of view
        {
            MovePopupIntoFov(); // set target location back inside user's field of view
        }

        _dataTransparencyUiCanvas.transform.position = Vector3.Slerp(_dataTransparencyUiCanvas.transform.position, _targetPos, _slerpRate);
        _dataTransparencyUiCanvas.transform.rotation = Quaternion.Slerp(_dataTransparencyUiCanvas.transform.rotation, _targetRot, _slerpRate);
    }

    private void MovePopupIntoFov()
    {
        _targetPos = _camera.transform.position + _camera.transform.forward * PreferredDistance; // project ahead of gaze 

        if (_targetPos.y < ClampBottomY)
            _targetPos.Scale(new Vector3(1, 0, 1)); // wipe out y level

        while (_targetPos.y < ClampBottomY) // keep bumping it up until we hit minimum desired location
        {
            _targetPos += new Vector3(0, ClampBottomY, 0);
            _targetPos += Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
        }

        Quaternion newRotation = Quaternion.LookRotation(_targetPos - _camera.transform.position); // face user
        _targetRot.eulerAngles.Set(_targetRot.eulerAngles.x, _targetRot.eulerAngles.y, 0);
        _targetRot = newRotation;
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}