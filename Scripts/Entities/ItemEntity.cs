namespace UAM3D.Scripts.Entities;

public partial class ItemEntity : Entity
{
    public ItemEntity( bool damageable = true, float customMaxHealth = -1 )
    {
        Invincible = damageable;
        MaxHealth = customMaxHealth > 0 
            ? customMaxHealth
            : 1;
        CurrentHealth = MaxHealth;
    }

    public void Kill()
    {
        QueueFree();
    }
}