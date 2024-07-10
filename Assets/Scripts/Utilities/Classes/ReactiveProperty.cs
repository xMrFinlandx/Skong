namespace Utilities.Classes
{
    public class ReactiveProperty<T>
    {
        private T _value;

        public delegate void ChangedDelegate(T previous, T current);

        public event ChangedDelegate ValueChangedEvent;

        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value))
                    return;
                
                ValueChangedEvent?.Invoke(_value, value);
                _value = value;
            }
        }

        public ReactiveProperty()
        {
        }

        public ReactiveProperty(T value)
        {
            _value = value;
        }
    }
}