using System;

namespace Entities
{
    public interface IWallet
    {
        public int Balance { get; }
        
        public event Action<int, int> BalanceChangedAction;

        public void Add(int amount);

        public bool TrySpend(int amount);
    }
}