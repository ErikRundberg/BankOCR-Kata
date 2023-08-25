// https://codingdojo.org/kata/BankOCR/
namespace BankOCR.Bank;

public class Number
{
    public int[] Likelihood { get; set; } = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int Strokes { get; set; } = 0;
    public List<string> Possible { get; set; } = new();

    // Returns digit or ? if unclear
    public string GetDigit()
    {
        for (int i = 0; i < Likelihood.Length; i++)
            if (Likelihood[i] == Strokes) return i.ToString();
        return "?";
    }

    public void SetPossible()
    {
        for (int i = 0; i < Likelihood.Length; i++)
            if (Math.Abs(Likelihood[i] - Strokes) <= 1) Possible.Add(i.ToString());
    }
}