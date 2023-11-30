namespace GettingStarted
{
    public partial class App : Application
    {
        const int WindowWidth = 600;

        const int WindowHeight = 800;
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
            {
#if WINDOWS
                        var mauiWindow = handler.VirtualView;
                var nativeWindow = handler.PlatformView;
                nativeWindow.Activate();
                IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                appWindow.Resize(new Windows.Graphics.SizeInt32(WindowWidth, WindowHeight));
                nativeWindow.ExtendsContentIntoTitleBar = false;
#endif
            });
        }
    }
}
