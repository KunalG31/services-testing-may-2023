using Banking;
using System.Security.Principal;

namespace ScratchTests.BankAccount;

public class NewAccount
{
    [Fact]
    public void NewAccountsHaveCorrectBalance()
    {
        // Given
        var account = new Account();

        // When 
        decimal openingBalance = account.GetBalance();

        // Then
        Assert.Equal(5000M, openingBalance); //
    }
}
