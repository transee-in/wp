﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation.Collections;

namespace Transee.Common {
	public class ObservableDictionary : IObservableMap<string, object> {
		private class ObservableDictionaryChangedEventArgs : IMapChangedEventArgs<string> {
			public ObservableDictionaryChangedEventArgs(CollectionChange change, string key) {
				CollectionChange = change;
				Key = key;
			}

			public CollectionChange CollectionChange { get; }
			public string Key { get; }
		}

		private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();
		public event MapChangedEventHandler<string, object> MapChanged;

		private void InvokeMapChanged(CollectionChange change, string key) {
			var eventHandler = MapChanged;
			eventHandler?.Invoke(this, new ObservableDictionaryChangedEventArgs(change, key));
		}

		public void Add(string key, object value) {
			_dictionary.Add(key, value);
			InvokeMapChanged(CollectionChange.ItemInserted, key);
		}

		public void Add(KeyValuePair<string, object> item) {
			Add(item.Key, item.Value);
		}

		public bool Remove(string key) {
			if (_dictionary.Remove(key)) {
				InvokeMapChanged(CollectionChange.ItemRemoved, key);
				return true;
			}
			return false;
		}

		public bool Remove(KeyValuePair<string, object> item) {
			object currentValue;
			if (_dictionary.TryGetValue(item.Key, out currentValue) &&
			    Equals(item.Value, currentValue) && _dictionary.Remove(item.Key)) {
				InvokeMapChanged(CollectionChange.ItemRemoved, item.Key);
				return true;
			}
			return false;
		}

		public object this[string key] {
			get { return _dictionary[key]; }
			set {
				_dictionary[key] = value;
				InvokeMapChanged(CollectionChange.ItemChanged, key);
			}
		}

		public void Clear() {
			var priorKeys = _dictionary.Keys.ToArray();
			_dictionary.Clear();
			foreach (var key in priorKeys) {
				InvokeMapChanged(CollectionChange.ItemRemoved, key);
			}
		}

		public ICollection<string> Keys => _dictionary.Keys;

		public bool ContainsKey(string key) => _dictionary.ContainsKey(key);

		public bool TryGetValue(string key, out object value) => _dictionary.TryGetValue(key, out value);

		public ICollection<object> Values => _dictionary.Values;

		public bool Contains(KeyValuePair<string, object> item) => _dictionary.Contains(item);

		public int Count => _dictionary.Count;

		public bool IsReadOnly => false;

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _dictionary.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {
			var arraySize = array.Length;
			foreach (var pair in _dictionary) {
				if (arrayIndex >= arraySize) {
					break;
				}
				array[arrayIndex++] = pair;
			}
		}
	}
}