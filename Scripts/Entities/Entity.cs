using Godot;

namespace UAM3D.Scripts.Entities;

/// <summary>
/// This class is the base of all entities, from which they inherit. Data and Methods that apply to all derivatives
/// should be stored here.
/// </summary>
public partial class Entity : Node3D
{
    protected float MaxHealth { get; set; }
    protected float CurrentHealth { get; set; }
    protected bool Invincible { get; set; }

    public virtual void SetHealth(float health)
    {
        if (Invincible) return;

        CurrentHealth = Mathf.Clamp(health, 0, MaxHealth);
    }

    public virtual void ChangeHealth(float dHealth) => SetHealth(CurrentHealth + dHealth);

    public virtual void Die()
    {
    }
}