using Entities;

namespace World.Environment
{
    public class Rosary : BaseCollectable
    {
        public override void Collect(IPlayerStats playerStats)
        {
            playerStats.CoinsWallet.Add(Value);
            Destroy(gameObject);
        }
    }
}