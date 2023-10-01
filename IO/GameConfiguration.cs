using Newtonsoft.Json.Linq;

namespace Terraria.IO;

public class GameConfiguration
{
	private readonly JObject _root;

	public GameConfiguration(JObject configurationRoot)
	{
		_root = configurationRoot;
	}

	public T Get<T>(string entry) => _root[entry].ToObject<T>();
}
