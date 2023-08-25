// https://codingdojo.org/kata/BankOCR/
using static BankOCR.Bank.Bank;

namespace BankOCR;

public class BankTest
{
    [Fact]
    public void Text_AMB_88888888()
    {
        const string input = @" _  _  _  _  _  _  _  _  _ 
|_||_||_||_||_||_||_||_||_|
|_||_||_||_||_||_||_||_||_|";
        
        Assert.Equal("888888888 AMB ['888886888', '888888880', '888888988']", ReadOcr(input));
    }
}