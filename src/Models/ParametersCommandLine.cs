using System.ComponentModel.DataAnnotations;

namespace KeepFiles.Models;

internal class ParametersCommandLine
{
    public string Path { get; private set; }

    public string FilesExtensions { get; }

    public bool IsToKeepExtensionsInformed { get; }

    public bool DeleteEmptiesFolder { get; }

    public ParametersCommandLine(string[] arguments)
    {
        Path = arguments.Length > 0 ? arguments[1] : null;
        FilesExtensions = arguments.Length > 1 ? arguments[2] : null;
        IsToKeepExtensionsInformed = arguments.Length > 2 && bool.Parse(arguments[3]);
        DeleteEmptiesFolder = arguments.Length > 3 && bool.Parse(arguments[4]);
    }

    public void UpdatePath(string path) => Path = path;
}
