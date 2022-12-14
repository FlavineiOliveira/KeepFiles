using System.ComponentModel.DataAnnotations;

namespace KeepFiles.Models;

internal class ParametersCommandLine
{
    [Required]
    public string Path { get; private set; }

    [Required]
    public string FilesExtensions { get; }

    [Required]
    public bool IsToKeepExtensionsInformed { get; }

    [Required]
    public bool DeleteEmptiesFolder { get; }

    public ParametersCommandLine(string[] arguments)
    {
        Path = arguments.Length > 0 ? arguments[1] : null;
        FilesExtensions = arguments.Length > 1 ? arguments[2] : null;
        IsToKeepExtensionsInformed = arguments.Length > 2 ? bool.Parse(arguments[3]) : false;
        DeleteEmptiesFolder = arguments.Length > 3 ? bool.Parse(arguments[4]) : false;
    }

    public void UpdatePath(string path) => Path = path;
}
