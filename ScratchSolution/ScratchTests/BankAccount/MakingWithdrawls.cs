namespace ScratchTests.BankAccount;

public class MakingWithdrawals
{
    [Fact]
    public void MakingWithdrawalsLowersBalance()
    {
        var account = new Account();
        var openingBalance = account.GetBalance();
        var amountToWithdraw = 100M;

        account.Withdraw(amountToWithdraw);

        Assert.Equal(openingBalance - amountToWithdraw, account.GetBalance());

    }
}