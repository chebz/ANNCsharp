
using System;
namespace CPAI.Utils
{
    public class Util
    {
        private static Util mInstance;

        public static Util Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new Util();
                return mInstance;
            }
        }

        private Random rnd;

        private Util()
        {
            rnd = new Random();
        }

        public static void swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        public static double Random01()
        {
            return Instance.rnd.NextDouble();
        }

        public static double RandomRange(double min, double max)
        {
            if (min == max)
                return min;

            if (min > max)
                swap(ref min, ref max);

            return Random01() * (max - min) + min;
        }

        public static int Random(int max)
        {
            return Instance.rnd.Next(max);
        }

        public static int RandomRange(int min, int max)
        {
            if (min == max)
                return min;

            if (min > max)
                swap(ref min, ref max);

            return Instance.rnd.Next(min, max);
        }

        public static int RandomRangeExcept(int min, int max, int except)
        {
            if (min == max)
                return min;

            if (min > max)
                swap(ref min, ref max);

            int val = Instance.rnd.Next(min, max);

            if (val == except)
                val = (val - min + 1) % (max - min) + min;

            return val;
        }

        public static int Clamp(int value, int min, int max) { return Math.Max(min, Math.Min(value, max)); }

        public static double Clamp(double value, double min, double max) { return Math.Max(min, Math.Min(value, max)); }
    }
}
