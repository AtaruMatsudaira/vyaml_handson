using Zx;
using static Zx.Env;
using VYaml.Serialization;

await ConsoleApp.RunAsync(args, MainAsync);

async Task MainAsync(
    [Option("path")] string projectPath)
{
    await $"cd {projectPath}";

    var projectSettingPath = Path.Combine(projectPath, "ProjectSettings", "TagManager.asset");

    var tagManager = YamlSerializer.Deserialize<dynamic>(File.ReadAllBytes(projectSettingPath));

    var layers = tagManager["TagManager"]["layers"];

    log(layers);

    var prefabFiles = Directory.GetFiles("./Assets", "*.prefab", SearchOption.AllDirectories);

    foreach (var filePath in prefabFiles)
    {
        var file = File.ReadAllBytes(filePath);

        var yaml = YamlSerializer.Deserialize<Dictionary<object, dynamic>>(file);

        if (!yaml.ContainsKey("GameObject"))
        {
            continue;
        }
        
        var gameObject = yaml["GameObject"];
        
        if(gameObject["m_TagString"] == "FindMe")
        {  
            log($"FindMe: {filePath}");
        }
        
        if(layers[gameObject["m_Layer"]] == "LookAtMe")
        {  
            log($"LookAtMe: {filePath}");
        }
    }
}