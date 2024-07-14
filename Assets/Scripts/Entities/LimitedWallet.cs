namespace Entities
{
    public class LimitedWallet : Wallet
    {
        private readonly int _limit;

        public LimitedWallet(int limit)
        {
            _limit = limit;
        }

        public LimitedWallet(int balance, int limit) : base(balance)
        {
            _limit = limit;
        }

        public override void Add(int amount)
        {
            var total = Balance + amount > _limit ? _limit - Balance : amount;
            base.Add(total);
        }
    }
}