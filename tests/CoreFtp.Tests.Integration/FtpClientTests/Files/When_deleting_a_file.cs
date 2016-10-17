namespace CoreFtp.Tests.Integration.FtpClientTests.Files
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Helpers;
    using Xunit;

    public class When_deleting_a_file
    {
        public When_deleting_a_file()
        {
            Program.Initialise();
        }

        [ Fact ]
        public async Task Should_delete_file()
        {
            using ( var sut = new FtpClient( new FtpClientConfiguration
                                             {
                                                 Host = Program.FtpConfiguration.Host,
                                                 Username = Program.FtpConfiguration.Username,
                                                 Password = Program.FtpConfiguration.Password,
                                                 Port = Program.FtpConfiguration.Port
                                             } ) )
            {
                string randomFileName = $"{Guid.NewGuid()}.jpg";
                await sut.LoginAsync();
                var fileinfo = ResourceHelpers.GetResourceFileInfo( "penguin.jpg" );

                using ( var writeStream = await sut.OpenFileWriteStreamAsync( randomFileName ) )
                {
                    var fileReadStream = fileinfo.OpenRead();
                    await fileReadStream.CopyToAsync( writeStream );
                }

                ( await sut.ListFilesAsync() ).Any( x => x.Name == randomFileName ).Should().BeTrue();

                await sut.DeleteFileAsync( randomFileName );

                ( await sut.ListFilesAsync() ).Any( x => x.Name == randomFileName ).Should().BeFalse();
            }
        }
    }
}