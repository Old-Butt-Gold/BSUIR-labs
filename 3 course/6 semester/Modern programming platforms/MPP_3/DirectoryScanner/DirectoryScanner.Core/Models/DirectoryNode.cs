namespace DirectoryScanner.Core.Models;

public class DirectoryNode
{
    public string Name { get; }
    public string FullPath { get; }
    public List<FileNode> Files { get; } = [];
    public List<DirectoryNode> Subdirectories { get; } = [];
    public DirectoryNode Parent { get; set; }
    public long FileSize { get; set; }
    public long TotalSize { get; set; }

    public DirectoryNode(string fullPath)
    {
        FullPath = fullPath;
        Name = System.IO.Path.GetFileName(fullPath);
    }
}