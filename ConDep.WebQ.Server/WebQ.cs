using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using ConDep.WebQ.Data;

namespace ConDep.WebQ.Server
{
    public class WebQ
    {
        private readonly EventLog _eventLog;
        private Dictionary<string, List<WebQItem>> _environments = new Dictionary<string, List<WebQItem>>();
        private object _asyncRoot = new object();

        private const string DATA_FOLDER = @"ConDepWebQData\";
        private string _jsonFileName = Path.Combine(DATA_FOLDER, "condepwebq.json");

        public WebQ(EventLog eventLog)
        {
            _eventLog = eventLog;

            Directory.CreateDirectory(DATA_FOLDER);
            Load();
        }

        public object AsyncRoot { get { return _asyncRoot; } }

        public WebQItem Enqueue(string environment)
        {
            if (string.IsNullOrWhiteSpace(environment))
            {
                _eventLog.WriteEntry("Environment argument is null or empty.", EventLogEntryType.Error);
                return null;
            }

            List<WebQItem> env;

            if (_environments.ContainsKey(environment.ToLower()))
            {
                env = _environments[environment.ToLower()];
            }
            else
            {
                env = new List<WebQItem>(); 
                _environments.Add(environment.ToLower(), env);
            }

            var item = new WebQItem {Id = Guid.NewGuid().ToString(), Environment = environment, CreatedTime = DateTime.Now};
            env.Add(item);
            item.Position = env.IndexOf(item);
            Save();
            return item;
        }

        private void Save()
        {
            using (var stream = File.Open(_jsonFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var serializer = new DataContractJsonSerializer(_environments.GetType());
                serializer.WriteObject(stream, _environments);
            }
        }

        private void Load()
        {
            if(File.Exists(_jsonFileName))
            {
                using (var stream = File.Open(_jsonFileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (stream.Length == 0) return;

                    var serializer = new DataContractJsonSerializer(_environments.GetType());
                    _environments = serializer.ReadObject(stream) as Dictionary<string, List<WebQItem>>;
                }
            }
        }

        public WebQItem Peek(string environment, string id)
        {
            if (_environments.ContainsKey(environment.ToLower()))
            {
                var env = _environments[environment.ToLower()];
                var item = env.SingleOrDefault(x => x.Id.ToLower() == id.ToLower());
                if(item != null)
                {
                    item.Position = env.IndexOf(item);
                    return item;
                }
            }
            return null;
        }

        public IEnumerable<WebQItem> Peek(string environment)
        {
            if(_environments.ContainsKey(environment.ToLower()))
            {
                var env = _environments[environment.ToLower()];
                var counter = 0;
                env.ForEach(x => x.Position = counter++);
                return env;
            }
            return new List<WebQItem>();
        }

        public IEnumerable<IEnumerable<WebQItem>> Peek()
        {
            return _environments.Values.ToList();
        }

        public bool TryDequeue(string environment, string id)
        {
            if(!Exist(environment, id)) return false;
            
            var env = _environments[environment.ToLower()];
            var item = env.Single(x => x.Id == id);
            var success = env.Remove(item);

            if (success && env.Count == 0)
            {
                _environments.Remove(environment.ToLower());
            }
            Save();
            return success;
        }

        public bool Exist(string environment, string id)
        {
            if(!_environments.ContainsKey(environment.ToLower()))
            {
                return false;
            }

            var env = _environments[environment.ToLower()];
            return env.Any(x => x.Id == id);
        }

        public bool TryDequeue(string environment)
        {
            if (!_environments.ContainsKey(environment.ToLower())) return false;

            _environments.Remove(environment.ToLower());

            Save();
            return true;
        }

        public void Clear()
        {
            _environments.Clear();
            Save();
        }

        public void DequeueTimedOut(int timeout)
        {
            bool save = false;
            var allEnvQueuesToRemoveFrom = _environments.Select(env => env.Value.Where(item => item.StartTime.HasValue && DateTime.Now > item.StartTime.Value.AddMinutes(timeout)).ToList()).ToList();

            foreach(var envQueue in allEnvQueuesToRemoveFrom)
            {
                envQueue.ForEach(x => TryDequeue(x.Environment, x.Id));
                if (envQueue.Count > 0) save = true;
            }

            if (save)
            {
                Save();
            }
        }

        public WebQItem Poke(string environment, string id)
        {
            var item = Peek(environment, id);
            if(item != null)
            {
                item.StartTime = DateTime.Now;
            }
            Save();
            return item;
        }
    }
}