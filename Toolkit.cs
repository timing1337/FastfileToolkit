using FastfileToolkit.Fastfiles;
using FastfileToolkit.Games;
using FastfileToolkit.Games;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace FastfileToolkit;

public class Toolkit {
    public static BaseGame Instance = null;
    public static string DumpDirectory = Path.Join(Directory.GetCurrentDirectory(), "Dumps");
    public static string CsvDirectory = Path.Join(Directory.GetCurrentDirectory(), "Hashes");

    public static Dictionary<ulong, string> HashList = new();

    static void Main() {
        Log.Logger = new LoggerConfiguration().WriteTo.Console(theme: new AnsiConsoleTheme(new Dictionary<ConsoleThemeStyle, string> {
            [ConsoleThemeStyle.Text] = "\x1b[38;5;0015m",
            [ConsoleThemeStyle.SecondaryText] = "\x1b[38;5;0007m",
            [ConsoleThemeStyle.TertiaryText] = "\x1b[38;5;0008m",
            [ConsoleThemeStyle.Invalid] = "\x1b[38;5;0011m",
            [ConsoleThemeStyle.Null] = "\x1b[38;5;0027m",
            [ConsoleThemeStyle.Name] = "\x1b[38;5;0007m",
            [ConsoleThemeStyle.String] = "\x1b[38;5;0045m",
            [ConsoleThemeStyle.Number] = "\x1b[38;2;255;165;0m",
            [ConsoleThemeStyle.Boolean] = "\x1b[38;5;0027m",
            [ConsoleThemeStyle.Scalar] = "\x1b[38;5;0085m",
            [ConsoleThemeStyle.LevelVerbose] = "\x1b[38;5;0007m",
            [ConsoleThemeStyle.LevelDebug] = "\x1b[38;5;0007m",
            [ConsoleThemeStyle.LevelInformation] = "\x1b[38;5;0015m",
            [ConsoleThemeStyle.LevelWarning] = "\x1b[38;5;0011m",
            [ConsoleThemeStyle.LevelError] = "\x1b[38;5;0015m\x1b[48;5;0196m",
            [ConsoleThemeStyle.LevelFatal] = "\x1b[38;5;0015m\x1b[48;5;0196m",
        })).CreateLogger();


        if (!Directory.Exists(DumpDirectory))
        {
            Directory.CreateDirectory(DumpDirectory);
        }

        if (Directory.Exists(CsvDirectory))
        {
            var csvs = Directory.GetFiles(CsvDirectory, "*.csv", SearchOption.AllDirectories);
            foreach (var csv in csvs)
            {
                var content = File.ReadAllLines(csv);
                foreach(var line in content)
                {
                    var splited = line.Split(',');
                    HashList[Convert.ToUInt64(splited[0], 16)] = splited[1];
                }
            }
            Log.Information("Finished loading all {0} hashes", HashList.Count);
        }

        // Read user path 
#if DEBUG
        Instance = new BlackOps7(@"E:\depots\1938091\20886482");
#else
        Log.Information("Enter your path: ");
        var path = Console.ReadLine();
        if (!Directory.Exists(path))
        {
            Log.Error("Game path doesn't exist");
            Environment.Exit(0);
            return;
        }
        Instance = new BlackOps7(path);
#endif

        var fastfiles = Directory.GetFiles(Instance.GamePath, "*.ff", SearchOption.AllDirectories);
        Log.Information("Found {count} fastfiles in {path}", fastfiles.Length, Instance.GamePath);
        foreach (var fastfile in fastfiles) {
            var zoneName = Path.GetFileNameWithoutExtension(fastfile);
            Log.Information("Loading zone {zone}", zoneName);
            Instance.LoadZone(zoneName);
        }

        Dictionary<string, StringBuilder> results = new();

        foreach (var entry in Instance.PotentialHashes)
        {
            var hash = entry.Key;
            var str = entry.Value;
            if (HashList.ContainsKey(hash)) continue;

            string? assetType = null;

            if (entry.Value.StartsWith("j_") || entry.Value.StartsWith("tag_"))
            {
                assetType = "bone";
            }else if (Instance.LoadedAssets.TryGetValue(hash, out var loadedAsset))
            {
                assetType = Instance.AssetTypes[loadedAsset.Type];
            }

            if(assetType != null)
            {
                if (!results.ContainsKey(assetType))
                {
                    results[assetType] = new StringBuilder();
                }

                var sb = results[assetType];
                sb.Append(hash.ToString("X").ToLower());
                sb.Append(',');
                sb.Append(str);
                sb.Append('\n');
            }
        }

        var finalSb = new StringBuilder();

        foreach(var sb in results)
        {
            finalSb.AppendLine("# " + sb.Key);
            finalSb.Append(sb.Value);
            finalSb.Append("\n");
        }

        File.WriteAllText(Path.Join(DumpDirectory, "newHash.txt"), finalSb.ToString());
    }
}