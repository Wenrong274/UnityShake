using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class RandomMovementJob : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 0.6f;
    [SerializeField] private float movementRange = 0.5f;

    private TransformAccessArray transforms;
    private NativeArray<Vector3> velocities;
    private MovementJob job;
    private JobHandle jobHandle;
    private float smoothTime = 0;
    private List<Vector3> initialPositions = new List<Vector3>();

    private void Awake()
    {
        var moves = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            moves.Add(transform.GetChild(i));
            initialPositions.Add(moves[i].localPosition);
        }
        transforms = new TransformAccessArray(moves.ToArray());
        velocities = new NativeArray<Vector3>(transform.childCount, Allocator.Persistent);
    }

    private void Start()
    {
        UpdateVelocities();
    }

    private void UpdateVelocities()
    {
        for (int i = 0; i < velocities.Length; i++)
        {
            velocities[i] = initialPositions[i] + Random.insideUnitSphere * movementRange;
        }
    }

    public void Update()
    {
        if (smoothTime <= 0)
        {
            smoothTime = movementSpeed;
            UpdateVelocities();
        }

        if (jobHandle.IsCompleted)
        {
            job = new MovementJob
            {
                DeltaTime = Time.deltaTime * movementSpeed,
                Velocities = velocities,
            };
            jobHandle = job.Schedule(transforms);
        }
    }

    private void LateUpdate()
    {
        smoothTime -= Time.deltaTime;

        jobHandle.Complete();
    }

    private void OnDestroy()
    {
        transforms.Dispose();
        velocities.Dispose();
    }

    public struct MovementJob : IJobParallelForTransform
    {
        public float DeltaTime;

        [ReadOnly] public NativeArray<Vector3> Velocities;

        public void Execute(int index, TransformAccess transform)
        {
            var velocity = Velocities[index];
            var position = transform.localPosition;
            transform.localPosition = Vector3.Lerp(position, velocity, DeltaTime);
        }
    }
}
