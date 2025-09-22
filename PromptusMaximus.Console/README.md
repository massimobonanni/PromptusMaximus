# 🏛️ Promptus Maximus Console

**The command-line interface for speaking like Caesar and debugging like a gladiator.**

The Promptus Maximus Console is a .NET command-line application that provides an interface for translating modern phrases into theatrical, Latin-inspired expressions using GitHub-hosted LLMs.

---

## 📋 Commands

The Promptus Maximus Console supports the following commands:

### `set` - Configure Session Settings

Manage application configuration and credentials.

#### `set credential` - Set Authentication Credentials

Configure your GitHub token for accessing GitHub Models.

**Usage:**
```bash
PromptusMaximus set credential --token YOUR_GITHUB_TOKEN
```

**Options:**
- `-t, --token` (required): GitHub Personal Access Token with GitHub Models access

**Example:**
```bash
PromptusMaximus set credential --token ghp_your_token_here
```

#### `set language` - Set Default Language

Configure the default language for translations.

**Usage:**
```bash
PromptusMaximus set language --language LANGUAGE
```

**Options:**
- `-l, --language` (required): The default language (currently supports `en` and `it`)

**Examples:**
```bash
PromptusMaximus set language --language en
PromptusMaximus set language -l it
```

#### `set model` - Set Default Model

Configure the default GitHub Model to use for translations.

**Usage:**
```bash
PromptusMaximus set model --model MODEL_NAME
```

**Options:**
- `-m, --model` (required): The default GitHub Model name (see [GitHub Models Marketplace](https://github.com/marketplace?type=models))

**Examples:**
```bash
PromptusMaximus set model --model gpt-4o
PromptusMaximus set model -m claude-3-sonnet
```

#### `set show` - Show Current Settings

Display the current application settings.

**Usage:**
```bash
PromptusMaximus set show
```

**Example Output:**
```
Default model : gpt-4o
Default language : en
GitHub Token : ghp_**********************
```

#### `set clear` - Clear All Settings

Remove all stored settings and credentials. This action requires confirmation.

**Usage:**
```bash
PromptusMaximus set clear
```

**Example:**
```bash
PromptusMaximus set clear
Are you sure you want to clear all settings? This action cannot be undone. (yes/no)
yes
All settings have been cleared successfully.
```

### `translate` - Translate Text to Roman Style

Translates a sentence as a Roman would speak it.

**Usage:**
```bash
PromptusMaximus translate --text "Your text here" [--model model1 model2]
```

**Options:**
- `-t, --text` (required): The text to translate
- `-m, --model` (optional): Models to use for translation. If not specified, uses the default model

**Examples:**
```bash
# Basic translation using default model
PromptusMaximus translate --text "I need coffee"

# Translation using specific models
PromptusMaximus translate --text "My code won't compile" --model gpt-4o gpt-3.5-turbo

# Using short form aliases
PromptusMaximus translate -t "I'm stuck in traffic" -m gpt-4o
```

### `models` - Manage GitHub Models

Work with available GitHub Models.

#### `models list` - List Available Models

Retrieve and display all available GitHub Models.

**Usage:**
```bash
PromptusMaximus models list
```

**Example Output:**
```
Retrieving Models from GitHub... ✓
Models retrieved successfully! (1.2s)

Total Models: 15

Model: gpt-4o
Publisher: OpenAI
Description: GPT-4o is OpenAI's most advanced multimodal model...
Task: chat-completion
```

---

## 🔧 Configuration

### First Time Setup

1. **Set your GitHub token:**
   ```bash
   PromptusMaximus set credential --token YOUR_GITHUB_TOKEN
   ```

2. **Set your preferred language:**
   ```bash
   PromptusMaximus set language --language en
   ```

3. **Set your default model:**
   ```bash
   PromptusMaximus set model --model gpt-4o
   ```

4. **Verify your settings:**
   ```bash
   PromptusMaximus set show
   ```

### GitHub Token Requirements

Your GitHub Personal Access Token needs access to GitHub Models. You can:
1. Go to [GitHub Settings > Developer settings > Personal access tokens](https://github.com/settings/tokens)
2. Create a new token with appropriate permissions
3. Use the token with the `set credential` command

---

## 📝 Examples

### Quick Translation
```bash
# Set up (first time only)
PromptusMaximus set credential --token ghp_your_token_here
PromptusMaximus set model --model gpt-4o
PromptusMaximus set language --language en

# Translate a phrase
PromptusMaximus translate --text "I'm debugging this code"
```

### Compare Multiple Models
```bash
PromptusMaximus translate --text "My Wi-Fi is down" --model gpt-4o claude-3-sonnet
```

### Check Available Models
```bash
PromptusMaximus models list
```

---

## 🏗️ Architecture

The console application is built with:

- **System.CommandLine**: Modern command-line interface framework
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **GitHub Models Client**: Custom client for GitHub Models API
- **Session Management**: Secure credential storage and configuration persistence
- **Loading Indicators**: Visual feedback during API calls

---

## 🔒 Security

- Credentials are stored securely using platform-specific encryption
- GitHub tokens are masked in display output
- Settings can be completely cleared when needed

---

## 📄 License

See the main project LICENSE file for licensing information.