
using UnityEngine;

public class Table : MonoBehaviour, IInteractable
{
    [SerializeField] private foodType requiredFoodType;
    [SerializeField] private foodType foodOnTable;

    [Header("Dish visuals")]
    [SerializeField] private GameObject food1Vis;
    [SerializeField] private GameObject food2Vis;
    [SerializeField] private GameObject food3Vis;

    [Header("Timers")]
    [SerializeField] private float minEatTime = 15f;
    [SerializeField] private float maxEatTime = 30f;

    [SerializeField] private float minWaitTime = 15f;
    [SerializeField] private float maxWaitTime = 30f;

    private float timeRemaining;
    private float timeToEatRemaining;

    private bool isWaiting = false;
    private bool isEating = false;

    private DishHolder dishHolder;

    private readonly foodType[] possibleFoods =
    {
        foodType.food1,
        foodType.food2,
        foodType.food3
    };

    private void Awake()
    {
        foodOnTable = foodType.nothing;

        UpdateVisual();

        dishHolder = FindFirstObjectByType<DishHolder>();

        ChooseDish();
    }

    private void Update()
    {
        if (isEating)
        {
            timeToEatRemaining -= Time.deltaTime;

            if (timeToEatRemaining <= 0f)
            {
                timeToEatRemaining = 0f;
                isEating = false;

                foodOnTable = foodType.nothing;
                UpdateVisual();

                ChooseDish();
            }
        }
        else if (isWaiting)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                NoDish();
                ChooseDish();
            }
        }
    }

    public bool CanInteract()
    {
        return isWaiting && !isEating;
    }

    public bool Interact(Interactor interactor)
    {
        return DishCheck();
    }

    private bool DishCheck()
    {
        if (dishHolder == null)
        {
            Debug.LogError("No DishHolder found!");
            return false;
        }

        if (dishHolder.dishTypeInHand != requiredFoodType)
        {
            WrongDish();
            return false;
        }

        foodType deliveredFood = dishHolder.dishTypeInHand;

        dishHolder.PutDown();

        foodOnTable = deliveredFood;

        UpdateVisual();

        isWaiting = false;

        EatTimer();

        Debug.Log("Correct dish delivered!");

        return true;
    }

    private void WrongDish()
    {
        Debug.Log("Wrong Dish!");
    }

    private void EatTimer()
    {
        timeToEatRemaining = Random.Range(minEatTime, maxEatTime);
        isEating = true;

        Debug.Log($"Eating for {timeToEatRemaining:F1} seconds.");
    }

    private void ChooseDish()
    {
        int randomIndex = Random.Range(0, possibleFoods.Length);

        requiredFoodType = possibleFoods[randomIndex];

        OrderTimer();

        Debug.Log($"I want {requiredFoodType}");
    }

    private void OrderTimer()
    {
        timeRemaining = Random.Range(minWaitTime, maxWaitTime);
        isWaiting = true;

        Debug.Log($"You have {timeRemaining:F1} seconds to deliver the food.");
    }

    private void NoDish()
    {
        isWaiting = false;

        requiredFoodType = foodType.nothing;

        Debug.Log("Didn't bring dish in time!");
    }

    private void UpdateVisual()
    {
        food1Vis.SetActive(foodOnTable == foodType.food1);
        food2Vis.SetActive(foodOnTable == foodType.food2);
        food3Vis.SetActive(foodOnTable == foodType.food3);
    }
}
