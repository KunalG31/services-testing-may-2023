namespace Banking;

public class Account
{
    private decimal _balance = 5000M;
    public void Deposit(decimal amountToDeposit)
    {
        _balance += amountToDeposit;
    }

    public decimal GetBalance()
    {
        return _balance; // "Slimed" means fake, can't stay - Gary Bernhardt
    }

    public void Withdraw(decimal amountToWithdraw)
    {
        GuardAgainstOverdraft(amountToWithdraw);


        _balance -= amountToWithdraw;

    }

    private void GuardAgainstOverdraft(decimal amount)
    {
        if (amount > _balance)
        {
            throw new AccountOverdraftException();
        }
    }
}