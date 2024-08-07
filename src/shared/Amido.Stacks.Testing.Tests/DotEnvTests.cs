using System;
using Amido.Stacks.Testing.Settings;
using Xunit;

namespace Amido.Stacks.Testing.Tests
{
    public class DotEnvTests
    {
        [Fact]
        public void LoadDotEnvFile()
        {
            //Using .env.example because .env are not commited to git
            //generally we should use only DotEnv.Load(); which load the .env file
            DotEnv.Load(".env.example");

            /* Test Data from .env.example file
             
                TESTA=123
                TEST_B=123
                TEST:C=123
                TEST.D=
                TEST_E=""
            */
            Assert.Equal("123", EnvVar("TESTA"));
            Assert.Equal("123", EnvVar("TEST_B"));
            Assert.Equal("123", EnvVar("TEST:C"));
            Assert.Null(EnvVar("TEST.D"));
            Assert.Null(EnvVar("TEST.E"));

            /*  Examples with unformated data and comments

                MyKey="MyIniConfig.ini Value"
                HOST = localhost
                //DATABASE = ydb
                #PORT = 5432
                USER	= 123
                ;PASS
            */
            Assert.Equal("MyIniConfig.ini Value", EnvVar("MyKey"));
            Assert.Equal("localhost", EnvVar("HOST"));
            Assert.Null(EnvVar("DATABASE"));
            Assert.Null(EnvVar("PORT"));
            Assert.Equal("123", EnvVar("USER"));
            Assert.Null(EnvVar("PASS"));

            /* Nested values
             
                [Position]
                Title="My INI Config title"
                Name="My INI Config name"

                [Logging:LogLevel]
                Default=Information
                Microsoft=Warning
            */
            Assert.Equal("My INI Config title", EnvVar("Position:Title"));
            Assert.Equal("My INI Config name", EnvVar("Position:Name"));
            Assert.Equal("Information", EnvVar("Logging:LogLevel:Default"));
            Assert.Equal("Warning", EnvVar("Logging:LogLevel:Microsoft"));
        }


        [Fact]
        public void InvalidFilenameDoesNotThrowException()
        {
            DotEnv.Load(".env.that.does.not.exist");
        }

        [Fact]
        public void MissingFilenameThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => DotEnv.Load(null));
        }

        private string EnvVar(string key) => Environment.GetEnvironmentVariable(key);
    }
}
