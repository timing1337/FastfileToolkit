using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Diagnostics.Debug;
using Windows.Win32.System.LibraryLoader;
using Windows.Win32.System.Memory;

namespace FastfileToolkit;

public class Module {
    public SafeHandle Handle { get; private set; }
    public nint BaseAddress => Handle.DangerousGetHandle();

    public static unsafe Module Load(string executable, string folderPath) {
        Module module = new Module();
        PInvoke.SetDllDirectory(folderPath);
        module.Handle = PInvoke.LoadLibrary(executable);
        Log.Information("Loaded {Executable} at {BaseAddress:X}", executable, module.BaseAddress);
        return module;
    }
}