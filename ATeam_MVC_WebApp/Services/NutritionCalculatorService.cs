
public static class NutritionCalculatorService
{
    public static bool IsNokkelhullQualified(float fat, float sugar, float salt, float fiber)
    {
         //Krav kan endres eller legges til her
        float max_energyKcal = 40; //gram per 100g
        float max_fat = 3; // gram per 100g
        float max_carbohydrates = 40; //gram per 100g
        float min_protein= 3; // gram per 100g
        float min_fiber = 6; // gram per 100g 
        float max_salt = 1; // gram per 100g
        
        if (max_energyKcal <= max_energyKcal && fett <= max_fat && carbohydrates <= max_carbohydrates
         && protein >= min_protein && fiber >= min_fiber && salt <= max_salt)
        {
            return true;
        }
        else
        {
           return false;
        }
    }
}


