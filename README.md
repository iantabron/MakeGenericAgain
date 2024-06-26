# MakeGenericAgain
Problem is that nswag client code generation from open api specification or swagger generates classes without generics. This tool can be used afterwards to make classes generic again

More infos about the general problem can be found here
 - https://github.com/RicoSuter/NSwag/issues/1139
 - https://blog.devgenius.io/nswag-csharp-client-with-generics-support-6ad6a09f81d6


To install it on other projects, add this to the csproj:

```
  <Target Name="MakeGenericAgain" AfterTargets="NSwag" Condition="'$(Configuration)' == 'Debug'">
    <Exec IgnoreExitCode="true" Command="dotnet tool install --global IanTabron.MakeGenericAgain" />
    <Exec Command="makeGenericAgain -f $(SolutionDir)src\SDK\Net\v1\ClientGenerated.cs" />
  </Target>
```

You can optionally provide names of types to ignore (should include any type names containing the word 'Of' or 'And' as a minimum)

```
  <Target Name="MakeGenericAgain" AfterTargets="NSwag" Condition="'$(Configuration)' == 'Debug'">
    <Exec IgnoreExitCode="true" Command="dotnet tool install --global IanTabron.MakeGenericAgain" />
    <Exec Command="makeGenericAgain -f $(SolutionDir)src\SDK\Net\v1\ClientGenerated.cs -i IgnorableOfType,AnotherAndIgnorable" />
  </Target>
```

To run int use
```
 makeGenericAgain -f "C:\Path\client.cs"
```

Or with ignorable type names

```
 makeGenericAgain -f "C:\Path\client.cs" -i "IgnorableOfType,AnotherAndIgnorable"
```


## Links
[Github Repository](https://github.com/iantabron/MakeGenericAgain) | [NuGet Package](https://www.nuget.org/packages/IanTabron.MakeGenericAgain/)
#