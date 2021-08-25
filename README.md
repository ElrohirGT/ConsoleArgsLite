![Nuget](https://img.shields.io/nuget/v/ConsoleArgsLite?style=for-the-badge)

# ConsoleArgsLite
A simple manager for console arguments.

## Usage
Supposing an executable named app.exe:

> app.exe Hello World

would translate to:

```csharp
public void main(string[] args)
{
  ConsoleArgsManagerLite manager = new ConsoleArgsManagerLite();
  manager.AddArgument("output");

  manager.ParseArguments(args);
  string argument = manager.GetValueFromArgument<string>("output");
  Console.WriteLine(argument)
}

//Outputs: Hello World
```

For named arguments you can provide a console name. This name does not have to include any "--", this will be automatically added by the manager.
> app.exe --outFile example.txt
```csharp
public void main(string[] args)
{
  ConsoleArgsManagerLite manager = new ConsoleArgsManagerLite();
  manager.AddArgument("output", true, "outFile");

  manager.ParseArguments(args);
  string argument = manager.GetValueFromArgument<string>("output");
  Console.WriteLine(argument)
}

//Outputs: example.txt
```

You can pass a parser that converts from string to a different type, just be carefull to use a value type and trying to retrieve the value of the argument before parsing them.
> app.exe --maxValue 100
```csharp
public void main(string[] args)
{
  ConsoleArgsManagerLite manager = new ConsoleArgsManagerLite();
  manager.AddArgument(
    name: "output",
    required: true,
    parser: (s) => int.Parse(s),
    consoleName: "maxValue"
  );

  manager.ParseArguments(args);
  int argument = manager.GetValueFromArgument<int>("output");//When using value types, be carefull to not call this before parsing the arguments. Or use nullables.
  Console.WriteLine(argument)
}

//Outputs: 100
```

You can add various arguments, the order is important, especially if you're going to use some that are not required.
> app.exe input.mp4 output.mp4 --CRF 30

> app.exe input.mp4 output.mp4
```csharp

public void main(string[] args)
{
  ConsoleArgsManagerLite manager = new ConsoleArgsManagerLite();
  manager.AddArgument("inputFile");
  manager.AddArgument("outputFile");
  manager.AddArgument<int?>(
    name: "crf",
    required: false,
    parser: (s) =>
    {
        if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
            return null;
        return int.Parse(s);
    },
    consoleName: "consoleName"
  );

  manager.ParseArguments(args);
  string outputFile = manager.GetValueFromArgument<string>("outputFile");
  string inputFile = manager.GetValueFromArgument<string>("inputFile");
  int? crf = manager.GetValueFromArgument<int?>("crf");
  
  Console.WriteLine($"{inputFile}->{outputFile}: {crf ?? ''}");
}

//Outputs: input.mp4->output.mp4: 30
//Outputs: input.mp4->output.mp4: 
```
