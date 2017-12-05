using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Experimentation.FeatureSwitches.Tests
{
    [TestFixture]
    public class Sandbox
    {
        private FeatureSwitch _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            var dict = new Dictionary<string, string>
            {
                {"ApiName", "ExperimentationApi"},
                {"MachineName", "PRSQ02"},
                {"PortNumber", "80"},
            };

            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(dict);

            _sut = new FeatureSwitch(builder.Build());
        }

        [Test, Category("Manual_Integration")]
        public async Task AsyncShouldReturnTrue()
        {
            var result = await _sut.IsSwitchEnabledAsync("PureApi_SearchExtensionWork");
            result.Should().BeTrue();
        }

        [Test, Category("Manual_Integration")]
        public void SyncShouldReturnTrue()
        {
            var result = _sut.IsSwitchEnabled("PureApi_SearchExtensionWork");
            result.Should().BeTrue();
        }
        
        [Test, Category("Manual_Integration")]
        public void SyncShouldReturnTrue_Bucket()
        {
            var result = _sut.IsPermittedOnSwitch("5a201443bdbc812050b07409", "RomansAgeId");
            result.Should().BeTrue();
        }
        
        [Test, Category("Manual_Integration")]
        public void SyncShouldReturnFalse_Bucket()
        {
            var result = _sut.IsPermittedOnSwitch("5a201443bdbc812050b07409", "Id");
            result.Should().BeFalse();
        }
        
        [Test, Category("Manual_Integration")]
        public void SyncShouldReturnTrue_Overriden()
        {
            _sut.OverrideSwitch("PureApi_SearchExtensionWork");
            var result = _sut.IsSwitchEnabled("PureApi_SearchExtensionWork");
            result.Should().BeTrue();
        }
    }
}
