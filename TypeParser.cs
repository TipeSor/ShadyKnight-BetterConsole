namespace BetterConsole
{
    public abstract class TypeParser<T>
    {
        public abstract bool TryParse(string input, out T value);
    }
}
