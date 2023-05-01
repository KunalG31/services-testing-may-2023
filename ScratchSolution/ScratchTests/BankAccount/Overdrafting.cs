namespace ScratchTests.BankAccount;

public class Overdrafting
{
    [Fact]
    public void OverdraftDoesNotDecreaseBalance()
    {
        var account = new Account();
        var openingBalance = account.GetBalance();

        try
        {
            account.Withdraw(openingBalance + 0.01M);
        }
        catch (Exception)
        {

            // gulp!
        }
        finally
        {

            Assert.Equal(openingBalance, account.GetBalance());
        }
    }

    [Fact]
    public void OverdraftThrowsAnException()
    {
        var account = new Account();
        var openingBalance = account.GetBalance();

        Assert.Throws<AccountOverdraftException>(() =>
        {
            account.Withdraw(openingBalance + .01M);
        });
    }

}
