namespace Whatwapp.Core.Extensions
{
    public static class EnumExtensions
    {
        
        public static bool IsFirst<T>(this T src) where T : System.Enum
        {
            var values = (T[])System.Enum.GetValues(src.GetType());
            return System.Array.IndexOf(values, src) == 0;
        }
        
        public static bool IsLast<T>(this T src) where T : System.Enum
        {
            var values = (T[])System.Enum.GetValues(src.GetType());
            return System.Array.IndexOf(values, src) == values.Length - 1;
        }
        
        public static bool HasNext<T>(this T src) where T : System.Enum
        {
            var values = (T[])System.Enum.GetValues(src.GetType());
            return System.Array.IndexOf(values, src) < values.Length - 1;
        }
        
        public static bool HasPrevious<T>(this T src) where T : System.Enum
        {
            var values = (T[])System.Enum.GetValues(src.GetType());
            return System.Array.IndexOf(values, src) > 0;
        }
        

        public static T Next<T>(this T src, bool circular = false) where T : System.Enum
        {
            var values = (T[])System.Enum.GetValues(src.GetType());
            var j = System.Array.IndexOf(values, src) + 1;
            if (j == values.Length)
            {
                return (circular) ? values[0] : values[j - 1];
            }
            return values[j];
        }
        
        public static T Previous<T>(this T src, bool circular = false) where T : System.Enum
        {
            var values = (T[])System.Enum.GetValues(src.GetType());
            var j = System.Array.IndexOf(values, src) - 1;
            if (j < 0)
            {
                return (circular) ? values[^1] : values[j + 1];
            }
            return values[j];
        }
        
        
    }
}