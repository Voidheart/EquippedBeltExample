# Equipped Belt Example

Sample code how to add Status Effects to a Belt

# Compiling

Checks for BepInEx folder in Valheim Directory if doesn't exist it will use R2 Mod Manager profile which you can select which profile by editing these values
default profile is `DebugTest`
```xml
    <PropertyGroup Condition="!Exists('$(BepInExPath)')">
        <!--        Change this to your output profile name-->
        <R2MMProfile>DebugTest</R2MMProfile>
        <BepInExPath>$(AppData)\r2modmanPlus-local\Valheim\profiles\$(R2MMProfile)\BepInEx</BepInExPath>
        <PluginPath>$(BepInExPath)\plugins</PluginPath>
    </PropertyGroup>
```
Build will ouput the assembly into the `BepInEx\Plugins` folder

# Asset is not included

`Plugin.cs` Line: 28 change it to reflect your asset file.
```c#
    Item beltHealthUpgrade = new("AssetFileNameGoesHere", "MyBeltPrefabNameGoesHere");
```
