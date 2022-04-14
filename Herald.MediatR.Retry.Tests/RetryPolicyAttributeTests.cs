using Activities.Application.Infrastructure.RetryPolicy;

using System;

using Xunit;

namespace Herald.MediatR.Retry.Tests
{
    public class RetryPolicyAttributeTests
    {
        [Fact]
        public void ShouldSetRetryCount()
        {
            //Arrange
            var attr = new RetryPolicyAttribute();

            //Act
            attr.RetryCount = 10;

            //Assert
            Assert.Equal(10, attr.RetryCount);
        }

        [Fact]
        public void ShouldSetSleepDuration()
        {
            //Arrange
            var attr = new RetryPolicyAttribute();

            //Act
            attr.SleepDuration = 10;

            //Assert
            Assert.Equal(10, attr.SleepDuration);
        }

        [Fact]
        public void ShouldThrowArgumentExeceptionWhenRetryCountLowerThanOne()
        {
            //Arrange
            var attr = new RetryPolicyAttribute();

            //Act
            Action act = () => { attr.RetryCount = 0; };

            //Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ShouldThrowArgumentExeceptionWhenSleepDurationLowerThanOne()
        {
            //Arrange
            var attr = new RetryPolicyAttribute();

            //Act
            Action act = () => { attr.SleepDuration = 0; };

            //Assert
            Assert.Throws<ArgumentException>(act);
        }
    }
}