using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TableIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image foodIcon;

    [Header("Food Icons")]
    [SerializeField] private Sprite meatIcon;
    [SerializeField] private Sprite soupIcon;
    [SerializeField] private Sprite shrimpIcon;

    [Header("Floating")]
    [SerializeField] private float floatHeight = 0.25f;
    [SerializeField] private float floatDuration = 1f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 8f;

    private Transform iconTransform;
    private Vector3 startPosition;
    private Tween floatTween;
    private Transform player;

    private void Awake()
    {
        iconTransform = foodIcon.transform;
        startPosition = iconTransform.localPosition;

        foodIcon.gameObject.SetActive(false);

        if (player == null)
        {
            PlayerMovement playerInstance =
                FindFirstObjectByType<PlayerMovement>();

            if (playerInstance != null)
                player = playerInstance.transform;
            else
                Debug.LogWarning("PlayerMovement not found!", this);
        }
    }

    private void LateUpdate()
    {
        if (!foodIcon.gameObject.activeInHierarchy || player == null)
            return;

        Vector3 direction = player.position - iconTransform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction) *
                Quaternion.Euler(0f, 180f, 0f);

            iconTransform.rotation = Quaternion.Slerp(iconTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void Show(foodType food)
    {
        Sprite sprite = food switch
        {
            foodType.foodSoup => soupIcon,
            foodType.foodMeat => meatIcon,
            foodType.foodShrimp => shrimpIcon,
            _ => null
        };

        if (sprite == null)
        {
            Debug.LogWarning($"Missing icon for {food}", this);
            Hide();
            return;
        }

        foodIcon.sprite = sprite;
        foodIcon.gameObject.SetActive(true);

        floatTween?.Kill();
        iconTransform.localPosition = startPosition;

        floatTween = iconTransform.DOLocalMoveY(startPosition.y + floatHeight, floatDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void Hide()
    {
        floatTween?.Kill();
        floatTween = null;

        iconTransform.localPosition = startPosition;
        foodIcon.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        floatTween?.Kill();
    }
}