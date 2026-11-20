
using UnityEngine;

public class Table : MonoBehaviour, IInteractable
{
    private foodType requiredFoodType;
    private foodType foodOnTable;

    [Header("Dish visuals")]
    [SerializeField] private GameObject foodSoupVis;
    [SerializeField] private GameObject foodMeatVis;
    [SerializeField] private GameObject foodShrimpVis;

    [Header("Timers")]
    [SerializeField] private float minEatTime = 10f;
    [SerializeField] private float maxEatTime = 20f;
    [SerializeField] private float waitTime = 20f;

    [Header("Stress damage")]
    [SerializeField] private float noDishStress = 0.025f;
    [SerializeField] private float wrongDishStress = 0.001f;

    [Header("References")]
    [SerializeField] private TableIndicator tableIndicator;

    private float timeToEatRemaining;

    private float timeWaitingRemaining;

    public bool isEating = false;
    public bool IsEating => isEating;

    private bool isWaiting = false;
    public bool IsWaiting => isWaiting;

    private DishHolder dishHolder;

    private StressBar stressBar;

    private readonly foodType[] possibleFoods =
    {
        foodType.foodSoup,
        foodType.foodMeat,
        foodType.foodShrimp
    };

    private void Awake()
    {
        foodOnTable = foodType.nothing;

        UpdateVisual();

        dishHolder = FindFirstObjectByType<DishHolder>();
        stressBar = FindAnyObjectByType<StressBar>();
        tableIndicator = GetComponentInChildren<TableIndicator>();
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
    }

    public void ChooseDish()
    {
        int randomIndex = Random.Range(0, possibleFoods.Length);
    
        requiredFoodType = possibleFoods[randomIndex];
    
        tableIndicator.Show(requiredFoodType);

        WaitTimer();
    }

    private void WaitTimer()
    {
        timeWaitingRemaining = waitTime;
        isWaiting = true;
    }

    private void NoDish()
    {
        requiredFoodType = foodType.nothing;
        UpdateVisual();

        stressBar.AddStress(noDishStress);
    }

        private void WrongDish()
    {
        stressBar.AddStress(wrongDishStress);
        Debug.Log("Wrong Dish");
    }

    private void UpdateVisual()
    {
        foodSoupVis.SetActive(foodOnTable == foodType.foodSoup);
        foodMeatVis.SetActive(foodOnTable == foodType.foodMeat);
        foodShrimpVis.SetActive(foodOnTable == foodType.foodShrimp);
    }
}
