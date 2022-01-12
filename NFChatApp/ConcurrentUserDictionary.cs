using System;
using System.Collections;
using System.Text;

namespace NFChatApp
{
    
    public class ConcurrentUserDictionary
    {
        private Hashtable _users = new Hashtable();
        private object _lock = new object(); 

        public int Count { get { return _users.Count; } }

        public string[] Keys { get { return GetKeys(); } }

        public string[] Values { get { return GetValues(); } }

        public string this[string key]
        {
            get
            {
                return _users[key] as string;
            }
            set
            {
                lock(_lock)
                {
                    _users[key] = value;
                }
            }
        }

        public void Remove(string value)
        {
            lock (_lock)
            {
                _users.Remove(value);
            }
        }

        private string[] GetKeys()
        {
            var keys = _users.Keys;
            string[] returnkeys = new string[keys.Count];

            int i = 0;
            foreach (object key in keys)
            {
                returnkeys[i] = key as string;
                i++;
            }

            return returnkeys;
        }

        private string[] GetValues()
        {
            var values = _users.Values;
            string[] returnkeys = new string[values.Count];

            int i = 0;
            foreach (object value in values)
            {
                returnkeys[i] = value as string;
                i++;
            }

            return returnkeys;
        }
    }
}
