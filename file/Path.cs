

namespace AutoAppdater.Path
{
    public static class Path
    {
        static string FullPath { get { return Common.Common...; } }
        public static string? GetFullPath(string omitPath)
        {
            string ret = FullPath + omitPath;
            try
            {
                return System.IO.Path.Combine(FullPath, omitPath);
            }
            catch
            {
                return null;
            }
        }
        public static string? GetOmitPath(string fullPath)
        {
            try
            {
                System.IO.Path.GetFullPath(fullPath);
                if (fullPath.Length < FullPath.Length) return null;
                if (fullPath.Substring(0, FullPath.Length) == FullPath)
                    return fullPath.Substring(FullPath.Length);
                else return null;
            }
            catch
            {
                return null;
            }
        }
    }
}