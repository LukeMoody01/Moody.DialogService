using FluentAssertions;
using Moody.Core.Services;
using Moody.Core.Settings;
using Xunit;

namespace Moody.Tests
{
    public class DialogServiceTests : UnitTestBase<DialogService>
    {
        [Fact]
        public void GetReturnParameters_ShouldReturnExpectedValue_WhenCalled()
        {
            //Arrange
            var expectedResult = true;
            Sut.ReturnParameters = true;

            //Act
            var actualResult = Sut.GetReturnParameters<bool>();

            //Assert
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public void SetReturnParameters_ShouldSetReturnParameters_WhenCalled()
        {
            //Arrange
            var expectedResult = true;
            Sut.ReturnParameters = false;

            //Act
            Sut.SetReturnParameters(true);

            //Assert
            Sut.ReturnParameters.Should().Be(expectedResult);
        }

        [Fact]
        public void GetDefaultDialogSettings_ShouldReturnExpectedSettings_WhenCalled()
        {
            //Arrange
            var expectedDefaultDialogSettings = new DefaultDialogSettings()
            {
                DialogWindowDefaultStyle = System.Windows.WindowStyle.ThreeDBorderWindow,
                DialogWindowDefaultTitle = "Expected Title",
            };

            Sut.Settings = expectedDefaultDialogSettings;

            //Act
            var actualResult = Sut.GetDefaultDialogSettings();

            //Assert
            actualResult.Should().Be(expectedDefaultDialogSettings);
        }

        [Fact]
        public void SetDefaultDialogSettings_ShouldSetSettings_WhenCalled()
        {
            //Arrange
            var expectedDefaultDialogSettings = new DefaultDialogSettings()
            {
                DialogWindowDefaultStyle = System.Windows.WindowStyle.ThreeDBorderWindow,
                DialogWindowDefaultTitle = "Expected Title",
            };

            //Act
            Sut.SetDefaultDialogSettings(expectedDefaultDialogSettings);

            //Assert
            Sut.Settings.Should().Be(expectedDefaultDialogSettings);
        }
    }
}