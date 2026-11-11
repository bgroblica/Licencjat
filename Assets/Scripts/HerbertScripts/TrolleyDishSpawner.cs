
using UnityEngine;

public class TrolleyDishSpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private foodType trolleyType;

    [Header("Dish visuals")]
    [SerializeField] private GameObject food1Vis;
    [SerializeField] private GameObject food2Vis;
    [SerializeField] private GameObject food3Vis;

    private DishHolder dishHolder;

    private void Awake()
    {
        dishHolder = FindFirstObjectByType<DishHolder>();

        if (dishHolder == null)
        {
            Debug.LogError("No DishHolder found in the scene!", this);
        }

        UpdateVisual();
    }

    public bool CanInteract()
    {
        return dishHolder != null;
    }

    public bool Interact(Interactor interactor)
    {
        if (dishHolder == null)
        {
            return false;
        }

        dishHolder.PickUp(trolleyType);

        Debug.Log($"Picked up {trolleyType} from {gameObject.name}");

        return true;
    }

    private void UpdateVisual()
    {
        food1Vis.SetActive(trolleyType == foodType.food1);
        food2Vis.SetActive(trolleyType == foodType.food2);
        food3Vis.SetActive(trolleyType == foodType.food3);
    }
}
