using System.IO;
using AssetBundlerSupport;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AssetBundlerSupportTests
{
    [TestFixture]
    public class FileUtilsTests
    {
        private const string ParentFolder = "TestParentFolder";
        private const string SubFolder = "SubFolder01";
        private const string TestFile01 = "testFile01.txt";
        private const string TestFile02 = "testFile02.txt";
        private const string DestFolder = "TestDestination";
        private IAssetBundlerLogging _logger;
        private FileUtils _sut;
        private string _parentFolderPath;
        private string _subFolderPath;
        private string _destFolderPath;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<IAssetBundlerLogging>();
            _sut = new FileUtils(_logger);
            _parentFolderPath = Path.Combine(Directory.GetCurrentDirectory(), ParentFolder);
            CreateFolder(_parentFolderPath);
            CreateFileInFolder(_parentFolderPath, TestFile01);
            _subFolderPath = Path.Combine(_parentFolderPath, SubFolder);
            CreateFolder(_subFolderPath);
            CreateFileInFolder(_subFolderPath, TestFile02);
            _destFolderPath = Path.Combine(Directory.GetCurrentDirectory(), DestFolder);
        }

        [TearDown]
        public void Cleanup()
        {
            Directory.Delete(_parentFolderPath, true);
            Directory.Delete(_destFolderPath, true);
        }

        [Test]
        public void should_copy_dir_with_subdirs()
        {
            _sut.DirectoryCopy(_parentFolderPath, _destFolderPath, true);
            Directory.Exists(_destFolderPath).Should().BeTrue();
            var destSubFolderPath = Path.Combine(_destFolderPath, SubFolder);
            Directory.Exists(destSubFolderPath).Should().BeTrue();
            File.Exists(Path.Combine(_destFolderPath, TestFile01)).Should().BeTrue();
            File.Exists(Path.Combine(destSubFolderPath, TestFile02)).Should().BeTrue();
        }

        [Test]
        public void should_copy_dir_without_subdirs()
        {
            _sut.DirectoryCopy(_parentFolderPath, _destFolderPath, false);
            Directory.Exists(_destFolderPath).Should().BeTrue();
            var destSubFolderPath = Path.Combine(_destFolderPath, SubFolder);
            Directory.Exists(destSubFolderPath).Should().BeFalse();
            File.Exists(Path.Combine(_destFolderPath, TestFile01)).Should().BeTrue();
            File.Exists(Path.Combine(destSubFolderPath, TestFile02)).Should().BeFalse();
        }

        private static void CreateFolder(string folderPath)
        {
            var existing = Directory.Exists(folderPath);
            existing.Should().BeFalse();
            Directory.CreateDirectory(folderPath);
        }

        private static void CreateFileInFolder(string parentFolderPath, string fileName)
        {
            var fullFilePath = Path.Combine(parentFolderPath, fileName);
            var existing = File.Exists(fullFilePath);
            existing.Should().BeFalse();
            // Write the string array to a new file named "WriteLines.txt".
            using (var outputFile = new StreamWriter(fullFilePath))
            {
                outputFile.WriteLine("SOME STUFF IN A FILE");
            }
        }
    }
}

