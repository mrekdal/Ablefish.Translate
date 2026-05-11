# TranslationPoster

A command-line tool for posting translation jobs to the translation database and outputting SQL statements for inserting translated text.

## Overview

TranslationPoster reads source text from either a database (monograph or project worklist) or `.resx` resource files, sends the text to a translation engine, and outputs `EXEC` SQL statements that can be run against the translation database.

## Configuration

The tool reads its settings from `appsettings.json` (and `appsettings.Development.json` for local development). The following sections are required:

```json
{
  "ConnectionStrings": {
	"MSSQL": "<your SQL Server connection string>"
  },
  "Azure": {
	"Key": "<Azure Translator API key>",
	"Region": "<Azure region, e.g. norwayeast>"
  },
  "DeepL": {
	"AuthKey": "<DeepL API authentication key>"
  },
  "Lara": {
	"Key": "<Lara API key>",
	"Secret": "<Lara API secret>"
  }
}
```

## Usage

```
TranslationPoster [options]
```

### Options

| Short | Long          | Default | Description                                         |
|-------|---------------|---------|-----------------------------------------------------|
| `-e`  | `--Engine`    | `Azure` | Translation engine to use: `Azure`, `Lara`, `DeepL` |
| `-m`  | `--Monograph` |         | Read source text from monograph fields (field number 1–n) |
| `-r`  | `--Resource`  |         | Read source text from a `.resx` resource file (specify the source language code) |
| `-t`  | `--Target`    |         | Target language code to translate into (e.g. `nb`, `de`, `fr`) |
| `-p`  | `--Project`   |         | Project ID to translate                             |
| `-n`  | `--Count`     | `100`   | Maximum number of strings to translate              |

### Examples

**Translate up to 50 strings in project 3 into Norwegian using Azure:**
```
TranslationPoster -p 3 -t nb -n 50 -e Azure
```

**Translate project 7 into German using DeepL (default count of 100):**
```
TranslationPoster -p 7 -t de -e DeepL
```

**Export monograph source texts as SQL insert statements:**
```
TranslationPoster -m 1
```

**Export resource file entries as SQL insert statements (source language `en`):**
```
TranslationPoster -r en
```

## Output

All output is written to stdout as SQL `EXEC` statements, which can be redirected to a file and executed against the translation database:

```
TranslationPoster -p 3 -t nb -n 100 > output.sql
```

## Translation Engines

| Engine  | Configuration section |
|---------|-----------------------|
| `Azure` | `Azure` (`Key`, `Region`) |
| `DeepL` | `DeepL` (`AuthKey`) |
| `Lara`  | `Lara` (`Key`, `Secret`) |
