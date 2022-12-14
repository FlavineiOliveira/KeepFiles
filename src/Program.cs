using KeepFiles.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

var log = new StringBuilder();

try
{
    log.AppendLine("Begin");
    log.AppendLine($"Date and time started: {DateTime.Now}");

    var arguments = Environment.GetCommandLineArgs();

    var parametersCommand = new ParametersCommandLine(arguments);

    var requiredProperties = parametersCommand
                            .GetType()
                            .GetProperties()
                            .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(RequiredAttribute)));

    if (requiredProperties.Count() > arguments.Length)
    {
        log.AppendLine("Required arguments not informed!");
        return;
    }

    if (string.IsNullOrEmpty(parametersCommand.Path))
        parametersCommand.UpdatePath(Directory.GetCurrentDirectory());

    log.AppendLine($"Arguments: {JsonSerializer.Serialize(parametersCommand)}");

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
        if (Directory.GetFiles(folder, "*", SearchOption.AllDirectories).Count() == 0)
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