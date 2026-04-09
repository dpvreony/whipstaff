# whipstaff-yamltemplating

A .NET global tool that merges a template YAML file with a content YAML file, writing the result to an output path. Also supports injecting a YAML fragment at a specific dot-notation path within a target file.

## Installation

```shell
dotnet tool install --global whipstaff-yamltemplating
```

## Usage

### Merge mode

Merges a template and a content file, with content values taking precedence:

```shell
whipstaff-yamltemplating --template template.yaml --content content.yaml --output output.yaml
```

### Path injection mode

Replaces a specific location in the content file with the template:

```shell
whipstaff-yamltemplating -t tasks.yaml -c target.yaml -o output.yaml --path steps.build.tasks
```

## Options

| Option | Alias | Required | Description |
|--------|-------|----------|-------------|
| `--template` | `-t` | Yes | Path to the template YAML file (`.yaml`/`.yml`, must exist) |
| `--content` | `-c` | Yes | Path to the content YAML file (`.yaml`/`.yml`, must exist) |
| `--output` | `-o` | Yes | Path to write the merged output YAML file |
| `--path` | `-p` | No | Dot-notation path to inject the template into the content (e.g. `steps.build.tasks`) |
