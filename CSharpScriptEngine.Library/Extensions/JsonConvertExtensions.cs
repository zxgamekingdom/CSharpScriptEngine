using Newtonsoft.Json;

namespace CSharpScriptEngine.Library.Extensions
{
    public static class JsonConvertExtensions
    {
        public static string JsonSerialize<T>(this T obj,
            Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }

        public static T? JsonDeserialize<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
