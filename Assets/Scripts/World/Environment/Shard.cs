using Entities;

namespace World.Environment
{
    public class Shard : BaseCollectable
    {
        public override void Collect(IPlayerStats playerStats)
        {
            playerStats.ShardsWallet.Add(Value);
            Destroy(gameObject);
        }
    }
}