using System.Collections.Generic;

namespace Utilities
{
    public static class CoinChanger
    {
        public static IEnumerable<int> GetCoins(int total, IList<int> values)
        {
            IList<int> coins = new List<int>();

            if (values.Count == 1)
            {
                for (int i = 0; i < total; i++)
                {
                    coins.Add(values[0]);
                }

                return coins;
            }

            foreach (var coin in values)
            {
                while (total >= coin)
                {
                    total -= coin;
                    coins.Add(coin);
                }
            }
            
            return coins;
        }
    }
}