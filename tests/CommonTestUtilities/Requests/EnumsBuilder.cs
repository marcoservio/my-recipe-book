using MyRecipeBook.Enums.Attibutes;

namespace CommonTestUtilities.Requests;

public class EnumsBuilder
{
    public static IList<string> EnumCollection()
    {
        return [.. AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsEnum &&
                    t.GetCustomAttributes(typeof(ExposeEnumAttribute), false).Length != 0)
                    .Select(t => t.Name)];
    }

    public static IList<string> EnumNotFoundCollection()
    {
        IList<string> enums = [.. AppDomain.CurrentDomain
                                .GetAssemblies()
                                .Where(a => !a.IsDynamic)
                                .SelectMany(a => a.GetTypes())
                                .Where(t => t.IsEnum &&
                                            t.Namespace == nameof(MyRecipeBook.Enums) &&
                                            t.GetCustomAttributes(typeof(ExposeEnumAttribute), false).Length == 0)
                                .Select(t => t.Name)];

        enums.Add("CookingTimes");
        enums.Add("Difficulties");
        enums.Add("DishTypes");

        return enums;
    }
}
