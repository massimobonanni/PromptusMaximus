namespace PromptusMaximus.Core.Tests.Extensions;

/// <summary>
/// Unit tests for the StringExtensions class.
/// </summary>
public class StringExtensionsTests
{
    #region Mask Tests

    [Fact]
    public void Mask_WithDefaultParameters_ShouldMaskAllButLast4Characters()
    {
        // Arrange
        var input = "1234567890123456";

        // Act
        var result = input.Mask();

        // Assert
        Assert.Equal("************3456", result);
    }

    [Fact]
    public void Mask_WithCustomMaskCharacter_ShouldUseCustomCharacter()
    {
        // Arrange
        var input = "1234567890";

        // Act
        var result = input.Mask('#', 4);

        // Assert
        Assert.Equal("######7890", result);
    }

    [Fact]
    public void Mask_WithCustomVisibleCharacters_ShouldShowCorrectNumberOfCharacters()
    {
        // Arrange
        var input = "1234567890";

        // Act
        var result = input.Mask('*', 2);

        // Assert
        Assert.Equal("********90", result);
    }

    [Fact]
    public void Mask_WithNullInput_ShouldReturnEmptyString()
    {
        // Arrange
        string? input = null;

        // Act
        var result = input.Mask();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Mask_WithEmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = input.Mask();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Mask_WithInputShorterThanVisibleCharacters_ShouldReturnOriginalString()
    {
        // Arrange
        var input = "123";

        // Act
        var result = input.Mask('*', 4);

        // Assert
        Assert.Equal("123", result);
    }

    [Fact]
    public void Mask_WithInputEqualToVisibleCharacters_ShouldReturnOriginalString()
    {
        // Arrange
        var input = "1234";

        // Act
        var result = input.Mask('*', 4);

        // Assert
        Assert.Equal("1234", result);
    }

    [Fact]
    public void Mask_WithZeroVisibleCharacters_ShouldMaskEntireString()
    {
        // Arrange
        var input = "1234567890";

        // Act
        var result = input.Mask('*', 0);

        // Assert
        Assert.Equal("**********", result);
    }

    [Fact]
    public void Mask_WithCreditCardExample_ShouldWorkCorrectly()
    {
        // Arrange
        var creditCard = "1234567890123456";

        // Act
        var masked = creditCard.Mask();

        // Assert
        Assert.Equal("************3456", masked);
    }

    #endregion

    #region MaskStringBothEnds Tests

    [Fact]
    public void MaskStringBothEnds_WithDefaultParameters_ShouldMaskMiddleCharacters()
    {
        // Arrange
        var input = "1234567890123456";

        // Act
        var result = input.MaskStringBothEnds();

        // Assert
        Assert.Equal("1234********3456", result);
    }

    [Fact]
    public void MaskStringBothEnds_WithCustomParameters_ShouldMaskCorrectly()
    {
        // Arrange
        var input = "john.doe@example.com";

        // Act
        var result = input.MaskStringBothEnds(2, 3, '*');

        // Assert
        Assert.Equal("jo***************com", result);
    }

    [Fact]
    public void MaskStringBothEnds_WithCustomMaskCharacter_ShouldUseCustomCharacter()
    {
        // Arrange
        var input = "1234567890";

        // Act
        var result = input.MaskStringBothEnds(2, 2, '#');

        // Assert
        Assert.Equal("12######90", result);
    }

    [Fact]
    public void MaskStringBothEnds_WithNullInput_ShouldReturnEmptyString()
    {
        // Arrange
        string? input = null;

        // Act
        var result = input.MaskStringBothEnds();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void MaskStringBothEnds_WithEmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = input.MaskStringBothEnds();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void MaskStringBothEnds_WithInputShorterThanTotalVisibleCharacters_ShouldReturnOriginalString()
    {
        // Arrange
        var input = "1234567";

        // Act
        var result = input.MaskStringBothEnds(4, 4);

        // Assert
        Assert.Equal("1234567", result);
    }

    [Fact]
    public void MaskStringBothEnds_WithInputEqualToTotalVisibleCharacters_ShouldReturnOriginalString()
    {
        // Arrange
        var input = "12345678";

        // Act
        var result = input.MaskStringBothEnds(4, 4);

        // Assert
        Assert.Equal("12345678", result);
    }

    [Fact]
    public void MaskStringBothEnds_WithZeroVisibleStartCharacters_ShouldMaskFromBeginning()
    {
        // Arrange
        var input = "1234567890";

        // Act
        var result = input.MaskStringBothEnds(0, 4, '*');

        // Assert
        Assert.Equal("******7890", result);
    }

    [Fact]
    public void MaskStringBothEnds_WithZeroVisibleEndCharacters_ShouldMaskToEnd()
    {
        // Arrange
        var input = "1234567890";

        // Act
        var result = input.MaskStringBothEnds(4, 0, '*');

        // Assert
        Assert.Equal("1234******", result);
    }

    [Fact]
    public void MaskStringBothEnds_WithBothZeroVisibleCharacters_ShouldMaskEntireString()
    {
        // Arrange
        var input = "1234567890";

        // Act
        var result = input.MaskStringBothEnds(0, 0, '*');

        // Assert
        Assert.Equal("**********", result);
    }

    [Fact]
    public void MaskStringBothEnds_WithEmailExample_ShouldWorkCorrectly()
    {
        // Arrange
        var email = "john.doe@example.com";

        // Act
        var masked = email.MaskStringBothEnds(2, 3);

        // Assert
        Assert.Equal("jo***************com", masked);
    }

    [Fact]
    public void MaskStringBothEnds_WithSingleCharacterVisible_ShouldMaskCorrectly()
    {
        // Arrange
        var input = "1234567890";

        // Act
        var result = input.MaskStringBothEnds(1, 1, '*');

        // Assert
        Assert.Equal("1********0", result);
    }

    #endregion
}
