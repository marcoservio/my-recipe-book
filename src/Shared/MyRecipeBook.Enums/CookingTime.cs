using MyRecipeBook.Enums.Attibutes;

namespace MyRecipeBook.Enums;

[ExposeEnum]
public enum CookingTime
{
    Less_10_Minutes = 0,
    Between_10_30_Minutes = 1,
    Between_30_60_Minutes = 2,
    Greater_60_Minutes = 3,
}
