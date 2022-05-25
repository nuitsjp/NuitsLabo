using System.Text.Json;

namespace UnitGeneratorStudy.Test
{
    public class UnitGeneratorTest
    {
        [Fact]
        public void TestMessage()
        {
            var message = new Message("Hello, Message!");
            var message2 = message;
            Assert.Same(message.AsPrimitive(), message2.AsPrimitive());
            Assert.True(object.ReferenceEquals(message.AsPrimitive(), message2.AsPrimitive()));
        }
    }
}