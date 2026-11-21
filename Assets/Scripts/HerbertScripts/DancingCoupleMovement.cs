
using UnityEngine;
using DG.Tweening;

public class DancingGuest : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementRadius = 1.5f;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float idleDuration = 0.5f;

    [Header("Rotation")]
    [SerializeField] private float rotationAmount = 30f;
    [SerializeField] private float rotationDuration = 1f;

    private Vector3 startPosition;

    private Tween movementTween;
    private Tween rotationTween;

    private void Start()
    {
        startPosition = transform.position;

        StartDancing();
    }

    private void StartDancing()
    {
        Vector2 randomDirection = Random.insideUnitCircle * movementRadius;

        Vector3 targetPosition = startPosition + new Vector3(randomDirection.x, 0f, randomDirection.y);

        movementTween = transform.DOMove(targetPosition, moveDuration).SetEase(Ease.InOutSine).OnComplete(() => {DOVirtual.DelayedCall(idleDuration, StartDancing).SetLink(gameObject);});

        rotationTween = transform.DORotate(new Vector3(0f, rotationAmount, 0f), rotationDuration).SetRelative().SetLoops(2, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        movementTween?.Kill();
        rotationTween?.Kill();
    }
}
