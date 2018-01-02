# MoonSharp.Extensions

## WebSupport
### C#:
```C#
var response = Web.Go("https://github.com",
    new Dictionary<string, object> { },
    new Web.ConfigOption
    {

    });
Console.WriteLine(response);
```
### C#(LuaInvoke):
```C#
var lua = new Script();
lua.LoadSupport<WebSupport>();

var response = lua.DoString(@"
print(web.go('https://github.com',
--updata
{
},
--config
{
}))");
Console.WriteLine(response.String);
```
