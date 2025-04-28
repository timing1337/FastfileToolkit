using FastfileToolkit.Fastfiles;
using FastfileToolkit.Games;
using Serilog;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace FastfileToolkit;

public class Toolkit
{

    public static ModernWarfare4 Instance = null;

    public static string DumpDirectory = Path.Join(Directory.GetCurrentDirectory(), "Dumps");

    static void Main()
    {
        if (Directory.Exists(DumpDirectory))
        {
            Directory.CreateDirectory(DumpDirectory);
        }

        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

        Instance = new ModernWarfare4(@"D:\SteamDownloader\Client\cod22");
        Instance.LoadZone("global");
    }
}