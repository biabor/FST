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
            result = (long)Math.Pow(2, Math.Pow(2, 2 * level + 3) - 2);
            b.Add(level, result);
            return result;
        }

        public static long Fi(int level)
        {
            if (f.TryGetValue(level, out long result))
                return result;
            result = (long)Math.Pow(2, Math.Pow(2, 2 * level + 1));
            f.Add(level, result);
            return result;
        }

        public static long Ai(int level)
        {
            if (a.TryGetValue(level, out long result))
                return result;
            result = (long)Math.Pow(2, Math.Pow(2, 2 * level));
            a.Add(level, result);
            return result;
        }
    }
}