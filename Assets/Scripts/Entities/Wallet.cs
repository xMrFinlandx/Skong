using System;

namespace Entities
{
    public class Wallet : IWallet
    {
        public int Balance { get; private set; }
        
        public event Action<int, int> BalanceChangedAction;

        public Wallet()
        {
        }

        public Wallet(int balance)
        {
            Balance = balance;
        }

        public virtual void Add(int amount)
        {
            Balance += amount;
            BalanceChangedAction?.Invoke(Balance, amount);
        }

        public bool TrySpend(int amount)
        {
            if (Balance - amount < 0)
                return false;

            Balance -= amount;
            BalanceChangedAction?.Invoke(Balance, -amount);

            return true;
        }
    }
}