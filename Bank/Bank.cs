// https://codingdojo.org/kata/BankOCR/

namespace BankOCR.Bank;

public static class Bank
{
  #region Numbers
    private static readonly string[] Zero =
    { " _ ",
      "| |",
      "|_|" };
    private static readonly string[] One = 
    { "   ",
      "  |",
      "  |" };
    private static readonly string[] Two =
    { " _ ",
      " _|",
      "|_ " };
    private static readonly string[] Three = 
    { " _ ",
      " _|",
      " _|" };
    private static readonly string[] Four =
    { "   ",
      "|_|",
      "  |" };
    private static readonly string[] Five =
    { " _ ",
      "|_ ",
      " _|" };
    private static readonly string[] Six = 
    { " _ ",
      "|_ ",
      "|_|" };
    private static readonly string[] Seven = 
    { " _ ",
      "  |",
      "  |" };
    private static readonly string[] Eight = 
    { " _ ",
      "|_|",
      "|_|" };
    private static readonly string[] Nine = 
    { " _ ",
      "|_|",
      " _|" };
    
    private static readonly List<string[]> Numbers = new()
        { Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine };
    #endregion

    // Returns each number split into 3 rows
    private static IEnumerable<string[]> ParseOcr(string input)
    {
      List<string> ocr = input.Split("\r\n").ToList();
      List<string[]> numbers = new();

      for (int row = 0; row < ocr.Count; row++)
        for (int i = 0; i < ocr[row].Length; i++)
        {
          int index = i / 3;
          if (row == 0 && index >= numbers.Count) numbers.Add(new string[3]);
          numbers[index][row] += ocr[row][i];
        }
      return numbers;
    }

    private static IReadOnlyList<Number> ParseNumbers(IEnumerable<string[]> numbers)
    {
      List<Number> result = new();

      foreach (string[] number in numbers)
      {
        Number num = new();

        for (int row = 0; row < number.Length; row++)
          for (int c = 0; c < number[0].Length; c++)
          {
            if (c != ' ') num.Strokes += 1;
            for (int check = 0; check < Numbers.Count; check++)
              if (number[row][c] == Numbers[check][row][c])
                num.Likelihood[check] += 1;
          }
        result.Add(num);
      }

      return result;
    }

    private static string GeneratePossible(IReadOnlyList<Number> numbers)
    {
      string readDigits = "";
      foreach (var number in numbers)
      {
        number.SetPossible();
        readDigits += number.GetDigit();
      }

      return readDigits;
    }
    
    private static bool CheckSum(string input)
    {
      int sum = 0;
       char[] numbers = input.ToCharArray();
       Array.Reverse(numbers);
       for (int i = 0; i < input.Length; i++)
         sum += (i + 1) * (numbers[i] - '0');
      return sum % 11 == 0;
    }

    private static List<string> GenerateCombinations(IReadOnlyList<Number> numbers, int currentIndex = 0)
    {
      return currentIndex == numbers.Count ? new List<string> { "" } : (from possibleNumber in numbers[currentIndex].Possible let nextCombinations = GenerateCombinations(numbers, currentIndex + 1) from nextCombination in nextCombinations select possibleNumber + nextCombination).ToList();
    }

    private static string GetValidCombinations(IReadOnlyList<Number> numbers)
    {
      string errorOcr = GeneratePossible(numbers);
      List<string> combinations = GenerateCombinations(numbers);
      List<string> passingCombinations = combinations.Where(CheckSum).ToList();

      return passingCombinations.Count switch
      {
        0 => errorOcr + " ILL",
        1 => errorOcr,
        _ => passingCombinations[0] + " AMB ['" + string.Join("', '", passingCombinations.Skip(1)) + "']"
      };
    }

    public static string ReadOcr(string ocr)
    {
      IEnumerable<string[]> parseOcr = ParseOcr(ocr);
      IReadOnlyList<Number> parseNumbers = ParseNumbers(parseOcr);
      return GetValidCombinations(parseNumbers);
    }
}