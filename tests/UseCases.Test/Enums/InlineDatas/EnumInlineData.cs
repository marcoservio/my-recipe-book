using CommonTestUtilities.Requests;

using System.Collections;

namespace UseCases.Test.Enums.InlineDatas;

public class EnumInlineData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var enums = EnumsBuilder.EnumCollection();
        foreach (var e in enums)
            yield return new object[] { e };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
