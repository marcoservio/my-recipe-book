using Sqids;

namespace CommonTestUtilities.IdEncryption;

public class IdEncripterBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "S92xuzMXCK0lNEf5VYFpWvAJZO6gITrc148UReb3aij7hDomByLPwqstkdHQGn"
        });
    }
}
