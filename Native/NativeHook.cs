using System.Runtime.InteropServices;

namespace FastfileToolkit.Native;

public class NativeLib
{
    [DllImport("Dependencies/dobby.dll")]
    public static extern unsafe int DobbyHook(void* address, void* fake_func, void** out_origin_func);

    [DllImport("Dependencies/dobby.dll")]
    public static extern unsafe int DobbyDestroy(void* address);
}

public class NativeHook<T> where T : Delegate
{
    private T _detour;
    private T _trampoline;

    private IntPtr _targetHandle;
    private IntPtr _detourHandle;
    private IntPtr _trampolineHandle;

    public IntPtr Target
    {
        get
        {
            return _targetHandle;
        }

        set
        {
            if (value == IntPtr.Zero)
                throw new ArgumentNullException("value");

            _targetHandle = value;
        }
    }

    public IntPtr Detour
    {
        get
        {
            return _detourHandle;
        }

        set
        {
            if (value == IntPtr.Zero)
                throw new ArgumentNullException("value");

            _detourHandle = value;
        }
    }

    public T Trampoline
    {
        get => _trampoline;
        private set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            _trampoline = value;
        }

    }

    public IntPtr TrampolineHandle
    {
        get => _trampolineHandle;
        private set
        {
            if (value == IntPtr.Zero)
                throw new ArgumentNullException(nameof(value));

            _trampolineHandle = value;
        }
    }

    public bool IsHooked { get; private set; }

    public unsafe NativeHook(nint target, T detour, bool autoAttach = true)
    {
        if (target == nint.Zero)
            throw new ArgumentNullException(nameof(target));

        if (detour == null)
            throw new ArgumentNullException(nameof(detour));

        _detour = detour;
        _targetHandle = target;
        _detourHandle = Marshal.GetFunctionPointerForDelegate(_detour);

        if (autoAttach)
            Attach();
    }

    public unsafe void Attach()
    {
        if (IsHooked)
            return;

        if (_targetHandle == IntPtr.Zero)
            throw new NullReferenceException("The NativeHook's target has not been set!");

        if (_detourHandle == IntPtr.Zero)
            throw new NullReferenceException("The NativeHook's detour has not been set!");

        IntPtr trampoline = IntPtr.Zero;
        NativeLib.DobbyHook(_targetHandle.ToPointer(), _detourHandle.ToPointer(), (void**)(&trampoline));
        _trampolineHandle = trampoline;

        _trampoline = (T)Marshal.GetDelegateForFunctionPointer(_trampolineHandle, typeof(T));

        IsHooked = true;
    }

    public unsafe void Detach()
    {
        if (!IsHooked)
            return;

        if (_targetHandle == IntPtr.Zero)
            throw new NullReferenceException("The NativeHook's target has not been set!");

        NativeLib.DobbyDestroy(_targetHandle.ToPointer());

        IsHooked = false;
        _trampoline = null;
        _trampolineHandle = IntPtr.Zero;
    }
}