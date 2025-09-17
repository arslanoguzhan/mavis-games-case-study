using UnityEngine;

namespace MavisCase.Common.Serialization
{
    public class JsonSerializer
    {
        public string Serialize<TData>(TData data)
        {
            return JsonUtility.ToJson(data);
        }

        public TData Deserialize<TData>(string json)
        {
            return JsonUtility.FromJson<TData>(json);
        }

        public void DeserializeOverwrite<TData>(string json, TData obj)
        {
            JsonUtility.FromJsonOverwrite(json, obj);
        }
    }
}
