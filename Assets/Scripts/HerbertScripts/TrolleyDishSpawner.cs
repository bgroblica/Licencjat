
using UnityEngine;

public class TrolleyDishSpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private foodType trolleyType;

    [Header("Dish visuals")]
    [SerializeField] private GameObject foodSoupVis;
    [SerializeField] private GameObject foodMeatVis;
    [SerializeField] private GameObject foodShrimpVis;

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
        foodSoupVis.SetActive(trolleyType == foodType.foodSoup);
        foodMeatVis.SetActive(trolleyType == foodType.foodMeat);
        foodShrimpVis.SetActive(trolleyType == foodType.foodShrimp);
    }
}
