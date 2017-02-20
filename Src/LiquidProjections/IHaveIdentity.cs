namespace LiquidProjections
{
    public interface IHaveIdentity<T>
    {
        T Id { get; set; }
    }
}