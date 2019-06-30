using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Xunit;

namespace FriendlyStudy.Test
{
    public class MainWindowTest
    {
        [Fact]
        public void TestCountUp()
        {
            using (var app = AppProxy.Run())
            {
                Assert.NotNull(app);

                var mainWindow = app.MainWindow;

                Assert.NotNull(mainWindow);
                Assert.Equal("0", mainWindow.CounterText);

                mainWindow.CountUp();

                Assert.Equal("1", mainWindow.CounterText);
            }
        }
    }
}