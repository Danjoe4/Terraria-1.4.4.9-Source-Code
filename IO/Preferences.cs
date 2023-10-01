using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using Terraria.Localization;

namespace Terraria.IO;

public class Preferences
{
	public delegate void TextProcessAction(ref string text);

	private Dictionary<string, object> _data = new Dictionary<string, object>();
	private readonly string _path;
	private readonly JsonSerializerSettings _serializerSettings;
	public readonly bool UseBson;
	private readonly object _lock = new object();
	public bool AutoSave;

	public event Action<Preferences> OnSave;
	public event Action<Preferences> OnLoad;
	public event TextProcessAction OnProcessText;

	public Preferences(string path, bool parseAllTypes = false, bool useBson = false)
	{
		_path = path;
		UseBson = useBson;
		if (parseAllTypes) {
			_serializerSettings = new JsonSerializerSettings {
				TypeNameHandling = TypeNameHandling.Auto,
				MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
				Formatting = Formatting.Indented
			};
		}
		else {
			_serializerSettings = new JsonSerializerSettings {
				Formatting = Formatting.Indented
			};
		}
	}

	public bool Load()
	{
		lock (_lock) {
			if (!File.Exists(_path))
				return false;

			try {
				if (!UseBson) {
					string value = File.ReadAllText(_path);
					_data = JsonConvert.DeserializeObject<Dictionary<string, object>>(value, _serializerSettings);
				}
				else {
					using FileStream stream = File.OpenRead(_path);
					using BsonReader reader = new BsonReader(stream);
					JsonSerializer jsonSerializer = JsonSerializer.Create(_serializerSettings);
					_data = jsonSerializer.Deserialize<Dictionary<string, object>>(reader);
				}

				if (_data == null)
					_data = new Dictionary<string, object>();

				if (this.OnLoad != null)
					this.OnLoad(this);

				return true;
			}
			catch (Exception) {
				return false;
			}
		}
	}

	public bool Save(bool canCreateFile = true)
	{
		lock (_lock) {
			try {
				if (this.OnSave != null)
					this.OnSave(this);

				if (!canCreateFile && !File.Exists(_path))
					return false;

				Directory.GetParent(_path).Create();
				if (File.Exists(_path))
					File.SetAttributes(_path, FileAttributes.Normal);

				if (!UseBson) {
					string text = JsonConvert.SerializeObject(_data, _serializerSettings);
					if (this.OnProcessText != null)
						this.OnProcessText(ref text);

					File.WriteAllText(_path, text);
					File.SetAttributes(_path, FileAttributes.Normal);
				}
				else {
					using FileStream stream = File.Create(_path);
					using BsonWriter jsonWriter = new BsonWriter(stream);
					File.SetAttributes(_path, FileAttributes.Normal);
					JsonSerializer.Create(_serializerSettings).Serialize(jsonWriter, _data);
				}
			}
			catch (Exception ex) {
				Console.WriteLine(Language.GetTextValue("Error.UnableToWritePreferences", _path));
				Console.WriteLine(ex.ToString());
				return false;
			}

			return true;
		}
	}

	public void Clear()
	{
		_data.Clear();
	}

	public void Put(string name, object value)
	{
		lock (_lock) {
			_data[name] = value;
			if (AutoSave)
				Save();
		}
	}

	public bool Contains(string name)
	{
		lock (_lock) {
			return _data.ContainsKey(name);
		}
	}

	public T Get<T>(string name, T defaultValue)
	{
		lock (_lock) {
			try {
				if (_data.TryGetValue(name, out var value)) {
					if (value is T)
						return (T)value;

					if (value is JObject)
						return JsonConvert.DeserializeObject<T>(((JObject)value).ToString());

					return (T)Convert.ChangeType(value, typeof(T));
				}

				return defaultValue;
			}
			catch {
				return defaultValue;
			}
		}
	}

	public void Get<T>(string name, ref T currentValue)
	{
		currentValue = Get(name, currentValue);
	}

	public List<string> GetAllKeys() => _data.Keys.ToList();
}
