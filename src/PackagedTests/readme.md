

Goals
- package acceptance tests for others to run
- consumers should be able to see source within their project
- consumers should be able to debug the tests as if they were local


## packaging test projects

requires the following tag in the proj file
```xml
<PropertyGroup>
<IsPackable>true</IsPackable>
</PropertyGroup>
```

Note it must explicitly be set to true


## Including source code

add `--include-source` to pack command. 

alternately use source link https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/sourcelink
- supported in vscode https://github.com/OmniSharp/omnisharp-vscode/issues/373

Also possible to use `contentFiles` so the code is actually copied into the target repo
- example: https://stackoverflow.com/questions/52880687/how-to-share-source-code-via-nuget-packages-for-use-in-net-core-projects
- supposedly the files should be read-only
- pro: this allows them to see the code file in their explorer, no need to use round-about navigation


## Including debug symbols

Can embed the PDBs https://github.com/dotnet/sourcelink/#alternative-pdb-distribution
 - pro: no need to change "just my code" settings
 - should work across editors
Can alternately publish https://devblogs.microsoft.com/nuget/improved-package-debugging-experience-with-the-nuget-org-symbol-server/
- smaller packages
- have to configure symbol sources in vs
- unsure if it works in other editors (looks like only VS)
- pro: can set options via proj file or dotnet cli flags 

update: example: how to use symbol servers with the dotnet cli and vscode
- http://iamnotmyself.com/2020/06/25/debugging-net-core-on-osx-in-vscode/

DECIDED: copying PDBs into the nupkg is best for this scenario

EXAMPLE: add the following 
```xml
<propertyGroup>
<AllowedOutputExtensionsInPackageBuildOutputFolder>$(AllowedOutputExtensionsInPackageBuildOutputFolder);.pdb</AllowedOutputExtensionsInPackageBuildOutputFolder>
</propertyGroup>
```

still need to get it to copy with the dll
- looks like this used to work but got messed up by .net core. 
  - thread with workaround 
    - source: https://github.com/dotnet/sdk/issues/1458
  - source: https://github.com/NuGet/Home/issues/5926
    - they broke this behavior, but may fix it for .net 6


!!!: can embed symbols in the dll
- source: https://docs.microsoft.com/en-us/nuget/create-packages/symbol-packages-snupkg
- source: https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file#include-pdb-files-inside-the-bundle
- this option works, but doesn't give a convenient view of test source

I found the csproj pack reference 
- https://docs.microsoft.com/en-us/nuget/reference/msbuild-targets
- !!! `<IncludeSource>` is the equivalent of `--include-source`
  - this only includes source in the symbols files, not in the main packages
- There does not appear to be a way to replicate file copying like 


this looks promising
- https://medium.com/@attilah/source-code-only-nuget-packages-8f34a8fb4738
- !!! it points out you can use Directory.Build.props for drag and drop adoption of complex csproj/build file properties
   - Could probably also use this to make the configuration available as a nuget package?
   - yup, just have to add it to the `build` nuget folder https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package#from-a-convention-based-working-directory

Adding `<Pack>true</pack>` to a content item does generate a nuspec with that Item as a contentFile, but it does not copy the user consumer project

looks like the old `files` nuspec api does not work with net core projects
- https://github.com/NuGet/Home/issues/6548
- contentFiles is supposed to replace it, but the docs are very old and not very clear. The behaviors seem to not be as expected

### Revisiting my options (because this is a hot mess)

Debugging
- OPT: debug type embed
   - pro: code available by default
   - con: cant' seem to navigate to the code without debugger
- opt: symbol servers

Source
- source link
  - con: doesnt' seem to support normal code navigation, only debug stepping
  - con: doesn't seem to have any benefits over embedded debug type for my purpose
- copy content to consumer project using nuspec
  - con: requires less standard package commands, but still doable with dotnet cli
  - pro: content available in target project browser
  - pro: might invalidate the need to embed pdbs
  - con: i think this isn't possible with .net core 
- count on end project to copy files out of the package


At last! progress based on https://medium.com/@attilah/source-code-only-nuget-packages-8f34a8fb4738
- the main problem is that the source code links are only visible in VisualStudio. other editors like vs code are just out of luck (though debug might still work? haven't tried)