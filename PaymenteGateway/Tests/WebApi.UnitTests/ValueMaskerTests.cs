using Xunit;
using FluentAssertions;
using WebApi.Maskers;

namespace WebApi.UnitTests
{
    public class ValueMaskerTests
    {
        ValueMasker sut;

        public ValueMaskerTests()
        {
            sut = new ValueMasker();
        }

        [Fact]
        public void should_always_return_asterisk()
        {
            string value = "hello";
            
            var response = sut.Mask(value);

            string expected = "******";
            response.Should().Be(expected);
        }
    }
}
