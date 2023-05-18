using System.Security.Cryptography;
using Application.Extensions;
using Application.Interfaces;
using Application.Models;

namespace Application.Services;

public class PasswordCreatorService : IPasswordCreatorService, IDisposable
{
    private readonly string numbersList = "0123456789";
    private readonly string upperLettersList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private readonly string lowerLettersList = "abcdefghijklmnopqrstuvwxyz";
    private readonly string specialLettersList = """!#$%&'()*+,.-_/;:<>=?@[]^`{}|~""";  //"""!"#$%&\'()*+,.-_/;:<>=?@[]\\^`{}|~""";
    private readonly RandomNumberGenerator rnd = RandomNumberGenerator.Create();

    /// <summary>
    /// LL - letters lowercase, LU - letters uppercase, LD - letters digits, LS - letters special
    /// 1) Determine number of arrays to calculate (depends on password options if user needs lu, ld, ls, etc). First 1 constant - means LL.
    ///    and calculate length of LD, LU, LS arrays
    /// 2) randomly generate number LD for uppercase.
    /// 3) randomly generate number LU for uppercase.
    /// 4) randomly generate number LS for uppercase.
    /// 5) LL array length will be [password lenght - sum(<all other arrays>.length)]
    /// 6) randomly generate LL lowercase letters
    /// 7) concatenate arrays into one array
    /// 8) Shuffle the array randomly
    /// 9) test result with regexp
    /// </summary>
    public string CreatePassword(CreatePasswordOptions options)
    {
        // step 1
        var randomArraysNumber = 1 + options.IncludeNumbers.ToShort() + options.IncludeUpperCaseLetters.ToShort() + options.IncludeSpecialCharacters.ToShort();
        var arraysLength = (int)Math.Floor((decimal)(options.Length / randomArraysNumber));

        // step 2 - 4
        List<char> ld = options.IncludeNumbers ? GetRandomList(arraysLength, numbersList) : new List<char>();
        List<char> lu = options.IncludeUpperCaseLetters ? GetRandomList(arraysLength, upperLettersList) : new List<char>();
        List<char> ls = options.IncludeSpecialCharacters ? GetRandomList(arraysLength, specialLettersList) : new List<char>();

        // step 5
        var llLength = options.Length - (ld.Count() + lu.Count() + ls.Count());

        // step 6 - 7
        List<char> charResult = GetRandomList(llLength, lowerLettersList)
            .Concat(ld).Concat(lu).Concat(ls).ToList();

        // step 8
        var result = string.Join("", Shuffle(charResult));

        return result;
    }

    private int GetRandomInt()
    {
        var data = new byte[sizeof(int)];
        rnd.GetBytes(data);
        return BitConverter.ToInt32(data, 0);
    }

    private IEnumerable<T> Shuffle<T>(IEnumerable<T> list)
    {
        return list.OrderBy(x => GetRandomInt()).ToList();
    }

    private List<char> GetRandomList(int length, string charsList)
    {
        var result = new List<char>();

        for (var i = 0; i < length; i++)
        {
            var rndInt = GetRandomInt();
            result.Add(charsList.GetRandomChar(rndInt));
        }

        return result;
    }

    public void Dispose()
    {
        rnd.Dispose();
    }
}
