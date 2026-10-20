using Microsoft.Win32;
using RedEngine.Services;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace RedEngine;

public partial class MainWindow : Window
{
    private const int HotkeyId = 0x5245;
    private const int WmHotkey = 0x0312;
    private const uint ModNone = 0x0000;
    private const uint VkHome = 0x24;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private HwndSource? _source;
    private bool _hotkeyRegistered;
    private readonly SettingsService _settings = new();
    private static readonly Dictionary<string, (string Description, string Status)> Modules = new()
    {
        ["Dumper"] = ("Browse accessible local files. Protected Roblox source is not extracted.", "Local file browser integration is planned."),
        ["Resources"] = ("Manage local project resources.", "Resource CRUD and previews are planned."),
        ["Menus"] = ("Authorized local development diagnostics.", "No live Roblox session is connected."),
        ["Settings"] = ("Application preferences are stored locally.", "Settings file: " + new SettingsService().SettingsPath),
        ["Misc"] = ("Utilities for local development projects.", "Additional utilities are planned."),
        ["Chat"] = ("Community chat requires a configured service.", "Disconnected: no chat backend configured.")
    };
    public MainWindow()
    {
        InitializeComponent();
        SourceInitialized += MainWindow_SourceInitialized;
        Closed += MainWindow_Closed;
        _settings.EnsureDefaults();
        Editor.Text = "-- RedEngine local Lua workspace\n-- Roblox APIs are not available in standard Lua.\n\nprint(\"Hello from local Lua\")\n";
    }
    private void Inject_Click(object sender, RoutedEventArgs e)
    {
        Footer.Text = "Inject is a UI demo only; no process injection is performed.";
    }

    private void MainWindow_SourceInitialized(object? sender, EventArgs e)
    {
        var handle = new WindowInteropHelper(this).Handle;
        _source = HwndSource.FromHwnd(handle);
        _source?.AddHook(WindowMessageHook);
        _hotkeyRegistered = RegisterHotKey(handle, HotkeyId, ModNone, VkHome);
        if (!_hotkeyRegistered) Footer.Text = "Home hotkey unavailable (already in use)";
    }

    private IntPtr WindowMessageHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WmHotkey && wParam.ToInt32() == HotkeyId)
        {
            if (IsVisible && WindowState != WindowState.Minimized) Hide();
            else { Show(); WindowState = WindowState.Normal; Activate(); }
            handled = true;
        }
        return IntPtr.Zero;
    }

    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        _source?.RemoveHook(WindowMessageHook);
        var handle = new WindowInteropHelper(this).Handle;
        if (_hotkeyRegistered) UnregisterHotKey(handle, HotkeyId);
    }

    private void LaunchRoblox_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo("roblox://") { UseShellExecute = true });
            Footer.Text = "Opening Roblox";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Roblox could not be opened. Install Roblox from its official source, then try again.\n\n" + ex.Message, "RedEngine", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void Navigate_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button b || b.Tag is not string module) return;
        HomePanel.Visibility = Visibility.Collapsed; ModulePanel.Visibility = Visibility.Visible;
        ModuleTitle.Text = module;
        ExecutorView.Visibility = module == "Executor" ? Visibility.Visible : Visibility.Collapsed;
        InfoView.Visibility = module == "Executor" ? Visibility.Collapsed : Visibility.Visible;
        if (Modules.TryGetValue(module, out var info)) { Description.Text = info.Description; Status.Text = info.Status; }
    }
    private void Home_Click(object sender, RoutedEventArgs e) { ModulePanel.Visibility = Visibility.Collapsed; HomePanel.Visibility = Visibility.Visible; Footer.Text = "Ready"; }
    private void New_Click(object sender, RoutedEventArgs e) { Editor.Clear(); Footer.Text = "New script"; }
    private void Open_Click(object sender, RoutedEventArgs e)
    {
        var d = new OpenFileDialog { Filter = "Lua/Luau scripts|*.lua;*.luau|All files|*.*" };
        if (d.ShowDialog() == true) try { Editor.Text = File.ReadAllText(d.FileName); Footer.Text = "Opened " + Path.GetFileName(d.FileName); } catch (Exception ex) { Error(ex); }
    }
    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var d = new SaveFileDialog { Filter = "Lua script|*.lua|Luau script|*.luau|All files|*.*", FileName = "NewScript.lua" };
        if (d.ShowDialog() == true) try { File.WriteAllText(d.FileName, Editor.Text); Footer.Text = "Saved " + Path.GetFileName(d.FileName); } catch (Exception ex) { Error(ex); }
    }
    private void Validate_Click(object sender, RoutedEventArgs e) { var result = ScriptValidator.ValidateBasic(Editor.Text); Output.AppendText(result + Environment.NewLine); Footer.Text = "Basic validation finished"; }
    private void Run_Click(object sender, RoutedEventArgs e)
    {
        var lua = FindLua();
        if (lua is null) { Output.AppendText("Lua runtime not found. Install Lua and add it to PATH. Roblox APIs are not provided by standard Lua.\n"); return; }
        var temp = Path.Combine(Path.GetTempPath(), "redengine-" + Guid.NewGuid().ToString("N") + ".lua");
        try
        {
            File.WriteAllText(temp, Editor.Text);
            var psi = new ProcessStartInfo(lua, $"\"{temp}\"") { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true, CreateNoWindow = true };
            using var p = Process.Start(psi); if (p is null) throw new InvalidOperationException("Could not start Lua.");
            var stdout = p.StandardOutput.ReadToEndAsync(); var stderr = p.StandardError.ReadToEndAsync();
            if (!p.WaitForExit(5000)) { p.Kill(true); Output.AppendText("Execution stopped after five seconds.\n"); }
            else { Output.AppendText(stdout.GetAwaiter().GetResult()); Output.AppendText(stderr.GetAwaiter().GetResult()); Output.AppendText($"Exit code: {p.ExitCode}\n"); }
        }
        catch (Exception ex) { Error(ex); } finally { try { File.Delete(temp); } catch { } }
    }
    private static string? FindLua()
    {
        foreach (var dir in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator))
        { var p = Path.Combine(dir.Trim('"'), OperatingSystem.IsWindows() ? "lua.exe" : "lua"); if (File.Exists(p)) return p; }
        return null;
    }
    private static void Error(Exception ex) => MessageBox.Show(ex.Message, "RedEngine", MessageBoxButton.OK, MessageBoxImage.Error);
}