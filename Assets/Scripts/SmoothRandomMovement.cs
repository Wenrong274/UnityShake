using UnityEngine;

public class SmoothRandomMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 0.6f;
    [SerializeField] private float movementRange = 0.5f;

    private Transform m_transform;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float smoothTime = 0;

    private void Awake()
    {
        m_transform = transform;
    }

    private void Start()
    {
        // 初始化目標位置為起始位置
        targetPosition = m_transform.localPosition;
        // 記錄初始位置
        initialPosition = m_transform.localPosition;
    }

    private void Update()
    {
        if (smoothTime <= 0)
        {
            smoothTime = movementSpeed;
            // 計算新的目標位置
            targetPosition = initialPosition + Random.insideUnitSphere * movementRange;
        }
        smoothTime -= Time.deltaTime;
        // 平滑移動到新的目標位置
        m_transform.localPosition = Vector3.Lerp(m_transform.localPosition, targetPosition, Time.deltaTime * movementSpeed);
    }
}
