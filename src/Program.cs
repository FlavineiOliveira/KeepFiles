using KeepFiles.Models;
using System.Text;
using System.Text.Json;

var log = new StringBuilder();

try
{
    log.AppendLine("Begin");
    log.AppendLine($"Date and time started: {DateTime.Now}");

    var arguments = Environment.GetCommandLineArgs();

    log.AppendLine($"Arguments: {JsonSerializer.Serialize(arguments)}");

    if ((arguments.Length - 1) != typeof(ParametersCommandLine).GetProperties().Length)
    {
        log.AppendLine("Arguments incorrects!");
        return;
    }

    var parametersCommand = new ParametersCommandLine(arguments);

    if (string.IsNullOrEmpty(parametersCommand.Path))
        parametersCommand.UpdatePath(Directory.GetCurrentDirectory());

    log.AppendLine($"Arguments to process: {JsonSerializer.Serialize(parametersCommand)}");

    var files = Directory.GetFiles(parametersCommand.Path, "*", SearchOption.AllDirectories)
                         .Where(file =>
                             (
                                !parametersCommand.IsToKeepExtensionsInformed &&
                                file.EndsWith(parametersCommand.FilesExtensions, StringComparison.OrdinalIgnoreCase)
                                ||
                                parametersCommand.IsToKeepExtensionsInformed &&
                                !file.EndsWith(parametersCommand.FilesExtensions, StringComparison.OrdinalIgnoreCase)
                             )
                             &&
                             !file.EndsWith(AppDomain.CurrentDomain.FriendlyName)
                          );

    log.AppendLine($"Total files to delete: {files.Count()}");

    foreach (var file in files)
    {
        File.Delete(file);
        log.AppendLine($"Deleted file: {file}");
    }

    var folders = Directory.GetDirectories(parametersCommand.Path, "*", SearchOption.TopDirectoryOnly);

    log.AppendLine($"Total folders to delete: {folders.Length}");

    foreach (var folder in folders)
    {
        if (Directory.GetFiles(folder, "*", SearchOption.AllDirectories).Length == 0)
        {
            Directory.Delete(folder, true);
            log.AppendLine($"Deleted folder: {folder}");
        }
    }

    log.AppendLine($"Date and time ended: {DateTime.Now}");
    log.AppendLine("End\n-----------\n");
}
catch(Exception ex)
{
    log.AppendLine($"Exception: {ex}");
}
finally
{
    File.AppendAllText(Path.Combine(Directory.GetCurrentDirectory(), "Log.txt"), log.ToString());
}