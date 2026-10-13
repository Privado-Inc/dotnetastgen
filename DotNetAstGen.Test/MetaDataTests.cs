namespace DotNetAstGen.Test;

public class MetaDataTests
{
    [Fact]
    public void NoCommentsFromPreviousLineBeforeInvocations()
    {
        const string code = """
                            // This is a comment
                            System.Console.WriteLine(0);
                            """;

        Assert.True(Program.TryAstForString("test.cs", code, out var jsonString));
        Assert.DoesNotContain(@"""Code"": ""// This is a comment\r\nSystem.Console.WriteLine(0)""", jsonString);
    }

    [Fact]
    public void NoCommentsFromSameLineBeforeInvocations()
    {
        const string code = """
                            /* Comment */System.Console.WriteLine(0);
                            """;

        Assert.True(Program.TryAstForString("test.cs", code, out var jsonString));
        Assert.DoesNotContain(@"""Code"": ""/* Comment */System.Console.WriteLine(0);""", jsonString);
    }

    [Fact]
    public void CancelsParsingWhenTheFileTimeoutExpires()
    {
        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        Assert.Throws<OperationCanceledException>(() =>
            Program.TryAstForString("test.cs", "class Example { }", cancellationSource.Token, out _));
    }
}
