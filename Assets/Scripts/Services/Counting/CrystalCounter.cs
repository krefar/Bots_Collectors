using UnityEngine;

public class CrystalCounter : CountingBase<Crystal>
{
    public override string Title => "Кристалы";

    public void DecreaseAmount(int amount)
    {
        this.Decrease(amount);
    }
}