namespace Entities
{
    public interface IPlayerStats
    {
        public IWallet CoinsWallet { get; }
        public IWallet ShardsWallet { get; }
        public IWallet EnergyWallet { get; }
    }
}