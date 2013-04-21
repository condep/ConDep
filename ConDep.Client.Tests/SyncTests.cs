using System.IO;
using NUnit.Framework;

namespace ConDep.Client.Tests
{
    [TestFixture]
    public class SyncTests
    {
        private DirectoryInfo _clientFilesRoot;
        private DirectoryInfo _serverFilesRoot;

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
            var handler = new SyncDirHandler();

            var clientDir = handler.GetSubDirInfo("", "", _clientFilesRoot);
            var serverDir = handler.GetSubDirInfo("", "", _serverFilesRoot);

            var container = handler.GetDiffs(clientDir, serverDir);

            Assert.That(container.FilesToCopy.Count, Is.GreaterThan(0));
            Assert.That(container.FilesToDelete.Count, Is.EqualTo(0));
            Assert.That(container.FilesToUpdate.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestThatChangedFilesOnserverWillBeOverwrittenByFilesFromClient()
        {
            var gitIgnoreFile = Path.Combine(_serverFilesRoot.FullName, @"psake\.gitignore");
            using(var stream = new FileStream(gitIgnoreFile, FileMode.Append, FileAccess.Write))
            {
                using(var writer = new StreamWriter(stream))
                {
                    writer.Write("Some text...");
                }
            }

            var handler = new SyncDirHandler();

            var clientDir = handler.GetSubDirInfo("", "", _clientFilesRoot);
            var serverDir = handler.GetSubDirInfo("", "", _serverFilesRoot);

            var container = handler.GetDiffs(clientDir, serverDir);

            Assert.That(container.FilesToCopy.Count, Is.EqualTo(0));
            Assert.That(container.FilesToDelete.Count, Is.EqualTo(0));
            Assert.That(container.FilesToUpdate.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestThatMissingFilesOnServerWillBeCopiedFromClient()
        {
            File.Delete(Path.Combine(_serverFilesRoot.FullName, @"psake\psake.cmd"));
            File.Delete(Path.Combine(_serverFilesRoot.FullName, @"psake\nuget\tools\chocolateyInstall.ps1"));

            var handler = new SyncDirHandler();

            var clientDir = handler.GetSubDirInfo("", "", _clientFilesRoot);
            var serverDir = handler.GetSubDirInfo("", "", _serverFilesRoot);

            var container = handler.GetDiffs(clientDir, serverDir);

            Assert.That(container.FilesToCopy.Count, Is.EqualTo(2));
            Assert.That(container.FilesToCopy.Contains(@"\psake\psake.cmd"));
            Assert.That(container.FilesToCopy.Contains(@"\psake\nuget\tools\chocolateyInstall.ps1"));
            Assert.That(container.FilesToDelete.Count, Is.EqualTo(0));
            Assert.That(container.FilesToUpdate.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestThatMissingFilesOnClientWillBeDeletedOnServer()
        {
            File.CreateText(Path.Combine(_serverFilesRoot.FullName, "test1.txt")).Close();
            File.CreateText(Path.Combine(_serverFilesRoot.FullName, @"psake\test2.txt")).Close();
            File.CreateText(Path.Combine(_serverFilesRoot.FullName, @"psake\nuget\test3.txt")).Close();

            var handler = new SyncDirHandler();

            var clientDir = handler.GetSubDirInfo("", "", _clientFilesRoot);
            var serverDir = handler.GetSubDirInfo("", "", _serverFilesRoot);

            var container = handler.GetDiffs(clientDir, serverDir);

            Assert.That(container.FilesToCopy.Count, Is.EqualTo(0));
            Assert.That(container.FilesToDelete.Count, Is.EqualTo(3));
            Assert.That(container.FilesToDelete.Contains(@"\test1.txt"));
            Assert.That(container.FilesToDelete.Contains(@"\psake\test2.txt"));
            Assert.That(container.FilesToDelete.Contains(@"\psake\nuget\test3.txt"));
            Assert.That(container.FilesToUpdate.Count, Is.EqualTo(0));
        }

        private void DuplicateSyncTestFiles()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var root = Path.GetFullPath(currentDir + "..\\..\\..\\SyncTestFiles\\");

            if (Directory.Exists(Path.Combine(root, "server")))
            {
                Directory.Delete(Path.Combine(root, "server"),true);
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