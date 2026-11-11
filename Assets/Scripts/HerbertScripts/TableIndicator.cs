
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TableIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform textTransform;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform player;

    [Header("Floating")]
    [SerializeField] private float floatHeight = 0.25f;
    [SerializeField] private float floatDuration = 1f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 8f;

    private Vector3 startPosition;
    private Tween floatTween;

    private void Awake()
    {
        startPosition = textTransform.localPosition;
        textTransform.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!textTransform.gameObject.activeSelf || player == null)
            return;

        Vector3 direction = player.position - textTransform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f, 0f);

            textTransform.rotation = Quaternion.Slerp(
                textTransform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void Show(foodType food)
    {
        text.text = food.ToString();

        if (textTransform.gameObject.activeSelf)
            return;

        textTransform.gameObject.SetActive(true);

        floatTween = textTransform.DOLocalMoveY(startPosition.y + floatHeight, floatDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void Hide()
    {
        floatTween?.Kill();
        floatTween = null;

        textTransform.localPosition = startPosition;
        textTransform.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        floatTween?.Kill();
    }
}
