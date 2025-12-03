using ReEstrena.Models;
using Newtonsoft.Json;
using Dapper;
public static class Objeto
{
    public static string ObjectToString<T>(T? obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
    public static T? StringToObject<T>(string txt)
    {
        if(string.IsNullOrEmpty(txt))
            return default;
        else
            return JsonConvert.DeserializeObject<T>(txt);
    }
    public static string ListToString<T>(List<T> lista)
    {
        return JsonConvert.SerializeObject(lista);
    }
    public static List<T>?StringToList<T>(string json)
    {
        if(string.IsNullOrEmpty(json))
            return new List<T>();
        else
            return JsonConvert.DeserializeObject<List<T>>(json);
    }
};