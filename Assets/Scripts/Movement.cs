using UnityEngine;

namespace hyhy
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 0.6f;
        [SerializeField] private float movementRange = 0.5f;

        private Transform m_transform;
        private Vector3 initialPosition;
        private Vector3 targetPosition;
        private float currentSmoothTime = 0;

        private void Awake()
        {
            m_transform = transform;
        }

        private void Start()
        {
            initialPosition = m_transform.localPosition;
            ResetTargetPosition();
        }

        private void Update()
        {
            if (currentSmoothTime <= 0)
            {
                ResetTargetPosition();
            }
            currentSmoothTime -= Time.deltaTime;
            m_transform.localPosition = Vector3.Lerp(m_transform.localPosition, targetPosition, Time.deltaTime * movementSpeed);
        }

        private void ResetTargetPosition()
        {
            currentSmoothTime = movementSpeed;
            targetPosition = initialPosition + Random.insideUnitSphere * movementRange;
        }
    }
}
