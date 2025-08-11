
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;
using AutoAppdater.Consoles;

namespace AutoAppdater.ExternalConsoleHost
{
    public class Author
    {
        public Author(string WindowTitle)
        {

        }
    }
    internal class Host : IDisposable
    {
        struct CongifValue
        {

        }
        struct DefaultValue
        {
            public const string ConsoleHostProc_OmitPath = "";
            public const int Cue_SendSpan = 250;//ms
            public const int Mapping_MissCount = 1024;
            public const int Mapping_MemoryLen = 1;
        }
        Author Auth;
        List<DisplayInfo> Cue = [];
        MemoryMappedFile mmf;
        MemoryMappedViewAccessor acc;
        public Host(Author author)
        {
            Auth = author;
            bool passed = false;
            for (int i = 0; i < DefaultValue.Mapping_MissCount; i++)
            {
                try
                {
                    mmf = MemoryMappedFile.CreateNew(HostManager.GetMappingName(), DefaultValue.Mapping_MemoryLen);
                    passed = true;
                    break;
                }
                catch
                {

                }
            }
            if (passed && mmf != null)
            {
                acc = mmf.CreateViewAccessor();
            }
            else
            {
                throw new Exception("Failed to create channel.");
            }
            //proc
        }
        public bool CueDisplayInfo(DisplayInfo info)
        {

        }
        public void SendHost()
        {
            while (true)
            {
                if ()
                {
                    
                }
                try
                {
                    string data = JsonSerializer.Serialize(Cue[0]);
                    acc.Write(0, data.Length);
                    acc.WriteArray<char>(sizeof(int), data.ToCharArray(), 0, data.Length);
                }
                catch
                {

                }
                Task t = Task.Delay(DefaultValue.Cue_SendSpan);
                t.Wait();
            }
        }
        public void Dispose()
        {
            acc.Dispose();
            mmf.Dispose();
        }
    }
    internal static class HostManager
    {
        struct DefaultValue
        {
            public const int MappingName_Len = 32;
            public const int MappingName_MaxCount = 65536;
            public const string MappingName_Refacts = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        }
        static List<string> ExistingMapping = [];
        static Rune[] NameRefacts =
        DefaultValue.MappingName_Refacts
        .EnumerateRunes().ToArray();
        static Random Rand = new Random();
        public static string? GetMappingName()
        {
            for (int i = 0; i < DefaultValue.MappingName_MaxCount; i++)
            {
                string result = string.Concat(
                    Enumerable.Range(0, DefaultValue.MappingName_Len)
                    .Select(i => NameRefacts[Rand.Next(NameRefacts.Length)]));
                if (!ExistingMapping.Contains(result))
                {
                    ExistingMapping.Add(result);
                    return result;
                }
            }
            return null;
        }
    }
}