namespace FingerSearchTree
{
    public static class Bounds
    {
        private static Dictionary<int, long> b = new Dictionary<int, long>();
        private static Dictionary<int, long> f = new Dictionary<int, long>();
        private static Dictionary<int, long> a = new Dictionary<int, long>();

        public static long BiP(int level)
        {
            if (b.TryGetValue(level, out long result))
                return result;
            result = 128 * (long)Math.Pow(4, level);
            b.Add(level, result);
            return result;
            //return (long)Math.Pow(2, Math.Pow(2, 2 * level + 3) - 2);
        }

        public static long Fi(int level)
        {
            if (f.TryGetValue(level, out long result))
                return result;
            result = 4 * (long)Math.Pow(2, level);
            f.Add(level, result);
            return result;
            //return (long)Math.Pow(2, Math.Pow(2, 2 * level + 1));
        }

        public static long Ai(int level)
        {
            if (a.TryGetValue(level, out long result))
                return result;
            result = (long)Math.Pow(2, level);
            a.Add(level, result);
            return result;
            //return (long)Math.Pow(2, Math.Pow(2, 2 * level));
        }
    }
}