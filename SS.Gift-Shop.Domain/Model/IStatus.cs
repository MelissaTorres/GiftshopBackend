namespace SS.GiftShop.Domain.Model
{
    public interface IStatus<T>
    {
        T Status { get; set; }
    }
}