namespace Entities
{
    public interface IDamageable
    {
        public void ApplyDamage(int damage);

        public void Kill();
    }
}