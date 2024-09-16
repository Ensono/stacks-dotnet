using System;
using System.IO;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration.Exceptions;
using AutoFixture.Xunit2;
using Xunit;
using Xunit.Abstractions;

namespace xxENSONOxx.xxSTACKSxx.Shared.Configuration.Tests
{
    [Trait("TestType", "IntegrationTests")]
    public class SecretTests
    {
        private readonly ITestOutputHelper output;
        SampleConfigurationWithSecret config;

        public SecretTests(ITestOutputHelper output)
        {
            this.output = output;
            config = Configuration.For<SampleConfigurationWithSecret>("SecretConfigurationTest");

            Environment.SetEnvironmentVariable("itsasecret", "PleaseDontTellAnyone");
        }


        [Theory, AutoData]
        public async Task EnvironmentBasedSecretTest(SecretResolver secretResolver)
        {
            var secret = await secretResolver.ResolveSecretAsync(config.EnvironmentSecret);

            Assert.Equal("PleaseDontTellAnyone", secret);
            Assert.Equal("TestString", config.TextValue);
        }

        [Theory, AutoData]
        public async Task ImplicitEnvironmentBasedSecretTest(SecretResolver secretResolver)
        {
            var secret = await secretResolver.ResolveSecretAsync(config.ImplicitEnvironmentSecret);

            Assert.Equal("PleaseDontTellAnyone", secret);
            Assert.Equal("TestString", config.TextValue);
        }

        [Theory, AutoData]
        public async Task FilenameFileSecretTest(SecretResolver secretResolver)
        {
            var secret = await secretResolver.ResolveSecretAsync(config.FilenameFileSecret);

            Assert.Equal("C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", secret);
            Assert.Equal("TestString", config.TextValue);
        }

        [Theory, AutoData]
        public async Task RelativeFilenameFileSecretTest(SecretResolver secretResolver)
        {
            var tmpFile = "subfolder/secretfile.txt";

            Directory.CreateDirectory("subfolder");

            File.Copy("secretfile.txt", tmpFile, true);

            try
            {
                var secret = await secretResolver.ResolveSecretAsync(config.RelativeFilenameFileSecret);

                Assert.Equal("C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", secret);
            }
            finally
            {
                File.Delete(tmpFile);
            }
        }


        [Theory, AutoData]
        public async Task FullPathFileSecretTest(SecretResolver secretResolver)
        {
            var tmpFile = "secretfile.txt";

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Directory.CreateDirectory(config.WindowsTempFolder);
                tmpFile = Path.Combine(config.WindowsTempFolder, tmpFile);
            }
            else
            {
                Directory.CreateDirectory(config.LinuxTempFolder);
                tmpFile = Path.Combine(config.LinuxTempFolder, tmpFile);
            }

            File.Copy("secretfile.txt", tmpFile, true);

            try
            {
                var secret = string.Empty;

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    secret = await secretResolver.ResolveSecretAsync(config.WindowsFullPathFileSecret);
                else
                    secret = await secretResolver.ResolveSecretAsync(config.LinuxFullPathFileSecret);

                Assert.Equal("C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", secret);
                Assert.Equal("TestString", config.TextValue);
            }
            finally
            {
                File.Delete(tmpFile);
            }
        }

        [Theory, AutoData]
        public async Task NonExistentRequiredEnvironmentSecretTest(SecretResolver secretResolver)
        {
            await Assert.ThrowsAsync<SecretNotFoundException>(async () => await secretResolver.ResolveSecretAsync(config.NonExistentRequiredEnvironmentSecret));
        }

        [Theory, AutoData]
        public async Task NonExistentRequiredFileSecretTest(SecretResolver secretResolver)
        {
            await Assert.ThrowsAsync<SecretNotFoundException>(async () => await secretResolver.ResolveSecretAsync(config.NonExistentRequiredFileSecret));
        }

        [Theory, AutoData]
        public async Task NonExistentOptionalEnvironmentSecretTest(SecretResolver secretResolver)
        {
            var secret = await secretResolver.ResolveSecretAsync(config.NonExistentOptionalEnvironmentSecret);

            Assert.Null(secret);
        }

        [Theory, AutoData]
        public async Task NonExistentOptionalFileSecretTest(SecretResolver secretResolver)
        {
            var secret = await secretResolver.ResolveSecretAsync(config.NonExistentOptionalFileSecret);

            Assert.Null(secret);
        }
    }


    public class SampleConfigurationWithSecret
    {
        public string TextValue { get; set; }
        public string WindowsTempFolder { get; set; }
        public string LinuxTempFolder { get; set; }

        public Secret EnvironmentSecret { get; set; }
        public Secret ImplicitEnvironmentSecret { get; set; }

        public Secret FilenameFileSecret { get; set; }
        public Secret RelativeFilenameFileSecret { get; set; }
        public Secret WindowsFullPathFileSecret { get; set; }
        public Secret LinuxFullPathFileSecret { get; set; }

        public Secret NonExistentRequiredEnvironmentSecret { get; set; }
        public Secret NonExistentRequiredFileSecret { get; set; }
        public Secret NonExistentOptionalEnvironmentSecret { get; set; }
        public Secret NonExistentOptionalFileSecret { get; set; }
    }
}
