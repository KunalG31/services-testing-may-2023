using Banking;

namespace ScratchTests.BankAccount;

public class MakingDeposits
{

    [Theory]
    [InlineData(110.12)]
    [InlineData(20)]
    [InlineData(8128.53)]
    public void DepositsIncreaseTheBalance(decimal amountToDeposit)
    {
        // Given
        var account = new Account();
        var openingBalance = account.GetBalance();

        // When
        account.Deposit(amountToDeposit); // A State Transition (State-based testing)

        // Then
        Assert.Equal(openingBalance + amountToDeposit, account.GetBalance());
    }
}