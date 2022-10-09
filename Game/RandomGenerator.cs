using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_The_Chips.Game
{
    public class RandomGenerator
    {
        public int Next_Integer(int max_value)
        {
            return Random.Next(max_value);
        }

        public int Next_Integer(int min_value, int max_value)
        {
            return Random.Next(min_value, max_value);
        }

        public T Next_Item<T>(T[] source)
        {
            return source[Random.Next(source.Length)];
        }

        public int Next_Index<T>(T[] source)
        {
            return Random.Next(source.Length);
        }

        public T[] Next_Items<T>(T[] source, int count)
        {
            if (count == 0)
                return new T[0];
            if (count == source.Length)
                return source;
            if (count > source.Length)
                throw new Exception($"Source array size less then {count}!");

            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
                int index = Next_Index(source);
                result[i] = source[index];
                source = source.Take(index).Union(source.Skip(index+1)).ToArray();
            }
            return result;
        }

        private Random Random
        {
            get
            {
                lock (_locker)
                {
                    if (_generator == null)
                        throw new InvalidOperationException("Not inited!");

                    if (++_counter >= WASHING_VALUE)
                    {
                        _generator = new Random();
                        _counter = 0;
                    }
                    return _generator;
                }
            }
        }

        private const int WASHING_VALUE = 1000;

        private Random _generator;
        private int _counter;
        private readonly object _locker;

        public RandomGenerator()
        {
            _generator = new Random();
            _counter = 0;
            _locker = new object();
        }
    }
}
