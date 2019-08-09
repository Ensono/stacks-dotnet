using System;
using System.IO;
using Amido.Stacks.Configuration.Exceptions;
using AutoFixture.Xunit2;
using Xunit;
using Xunit.Abstractions;
using Config = Amido.Stacks.Tests.Settings.Configuration;

namespace Amido.Stacks.Configuration.Tests
{
    [Trait("TestType", "IntegrationTests")]
    public class SecretTests
    {
        private readonly ITestOutputHelper output;
        SampleConfigurationWithSecret config;

        public SecretTests(ITestOutputHelper output)
        {
            this.output = output;
            config = Config.For<SampleConfigurationWithSecret>("SecretConfigurationTest");

            Environment.SetEnvironmentVariable("itsasecret", "PleaseDontTellAnyone");
        }


        [Theory, AutoData]
        public void EnvironmentBasedSecretTest(SecretResolver secretResolver)
        {
            var secret = secretResolver.ResolveSecret(config.EnvironmentSecret);

            Assert.Equal("PleaseDontTellAnyone", secret);
            Assert.Equal("TestString", config.TextValue);
        }

        [Theory, AutoData]
        public void ImplicitEnvironmentBasedSecretTest(SecretResolver secretResolver)
        {
            var secret = secretResolver.ResolveSecret(config.ImplicitEnvironmentSecret);

            Assert.Equal("PleaseDontTellAnyone", secret);
            Assert.Equal("TestString", config.TextValue);
        }

        [Theory, AutoData]
        public void FilenameFileSecretTest(SecretResolver secretResolver)
        {
            var secret = secretResolver.ResolveSecret(config.FilenameFileSecret);

            Assert.Equal("C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", secret);
            Assert.Equal("TestString", config.TextValue);
        }

        [Theory, AutoData]
        public void RelativeFilenameFileSecretTest(SecretResolver secretResolver)
        {
            var tmpFile = "subfolder/secretfile.txt";

            Directory.CreateDirectory("subfolder");

            File.Copy("secretfile.txt", tmpFile, true);

            try
            {
                var secret = secretResolver.ResolveSecret(config.RelativeFilenameFileSecret);

                Assert.Equal("C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", secret);
            }
            finally
            {
                File.Delete(tmpFile);
            }
        }


        [Theory, AutoData]
        public void FullPathFileSecretTest(SecretResolver secretResolver)
        {
            var tmpFile = "secretfile.txt";

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                tmpFile = Path.Combine(config.WindowsTempFolder, tmpFile);
            }
            else
            {
                tmpFile = Path.Combine(config.LinuxTempFolder, tmpFile);
                config.WindowsFullPathFileSecret.Identifier.Replace("/temp/", "");
            }

            File.Copy("secretfile.txt", tmpFile, true);

            try
            {
                var secret = string.Empty;

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    secret = secretResolver.ResolveSecret(config.WindowsFullPathFileSecret);
                else
                    secret = secretResolver.ResolveSecret(config.LinuxFullPathFileSecret);

                Assert.Equal("C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", secret);
                Assert.Equal("TestString", config.TextValue);
            }
            finally
            {
                File.Delete(tmpFile);
            }
        }

        [Theory, AutoData]
        public void NonExistentRequiredEnvironmentSecretTest(SecretResolver secretResolver)
        {
            Assert.Throws<SecretNotFoundException>(() => secretResolver.ResolveSecret(config.NonExistentRequiredEnvironmentSecret));
        }

        [Theory, AutoData]
        public void NonExistentRequiredFileSecretTest(SecretResolver secretResolver)
        {
            Assert.Throws<SecretNotFoundException>(() => secretResolver.ResolveSecret(config.NonExistentRequiredFileSecret));
        }

        [Theory, AutoData]
        public void NonExistentOptionalEnvironmentSecretTest(SecretResolver secretResolver)
        {
            var secret = secretResolver.ResolveSecret(config.NonExistentOptionalEnvironmentSecret);

            Assert.Null(secret);
        }

        [Theory, AutoData]
        public void NonExistentOptionalFileSecretTest(SecretResolver secretResolver)
        {
            var secret = secretResolver.ResolveSecret(config.NonExistentOptionalFileSecret);

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
