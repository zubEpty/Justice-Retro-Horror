using System.Buffers.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OutsideCameraController : MonoBehaviour
{
    [Header("Read-Only Debug Info")]
    [SerializeField] private float currentAngle;

   
    [SerializeField] private float _startAngle = -45f;
    [SerializeField] private float _endAngle = 45f;
    [SerializeField] private float _sweepDuration = 4f;

    [SerializeField] private CircuitBoxInterActor _circuitBox;
    [SerializeField] private GameObject _jumpscareJack;
    [SerializeField] private GameObject _playerCam;

    public UnityEvent OnJumpscareEnabledEvent;

    

    private float t = 0f;
    private bool isReversing = false;


    private float baseX;
    private float baseZ;

    void Start()
    {
        DoJumpscareCamera();
        // Cache the original X and Z rotation
        Vector3 initialEuler = transform.localEulerAngles;
        baseX = initialEuler.x;
        baseZ = initialEuler.z;
    }

    void Update()
    {
        if (_sweepDuration <= 0f) return;

        // Update t (ping-pong between 0 and 1)
        float delta = Time.deltaTime / (_sweepDuration / 2f); // Half duration for one direction
        t += isReversing ? -delta : delta;
        t = Mathf.Clamp01(t);

        // Interpolate Y angle
        float yAngle = Mathf.Lerp(_startAngle, _endAngle, t);

        // Apply rotation while keeping X and Z
        transform.localEulerAngles = new Vector3(baseX, yAngle, baseZ);

        // Reverse when reaching ends
        if (t >= 1f || t <= 0f)
        {
            isReversing = !isReversing;
        }
    }

    public void DoJumpscareCamera()
    {
        if(_circuitBox.OnPowerCircuitEnabled)
        {
            _jumpscareJack.SetActive(true);
            StartCoroutine(ExitCameraState());
        }
    }

    IEnumerator ExitCameraState()
    {
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
        OnJumpscareEnabledEvent.Invoke();
        _playerCam.SetActive(true);
    }
}
