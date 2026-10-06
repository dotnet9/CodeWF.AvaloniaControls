using CodeWF.AvaloniaControls.Text;

namespace CodeWF.AvaloniaControls.Tests;

/// <summary>
/// 自动配对（纯逻辑）：默认成对插入、选区包裹、右半边跳过、空配对退格删除与关闭开关。
/// </summary>
public sealed class TextAutoPairTests
{
    [Fact]
    public void HandleTextInput_WhenTypingOpeningBracket_InsertsClosingAndPlacesCaretInside()
    {
        var result = TextAutoPair.HandleTextInput("ab", 1, 0, "(");

        Assert.NotNull(result);
        Assert.Equal(1, result!.Value.Start);
        Assert.Equal(0, result.Value.Length);
        Assert.Equal("()", result.Value.Text);
        Assert.Equal(2, result.Value.CaretOffset);
    }

    [Fact]
    public void HandleTextInput_WhenSelectionExists_WrapsSelectionWithPair()
    {
        var result = TextAutoPair.HandleTextInput("hello world", 6, 5, "[");

        Assert.NotNull(result);
        Assert.Equal(6, result!.Value.Start);
        Assert.Equal(5, result.Value.Length);
        Assert.Equal("[world]", result.Value.Text);
        Assert.Equal(13, result.Value.CaretOffset);
    }

    [Fact]
    public void HandleTextInput_WhenSelectionExistsAndKeyCannotWrap_ReturnsNull()
    {
        Assert.Null(TextAutoPair.HandleTextInput("hi", 0, 2, "`"));
    }

    [Fact]
    public void HandleTextInput_WhenClosingAlreadyFollows_SkipsInsteadOfDuplicating()
    {
        var result = TextAutoPair.HandleTextInput("()", 1, 0, ")");

        Assert.NotNull(result);
        Assert.Equal(1, result!.Value.Start);
        Assert.Equal(0, result.Value.Length);
        Assert.Equal(string.Empty, result.Value.Text);
        Assert.Equal(2, result.Value.CaretOffset);
    }

    [Fact]
    public void HandleTextInput_WhenPlainCharacter_ReturnsNull()
    {
        Assert.Null(TextAutoPair.HandleTextInput("ab", 1, 0, "x"));
    }

    [Fact]
    public void HandleBackspace_WhenCaretIsBetweenEmptyPair_DeletesBothHalves()
    {
        var result = TextAutoPair.HandleBackspace("ab()cd", 3, 0);

        Assert.NotNull(result);
        Assert.Equal(2, result!.Value.Start);
        Assert.Equal(2, result.Value.Length);
        Assert.Equal(string.Empty, result.Value.Text);
        Assert.Equal(2, result.Value.CaretOffset);
    }

    [Fact]
    public void HandleBackspace_WhenPairIsNotEmpty_ReturnsNull()
    {
        Assert.Null(TextAutoPair.HandleBackspace("(x)", 2, 0));
    }

    [Fact]
    public void HandleBackspace_WhenSelectionExists_ReturnsNull()
    {
        Assert.Null(TextAutoPair.HandleBackspace("abc", 1, 1));
    }

    [Fact]
    public void Options_WhenDeleteEmptyPairDisabled_ReturnsNull()
    {
        var options = new TextAutoPairOptions { DeleteEmptyPairOnBackspace = false };

        Assert.Null(TextAutoPair.HandleBackspace("()", 1, 0, options));
    }
}
