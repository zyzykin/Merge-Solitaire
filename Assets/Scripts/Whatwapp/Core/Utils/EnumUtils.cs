namespace Whatwapp.Core.Utils
{
    public static class EnumUtils
    {
        public static T GetRandom<T>() where T : System.Enum
        {
            var values = (T[])System.Enum.GetValues(typeof(T));
            return values[UnityEngine.Random.Range(0, values.Length)];
        }

        public static T GetRandom<T>(T min, T max) where T : System.Enum
        {
            var values = (T[])System.Enum.GetValues(typeof(T));
            var minPos = System.Array.IndexOf(values, min);
            var maxPos = System.Array.IndexOf(values, max);
            return values[UnityEngine.Random.Range(minPos, maxPos)];
        }
    }
}