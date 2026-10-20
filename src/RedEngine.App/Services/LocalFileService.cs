using System.IO;
using RedEngine.Models;

namespace RedEngine.Services;

public sealed class LocalFileService
{
    public string DataRoot { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RedEngine");
    public string ScriptsRoot => Path.Combine(DataRoot, "Local", "Scripts");

    public IEnumerable<LocalFileItem> Index(string root, string search = "")
    {
        if (!Directory.Exists(root)) yield break;
        var pending = new Stack<string>(); pending.Push(root);
        while (pending.Count > 0)
        {
            var current = pending.Pop(); string[] dirs; string[] files;
            try { dirs = Directory.GetDirectories(current); files = Directory.GetFiles(current); }
            catch (UnauthorizedAccessException) { continue; }
            catch (IOException) { continue; }
            foreach (var dir in dirs) pending.Push(dir);
            foreach (var file in files)
            {
                FileInfo info;
                try { info = new FileInfo(file); if (!info.Name.Contains(search, StringComparison.OrdinalIgnoreCase)) continue; }
                catch (IOException) { continue; }
                yield return new LocalFileItem(info.Name, info.Extension, FormatSize(info.Length), info.LastWriteTime.ToString("yyyy-MM-dd HH:mm"), info.FullName);
            }
        }
    }

    public string Import(string source)
    {
        if (!File.Exists(source)) throw new FileNotFoundException("Source file no longer exists.", source);
        Directory.CreateDirectory(ScriptsRoot);
        var name = Path.GetFileName(source); var destination = Path.Combine(ScriptsRoot, name);
        if (File.Exists(destination)) destination = Path.Combine(ScriptsRoot, $"{Path.GetFileNameWithoutExtension(name)}-{DateTime.Now:yyyyMMdd-HHmmss}-{Guid.NewGuid():N}{Path.GetExtension(name)}");
        File.Copy(source, destination, false); return destination;
    }

    private static string FormatSize(long size) => size switch { < 1024 => $"{size} B", < 1024 * 1024 => $"{size / 1024d:0.0} KB", _ => $"{size / (1024d * 1024d):0.0} MB" };
}