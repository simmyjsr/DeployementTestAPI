namespace InventoryAPI.Exceptions
{
    public class DuplicateDataException : System.Exception
    {
        public DuplicateDataException() { }
        public DuplicateDataException(string message) : base(message) { }
        public DuplicateDataException(string message, System.Exception inner) : base(message, inner) { }
    }
}
