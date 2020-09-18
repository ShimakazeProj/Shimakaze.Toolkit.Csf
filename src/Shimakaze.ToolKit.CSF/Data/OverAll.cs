namespace Shimakaze.ToolKit.Csf.Data
{
    public static class OverAll
    {
        public static string GetResource(this string key) => Properties.Resource.ResourceManager.GetString(key);


    }
}