using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading;
using ConDep.WebQ.Data;

namespace ConDep.WebQ.Server
{
    public class WebQ
    {
        private readonly EventLog _eventLog;
        private ReaderWriterLockSlim _environmentLock = new ReaderWriterLockSlim();
        private List<EnvironmentQ> _environments = new List<EnvironmentQ>();

        private const string DATA_FOLDER = @"ConDepWebQData\";
        private string _jsonFileName = Path.Combine(DATA_FOLDER, "condepwebq.json");

        public WebQ(EventLog eventLog)
        {
            _eventLog = eventLog;

            Directory.CreateDirectory(DATA_FOLDER);
            Load();
        }

        public WebQItem Add(string environment)
        {
            if(string.IsNullOrWhiteSpace(environment))
            {
                _eventLog.WriteEntry("Environment argument is null or empty.", EventLogEntryType.Error);
                return null;
            }

            _environmentLock.EnterWriteLock();
            try
            {
                EnvironmentQ env;

                if (EnvironmentExist(environment))
                {
                    env = GetEnvironment(environment);
                }
                else
                {
                    env = new EnvironmentQ { Environment = environment, Queue = new List<WebQItem>() };
                    _environments.Add(env);
                }

                var item = new WebQItem { Id = Guid.NewGuid().ToString(), Environment = environment, CreatedTime = DateTime.Now };
                env.Queue.Add(item);
                item.Position = env.Queue.IndexOf(item);
                Save();
                return item;
            }
            finally
            {
                _environmentLock.ExitWriteLock();
            }
        }

        private bool EnvironmentExist(string environment)
        {
            _environmentLock.EnterReadLock();
            try
            {
                return _environments.Any(x => x.Environment.ToLower() == environment.ToLower());
            }
            finally
            {
                _environmentLock.ExitReadLock();
            }

        }

        private EnvironmentQ GetEnvironment(string environment)
        {
            _environmentLock.EnterReadLock();
            try
            {
                return _environments.SingleOrDefault(x => x.Environment.ToLower() == environment.ToLower());
            }
            finally
            {
                _environmentLock.ExitReadLock();
            }
        }

        private void Save()
        {
            using (var stream = File.Open(_jsonFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                _environmentLock.EnterReadLock();
                try
                {
                    var serializer = new DataContractJsonSerializer(_environments.GetType());
                    serializer.WriteObject(stream, _environments);
                }
                finally
                {
                    _environmentLock.ExitReadLock();
                }
            }
        }

        private void Load()
        {
            if(File.Exists(_jsonFileName))
            {
                using (var stream = File.Open(_jsonFileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (stream.Length == 0) return;

                    _environmentLock.EnterWriteLock();
                    try
                    {
                        var serializer = new DataContractJsonSerializer(_environments.GetType());
                        _environments = serializer.ReadObject(stream) as List<EnvironmentQ>;
                    }
                    finally
                    {
                        _environmentLock.ExitWriteLock();
                    }
                }
            }
        }

        public WebQItem Get(string environment, string id)
        {
            var env = Get(environment).ToList();
            var item = env.SingleOrDefault(x => x.Id.ToLower() == id.ToLower());
            if(item != null) item.Position = env.IndexOf(item);
            return item;
        }

        public IEnumerable<WebQItem> Get(string environment)
        {
            if(EnvironmentExist(environment))
            {
                var env = GetEnvironment(environment);
                var counter = 0;
                env.Queue.ForEach(x => x.Position = counter++);
                return env.Queue;
            }
            return new List<WebQItem>();
        }

        public IEnumerable<EnvironmentQ> Get()
        {
            return _environments.AsReadOnly();
        }

        public bool TryRemove(string environment, string id)
        {
            if(!Exist(environment, id)) return false;
            bool success;
            lock(_environments)
            {
                var env = _environments.Single(x => x.Environment.ToLower() == environment.ToLower());
                var item = env.Queue.Single(x => x.Id == id);

                success = env.Queue.Remove(item);

                if (success && env.Queue.Count == 0)
                {
                    _environments.Remove(env);
                }
            }
            Save();
            return success;
        }

        public bool Exist(string environment, string id)
        {
            if(!EnvironmentExist(environment))
            {
                return false;
            }

            lock(_environments)
            {
                var env = _environments.Single(x => x.Environment.ToLower() == environment.ToLower());
                return env.Queue.Any(x => x.Id == id);
            }
        }

        public bool TryRemove(string environment)
        {
            if (!EnvironmentExist(environment)) return false;

            lock(_environments)
            {
                var env = _environments.Single(x => x.Environment.ToLower() == environment.ToLower());
                _environments.Remove(env);
            }
            Save();
            return true;
        }

        public void Clear()
        {
            lock(_environments)
            {
                _environments.Clear();
            }
            Save();
        }

        public void RemoveOldEntries(int timeout)
        {
            lock(_environments)
            {
                bool save = false;
                foreach (var env in _environments)
                {
                    var itemsToRemove = env.Queue.Where(item => item.StartTime.HasValue && item.StartTime.Value.AddSeconds(timeout) > DateTime.Now).ToList();
                    itemsToRemove.ForEach(x => TryRemove(x.Environment, x.Id));
                    if (itemsToRemove.Count > 0) save = true;
                }

                if (save)
                {
                    Save();
                }
            }
        }
    }
}