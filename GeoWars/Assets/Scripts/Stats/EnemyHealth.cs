namespace Stats
{
    public class EnemyHealth : Health, IDamageable
    {
        public override void TakeDamage(float damageTaken)
        {
            base.TakeDamage(damageTaken);
            //SendMessage("BecomeAwareOnTakeDamage");
        }
    }
}
