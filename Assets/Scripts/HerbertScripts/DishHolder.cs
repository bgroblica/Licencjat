using UnityEngine;

public class DishHolder : MonoBehaviour
{
    [Header("Dish visuals")]
    [SerializeField] private GameObject food1Vis;
    [SerializeField] private GameObject food2Vis;
    [SerializeField] private GameObject food3Vis;

    public foodType dishTypeInHand{ get; private set;}

    private void Awake()
    {
        dishTypeInHand = foodType.nothing;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        food1Vis.SetActive(dishTypeInHand == foodType.food1);
        food2Vis.SetActive(dishTypeInHand == foodType.food2);
        food3Vis.SetActive(dishTypeInHand == foodType.food3);
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
