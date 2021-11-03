using API.Helpers;
using Xunit;

namespace Tests
{
    public class DroneServiceTest
    {
        [Fact]
        public void IsValidMedicationName_ValidNamePassed_ReturnsTrue()
        {
            var result = Utilities.IsValidMedicationName("3nM-_");
            Assert.True(result);
        }

        [Fact]
        public void IsValidMedicationName_InvalidNamePassed_ReturnsFalse()
        {
            var result = Utilities.IsValidMedicationName("#hff4554");
            Assert.False(result);
        }

        [Fact]
        public void IsValidMedicationCode_ValidCodePassed_ReturnsTrue()
        {
            var result = Utilities.IsValidMedicationCode("3M_");
            Assert.True(result);
        }

        [Fact]
        public void IsValidMedicationCode_InvalidCodePassed_ReturnsFalse()
        {
            var result = Utilities.IsValidMedicationCode("3nM-_");
            Assert.False(result);
        }
    }
}
