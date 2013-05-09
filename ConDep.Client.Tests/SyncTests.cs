using System.IO;
using System.Linq;
using ConDep.Node;
using ConDep.Node.Client.Model;
using NUnit.Framework;
using Newtonsoft.Json;

namespace ConDep.Client.Tests
{
    [TestFixture]
    public class SyncTests
    {
        private DirectoryInfo _clientFilesRoot;
        private DirectoryInfo _serverFilesRoot;
        private readonly SyncDirHandler _dirHandler = new SyncDirHandler();

        [SetUp]
        public void Setup()
        {
            DuplicateSyncTestFiles();
        }

        [TearDown]
        public void TearDown()
        {
            if(Directory.Exists(_serverFilesRoot.FullName))
            {
                Directory.Delete(_serverFilesRoot.FullName, true);
            }
        }

        [Test]
        public void TestThatAllFilesWillBeCopiedWhenDestinationDirDoesNotExist()
        {
            Directory.Delete(_serverFilesRoot.FullName, true);

            var diffs = GetDiffs();

            Assert.That(diffs.MissingPaths.Count(), Is.GreaterThan(0));
            Assert.That(diffs.ChangedPaths.Count(), Is.EqualTo(0));
            Assert.That(diffs.DeletedPaths.Count(), Is.EqualTo(0));

        }

        [Test]
        public void TestThatChangedFilesOnserverWillBeOverwrittenByFilesFromClient()
        {
            var gitIgnoreFile = Path.Combine(_serverFilesRoot.FullName, @"psake\.gitignore");
            using (var stream = new FileStream(gitIgnoreFile, FileMode.Append, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write("Some text...");
                }
            }

            var diffs = GetDiffs();

            Assert.That(diffs.MissingPaths.Count(), Is.EqualTo(0));
            Assert.That(diffs.ChangedPaths.Count(), Is.EqualTo(1));
            Assert.That(diffs.DeletedPaths.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TestThatMissingFilesOnServerWillBeCopiedFromClient()
        {
            File.Delete(Path.Combine(_serverFilesRoot.FullName, @"psake\psake.cmd"));
            File.Delete(Path.Combine(_serverFilesRoot.FullName, @"psake\nuget\tools\chocolateyInstall.ps1"));

            var diffs = GetDiffs();

            Assert.That(diffs.MissingPaths.Count(), Is.EqualTo(2));
            Assert.That(diffs.ChangedPaths.Count(), Is.EqualTo(0));
            Assert.That(diffs.DeletedPaths.Count(), Is.EqualTo(0));
            Assert.That(diffs.MissingPaths.Any(x => x.RelativePath == @"psake\psake.cmd"));
            Assert.That(diffs.MissingPaths.Any(x => x.RelativePath == @"psake\nuget\tools\chocolateyInstall.ps1"));
        }

        [Test]
        public void TestThatMissingFilesOnClientWillBeDeletedOnServer()
        {
            File.CreateText(Path.Combine(_serverFilesRoot.FullName, "test1.txt")).Close();
            File.CreateText(Path.Combine(_serverFilesRoot.FullName, @"psake\test2.txt")).Close();
            File.CreateText(Path.Combine(_serverFilesRoot.FullName, @"psake\nuget\test3.txt")).Close();

            var diffs = GetDiffs();

            Assert.That(diffs.MissingPaths.Count(), Is.EqualTo(0));
            Assert.That(diffs.ChangedPaths.Count(), Is.EqualTo(0));
            Assert.That(diffs.DeletedPaths.Count(), Is.EqualTo(3));
            Assert.That(diffs.DeletedPaths.Any(x => x.RelativePath == @"test1.txt"));
            Assert.That(diffs.DeletedPaths.Any(x => x.RelativePath == @"psake\test2.txt"));
            Assert.That(diffs.DeletedPaths.Any(x => x.RelativePath == @"psake\nuget\test3.txt"));
        }

        private SyncDirDiff GetDiffs()
        {
            var dstDirInfo = _dirHandler.GetSubDirInfo(_serverFilesRoot.FullName, "", "", _serverFilesRoot);
            var json = JsonConvert.SerializeObject(dstDirInfo);
            var syncDirInfo = JsonConvert.DeserializeObject<SyncDirDirectory>(json);

            var diffs = syncDirInfo.Diff(_clientFilesRoot);
            return diffs;
        }

        private void DuplicateSyncTestFiles()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var root = Path.GetFullPath(currentDir + "..\\..\\..\\SyncTestFiles\\");

            if (Directory.Exists(Path.Combine(root, "server")))
            {
                Directory.Delete(Path.Combine(root, "server"), true);
            }

            _clientFilesRoot = new DirectoryInfo(Path.Combine(root, "client"));
            _serverFilesRoot = new DirectoryInfo(Path.Combine(root, "server"));

            CopyAll(_clientFilesRoot, _serverFilesRoot);
        }


        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            foreach (var fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            foreach (var diSourceSubDir in source.GetDirectories())
            {
                var nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}