using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Weather.Classes
{
    //метод расширения, ссылающийся на статическое поле Random для получения случайного номера элемента из списка
    public static class RandomExt
    {
        private static Random rnd = new Random();

        public static T PickRandom<T>(this IList<T> source)
        {
            int randIndex = rnd.Next(source.Count);
            return source[randIndex];
        }
    }
}
