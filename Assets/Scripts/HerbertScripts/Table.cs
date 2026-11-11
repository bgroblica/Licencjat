
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
    [SerializeField] private float minEatTime = 10f;
    [SerializeField] private float maxEatTime = 20f;

    [SerializeField] private float waitTime = 20f;

    [SerializeField] private TableIndicator tableIndicator;

    private float timeToEatRemaining;

    private float timeWaitingRemaining;

    public bool isEating = false;
    public bool IsEating => isEating;

    private bool isWaiting = false;
    public bool IsWaiting => isWaiting;

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
            }
        }

        if (isWaiting)
        {
            timeWaitingRemaining -= Time.deltaTime;

            if (timeWaitingRemaining <= 0f)
            {
                timeWaitingRemaining = 0f;
                isWaiting = false;

                NoDish();
            }
        }
    }

    public bool CanInteract()
    {
        return !isEating;
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

        tableIndicator.Hide();

        dishHolder.PutDown();

        foodOnTable = deliveredFood;

        UpdateVisual();

        EatTimer();

        Debug.Log("Correct dish delivered!");

        return true;
    }

    private void EatTimer()
    {
        timeToEatRemaining = Random.Range(minEatTime, maxEatTime);
        isEating = true;

        Debug.Log($"Eating for {timeToEatRemaining:F1} seconds.");
    }

    public void ChooseDish()
    {
        int randomIndex = Random.Range(0, possibleFoods.Length);
    
        requiredFoodType = possibleFoods[randomIndex];
    
        tableIndicator.Show(requiredFoodType);

        WaitTimer();
    
        Debug.Log($"I want {requiredFoodType}");
    }

    private void WaitTimer()
    {
        timeWaitingRemaining = waitTime;
        isWaiting = true;

        Debug.Log($"Waiting for {waitTime:F1} seconds.");
    }

    private void NoDish()
    {
        requiredFoodType = foodType.nothing;
        UpdateVisual();

        Debug.Log("Didn't bring dish in time!");
    }

        private void WrongDish()
    {
        Debug.Log("Wrong Dish!");
    }

    private void UpdateVisual()
    {
        food1Vis.SetActive(foodOnTable == foodType.food1);
        food2Vis.SetActive(foodOnTable == foodType.food2);
        food3Vis.SetActive(foodOnTable == foodType.food3);
    }
}
