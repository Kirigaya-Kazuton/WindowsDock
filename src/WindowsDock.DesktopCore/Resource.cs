using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Markup;

namespace DesktopCore
{
    public static class Resources
    {
        private static Dictionary<string, string> _resources = new();
        private static string _basePath = "Resources/Resources";
        private static CultureInfo _currentCulture = CultureInfo.CurrentCulture;

        public static string Get(string key)
        {
            if (_resources.TryGetValue($"{_currentCulture.Name}.{key}", out var value) ||
                _resources.TryGetValue(key, out value))
                return value;
            return key;
        }

        public static void Load(string basePath)
        {
            _basePath = basePath;
            LoadFile($"{basePath}.txt");
            var cultureSuffix = _currentCulture.Name;
            LoadFile($"{basePath}_{cultureSuffix}.txt");
            var shortSuffix = cultureSuffix.Split('-')[0];
            if (shortSuffix != cultureSuffix)
                LoadFile($"{basePath}_{shortSuffix}.txt");
        }

        private static void LoadFile(string path)
        {
            if (!File.Exists(path)) return;
            foreach (var line in File.ReadAllLines(path))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#") || trimmed.StartsWith(";"))
                    continue;
                var eq = trimmed.IndexOf('=');
                if (eq > 0)
                {
                    var key = trimmed[..eq].Trim();
                    var value = trimmed[(eq + 1)..].Trim();
                    _resources[$"{_currentCulture.Name}.{key}"] = value;
                    if (!_resources.ContainsKey(key))
                        _resources[key] = value;
                }
            }
        }

        public static void ReProvideAll()
        {
            // Trigger any bound elements to refresh
        }
    }

    public class Resource : MarkupExtension
    {
        public string Key { get; set; }

        public Resource(string key)
        {
            Key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Resources.Get(Key);
        }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class ResourceAttribute : Attribute
    {
        public string Key { get; }
        public ResourceAttribute(string key) => Key = key;
    }

    public static class ResourceExtensions
    {
        public static string GetResourceDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<ResourceAttribute>();
            return attr != null ? Resources.Get(attr.Key) : value.ToString();
        }
    }
}
