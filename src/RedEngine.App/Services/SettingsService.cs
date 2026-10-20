using System.Text.Json;
namespace RedEngine.Services;
public sealed class SettingsService
{
    public string SettingsPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RedEngine", "settings.json");
    public void EnsureDefaults()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
        if (File.Exists(SettingsPath)) return;
        var defaults = new { theme = "RedEngine", editorFont = "Consolas", editorFontSize = 14, autosave = true, streamHideEnabled = false, chatBackendConfigured = false };
        File.WriteAllText(SettingsPath, JsonSerializer.Serialize(defaults, new JsonSerializerOptions { WriteIndented = true }));
    }
}