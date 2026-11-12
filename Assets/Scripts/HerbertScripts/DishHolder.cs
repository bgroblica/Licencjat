using UnityEngine;

public class DishHolder : MonoBehaviour
{
    [Header("Dish visuals")]
    [SerializeField] private GameObject foodSoupVis;
    [SerializeField] private GameObject foodMeatVis;
    [SerializeField] private GameObject foodShrimpVis;

    public foodType dishTypeInHand{ get; private set;}

    private void Awake()
    {
        dishTypeInHand = foodType.nothing;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foodSoupVis.SetActive(dishTypeInHand == foodType.foodSoup);
        foodMeatVis.SetActive(dishTypeInHand == foodType.foodMeat);
        foodShrimpVis.SetActive(dishTypeInHand == foodType.foodShrimp);
    }
    public void PickUp(foodType type)
    {
        dishTypeInHand = type;
        UpdateVisual();
    }

    public void PutDown()
    {
        dishTypeInHand = foodType.nothing;
        UpdateVisual();
    }
}
