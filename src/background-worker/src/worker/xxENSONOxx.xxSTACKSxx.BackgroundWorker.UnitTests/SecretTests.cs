using AutoFixture.Xunit2;
using Xunit;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.Exceptions;
using xxENSONOxx.xxSTACKSxx.BackgroundWorker.Secrets;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests
{
    [Trait("TestType", "IntegrationTests")]
    public class SecretTests
    {
        SampleConfigurationWithSecret config;

        public SecretTests()
        {
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
}
