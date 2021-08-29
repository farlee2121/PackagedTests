

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



## Including debug symbols

Can embed the PDBs https://github.com/dotnet/sourcelink/#alternative-pdb-distribution
 - pro: no need to change "just my code" settings
 - should work across editors
Can alternately publish https://devblogs.microsoft.com/nuget/improved-package-debugging-experience-with-the-nuget-org-symbol-server/
- smaller packages
- have to configure symbol sources in vs
- unsure if it works in other editors (looks like only VS)
- pro: can set options via proj file or dotnet cli flags 

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
- sournce: https://docs.microsoft.com/en-us/nuget/create-packages/symbol-packages-snupkg
- sourece: https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file#include-pdb-files-inside-the-bundle