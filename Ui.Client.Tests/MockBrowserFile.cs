using Microsoft.AspNetCore.Components.Forms;

namespace Ui.Client.Tests;

public sealed class MockBrowserFile(string name, string contentType, string content) : IBrowserFile
{
    public string ContentType { get; } = contentType;

    public DateTimeOffset LastModified => DateTimeOffset.Now;

    public string Name { get; } = name;

    public long Size { get; } = content.Length;

    public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
    {
        return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
    }
}