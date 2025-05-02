using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Controle des points de vues et de la camera (focus, panorama ...)
/// </summary>

public class CameraController : MonoBehaviour
{
    // Singleton
    private static CameraController _instance;
    public static CameraController Instance => _instance;


    [SerializeField] private Transform _trackingTarget;
    [SerializeField] private GameObject _camera;
    private float _orthographicsize;


    private void Awake()
    {
        // Initialisation du Singleton
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _orthographicsize = _camera.GetComponent<CinemachineCamera>().Lens.OrthographicSize;
    }

    // Fonction qui montre pendant un court instant une autre target
    public void ShowPanorama()
    {
        _camera.GetComponent<CinemachinePositionComposer>().Damping = new Vector3(3f, 3f, 3f);
        _camera.GetComponent<CinemachineCamera>().Target.TrackingTarget = _trackingTarget;
        _camera.GetComponent<CinemachineCamera>().Lens.OrthographicSize = 8f;
        
        StartCoroutine(showForSeconds());
    }

    IEnumerator showForSeconds()
    {
        yield return new WaitForSeconds(7f);
        _camera.GetComponent<CinemachineCamera>().Target.TrackingTarget = GameManager.Instance.Player;
        _camera.GetComponent<CinemachineCamera>().Lens.OrthographicSize = _orthographicsize;
        _camera.GetComponent<CinemachinePositionComposer>().Damping = new Vector3(1f, 1f, 1f);

        Destroy(gameObject);
    }

    // Quand on entre dans le trigger, ca declenche le changement de target
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_camera != null)
        {
            ShowPanorama();
        }
    }


    // chaking de la camera pour dire qu'on peut pas invoquer le pouvoir choisit
    public void Shake()
    {
        _camera.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }

}
