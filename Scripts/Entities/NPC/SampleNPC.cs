namespace UAM3D.Scripts.Entities.NPC;

public partial class SampleNPC : NPC
{
    public SampleNPC()
    {
        MaxHealth = 21;
        CurrentHealth = MaxHealth;
    }

    private void UniqueAction()
    {
        ChangeHealth(-1);
    }

    public override void Die()
    {
        QueueFree();
    }
}