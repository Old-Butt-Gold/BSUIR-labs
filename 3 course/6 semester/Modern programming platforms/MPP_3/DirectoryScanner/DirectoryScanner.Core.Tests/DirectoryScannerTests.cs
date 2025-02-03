namespace DirectoryScanner.Core.Tests;

public class DirectoryScannerTests
{
    private readonly string _testRoot = Path.Combine(Path.GetTempPath(), "DirectoryScannerTests");

    public DirectoryScannerTests()
    {
        Directory.CreateDirectory(_testRoot);
    }

    private void CleanupTestDir()
    {
        if (Directory.Exists(_testRoot))
            Directory.Delete(_testRoot, true);
    }

    [Fact]
    public void Scan_EmptyDirectory_ReturnsValidStructure()
    {
        // Arrange
        var scanner = new DirectoryScanner();
        var dirPath = Path.Combine(_testRoot, "EmptyDir");
        Directory.CreateDirectory(dirPath);

        // Act
        var result = scanner.Scan(dirPath, CancellationToken.None);

        // Assert
        Assert.Equal(dirPath, result.FullPath);
        Assert.Empty(result.Files);
        Assert.Empty(result.Subdirectories);
        Assert.Equal(0L, result.TotalSize);

        CleanupTestDir();
    }

    [Fact]
    public void Scan_DirectoryWithFiles_CalculatesSizesCorrectly()
    {
        // Arrange
        var scanner = new DirectoryScanner();
        var dirPath = Path.Combine(_testRoot, "FilesDir");
        Directory.CreateDirectory(dirPath);
            
        File.WriteAllText(Path.Combine(dirPath, "file1.txt"), "test");
        File.WriteAllText(Path.Combine(dirPath, "file2.txt"), "content");

        // Act
        var result = scanner.Scan(dirPath, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Files.Count);
        Assert.Equal(11L, result.TotalSize); // 4 + 7 bytes

        CleanupTestDir();
    }

    [Fact]
    public void Scan_NestedDirectories_CalculatesHierarchyCorrectly()
    {
        // Arrange
        var scanner = new DirectoryScanner();
        var dirPath = Path.Combine(_testRoot, "NestedDir");
        Directory.CreateDirectory(dirPath);
            
        var subDir = Path.Combine(dirPath, "SubDir");
        Directory.CreateDirectory(subDir);
            
        File.WriteAllText(Path.Combine(dirPath, "root.txt"), "test");
        File.WriteAllText(Path.Combine(subDir, "sub.txt"), "content");

        // Act
        var result = scanner.Scan(dirPath, CancellationToken.None);

        // Assert
        Assert.Equal(1, result.Files.Count);
        Assert.Equal(1, result.Subdirectories.Count);
        Assert.Equal(11L, result.TotalSize); // 4 + 7 bytes

        var subDirNode = result.Subdirectories[0];
        Assert.Equal(1, subDirNode.Files.Count);
        Assert.Equal(7L, subDirNode.TotalSize);

        CleanupTestDir();
    }

    [Fact]
    public void Scan_WithSymbolicLink_IgnoresLinkedContent()
    {
        // Arrange
        var scanner = new DirectoryScanner();
        var dirPath = Path.Combine(_testRoot, "LinkDir");
        Directory.CreateDirectory(dirPath);
            
        var realFile = Path.Combine(dirPath, "real.txt");
        File.WriteAllText(realFile, "content");
            
        var linkPath = Path.Combine(dirPath, "link.txt");
        File.CreateSymbolicLink(linkPath, realFile);

        // Act
        var result = scanner.Scan(dirPath, CancellationToken.None);

        // Assert
        Assert.Equal(1, result.Files.Count);
        Assert.Equal(7L, result.TotalSize);

        CleanupTestDir();
    }

    [Fact]
    public void Scan_Cancellation_ReturnsPartialResults()
    {
        // Arrange
        var scanner = new DirectoryScanner();
        var cts = new CancellationTokenSource();
        var dirPath = Path.Combine(_testRoot, "BigDir");
        Directory.CreateDirectory(dirPath);

        // Create 100 files
        for (int i = 0; i < 1000; i++)
            File.WriteAllText(Path.Combine(dirPath, $"file{i}.txt"), "content");

        // Act
        cts.CancelAfter(1);
        var result = scanner.Scan(dirPath, cts.Token);

        // Assert
        Assert.True(result.Files.Count < 1000);
        Assert.True(result.TotalSize < 7000L); // 100 files × 7 bytes

        CleanupTestDir();
    }
}