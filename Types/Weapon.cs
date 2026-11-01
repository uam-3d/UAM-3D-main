namespace UAM3D.Types;

public class Weapon : Item
{
    public Weapon()
    {
        this.itemClass = ItemClass.Weapon;
    }
    
    public float baseAttack;
}