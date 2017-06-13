namespace Drawing.Models
{
    public interface IIterator<T>
    {
        bool HasNext();

        T GetCurrent();

        void MoveNext();

        void Rest();
    }

    public interface IIteratable<T>
    {
        IIterator<T> GetIterator();

        int Length { get; }

        T GetElement(int index);
    }
}
