public class CrystalCounter : CountingBase<Crystal>
{
    public override string Title => "Кристаллы";

    public void IncreaseAmount(int amount)
    {
        this.Increase(amount);
    }
    
    public void DecreaseAmount(int amount)
    {
        this.Decrease(amount);
    }
}