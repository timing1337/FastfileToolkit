using FastfileToolkit.Fastfiles;
using FastfileToolkit.Games;
using FastfileToolkit.Games;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace FastfileToolkit;

public class Toolkit {
    public static BaseGame Instance = null;
    public static string DumpDirectory = Path.Join(Directory.GetCurrentDirectory(), "Dumps");

    static void Main() {
        if (!Directory.Exists(DumpDirectory)) {
            Directory.CreateDirectory(DumpDirectory);
        }

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
        Instance = new BlackOps6(@"D:\SteamDownloader\BlackOps6\cod24");

        //get all fastfile files in the game directory
        var fastfiles = Directory.GetFiles(Instance.GamePath, "*.ff", SearchOption.AllDirectories);
        Log.Information("Found {count} fastfiles in {path}", fastfiles.Length, Instance.GamePath);
        foreach (var fastfile in fastfiles) {
            var zoneName = Path.GetFileNameWithoutExtension(fastfile);
            if (zoneName.Length >= 20) continue;
            Log.Information("Loading zone {zone}", zoneName);
            Instance.LoadZone(zoneName);
        }
    }
}