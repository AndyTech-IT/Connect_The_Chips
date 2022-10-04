using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_The_Chips.Game
{
    public class RandomGenerator
    {
        public Random Random
        {
            get
            {
                lock (_locker)
                {
                    if (Random == null)
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
