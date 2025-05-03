public interface IObserver<T>
{
    public void OnNotify(T state);
    public void PopulateNotifier();
}
