
namespace ATeam_MVC_WebApp.Services;

public static class NutritionCalculatorService
{
    public static bool IsNokkelhullQualified(float energyKcal, float fat, float carbohydrates,
        float protein, float fiber, float salt)
    {
        //Krav kan endres eller legges til her
        const float maxEnergyKcal = 40; //gram per 100g
        const float maxFat = 3; // gram per 100g
        const float maxCarbohydrates = 40; //gram per 100g
        const float minProtein = 3; // gram per 100g
        const float minFiber = 6; // gram per 100g
        const float maxSalt = 1; // gram per 100g
        
        return energyKcal <= maxEnergyKcal && fat <= maxFat && carbohydrates <= maxCarbohydrates
               && protein >= minProtein && fiber >= minFiber && salt <= maxSalt;
    }
}