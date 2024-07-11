namespace Entities
{
    public interface IWallet
    {
        public int Balance { get; }

        public void Add(int amount);

        public bool TrySpend(int amount);
    }
}