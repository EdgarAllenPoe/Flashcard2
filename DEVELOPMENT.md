# Development Workflow - Flashcard App

This document describes the automated workflows and quality assurance processes for the Flashcard App.

## 🚀 Quick Start

### Windows Users
```bash
# Setup development environment
dev.bat setup

# Format code
dev.bat format

# Run tests
dev.bat test

# Run quality checks
dev.bat quality
```

### PowerShell Users
```powershell
# Setup development environment
.\scripts\dev-workflow.ps1 -Action setup

# Format code
.\scripts\dev-workflow.ps1 -Action format

# Run tests
.\scripts\dev-workflow.ps1 -Action test

# Run quality checks
.\scripts\dev-workflow.ps1 -Action quality
```

## 🔧 Available Commands

### Development Workflow (`dev-workflow.ps1`)

| Action | Description | Example |
|--------|-------------|---------|
| `format` | Format C# and XAML code | `.\scripts\dev-workflow.ps1 -Action format` |
| `test` | Run fast tests | `.\scripts\dev-workflow.ps1 -Action test` |
| `test -All` | Run all tests | `.\scripts\dev-workflow.ps1 -Action test -All` |
| `build` | Build the solution | `.\scripts\dev-workflow.ps1 -Action build` |
| `coverage` | Generate coverage report | `.\scripts\dev-workflow.ps1 -Action coverage` |
| `quality` | Run all quality checks | `.\scripts\dev-workflow.ps1 -Action quality` |
| `clean` | Clean build artifacts | `.\scripts\dev-workflow.ps1 -Action clean` |
| `setup` | Setup development environment | `.\scripts\dev-workflow.ps1 -Action setup` |

### Pre-commit Hook (`pre-commit.ps1`)

Automatically runs before each Git commit:
- Code formatting check
- XAML formatting check
- Build verification
- Fast tests execution
- Code analysis
- Security vulnerability check

## 🏗️ CI/CD Pipeline

The GitHub Actions workflow (`.github/workflows/ci-cd.yml`) includes:

### Quality Gates
- Code formatting verification
- XAML formatting verification
- Build verification
- Code analysis

### Testing
- Fast tests execution
- Integration tests execution
- All tests execution
- Coverage report generation

### Security
- Vulnerable package detection
- Outdated package detection
- Security analysis

### Build & Package
- WinUI application build
- Application packaging
- Artifact upload

### Performance Testing
- Performance test execution
- Performance metrics collection

## 📊 Quality Metrics

### Code Coverage
- Generated automatically during CI/CD
- Available in `coverage-report/` directory
- HTML and JSON summary formats

### Test Categories
- **Fast**: Quick unit tests
- **Integration**: Integration tests
- **Performance**: Performance benchmarks
- **E2E**: End-to-end tests

### Code Analysis
- .NET analyzers enabled
- Security analysis
- Performance analysis
- Maintainability analysis

## 🔒 Git Hooks

### Setup Git Hooks
```bash
.\scripts\setup-git-hooks.ps1
```

### Available Hooks
- **pre-commit**: Quality checks before commit
- **commit-msg**: Commit message validation
- **post-commit**: Success notification

### Commit Message Format
```
type(scope): description

Types: feat, fix, docs, style, refactor, test, chore, perf, ci, build, revert
Example: feat(config): add configuration page
```

## 🛠️ Development Tools

### Installed Tools
- `dotnet-format`: Code formatting
- `xstyler`: XAML formatting
- `dotnet-coverage`: Coverage collection
- `reportgenerator`: Coverage reports
- `dotnet-test-rerun`: Test retry
- `tyrannosaurustrx`: Test result formatting
- `Svg2Xaml`: SVG to XAML conversion
- `XamlPlayground`: XAML testing
- `auto-tester`: Automated testing

### Usage Examples
```bash
# Format code
dotnet-format

# Format XAML
xstyler --directory Views

# Generate coverage
dotnet-coverage collect dotnet test
reportgenerator -reports:"coverage.cobertura.xml" -targetdir:"coverage-report"

# Convert SVG to XAML
Svg2Xaml icon.svg Views/Icons/icon.xaml
```

## 📁 Project Structure

```
FlashcardApp/
├── .github/workflows/          # CI/CD pipelines
├── scripts/                    # Development scripts
│   ├── dev-workflow.ps1       # Main development workflow
│   ├── pre-commit.ps1         # Pre-commit quality checks
│   └── setup-git-hooks.ps1    # Git hooks setup
├── .editorconfig              # Editor configuration
├── Directory.Build.props      # Build configuration
├── dev.bat                    # Windows batch file
└── DEVELOPMENT.md             # This file
```

## 🎯 Best Practices

### Before Committing
1. Run quality checks: `dev.bat quality`
2. Format code: `dev.bat format`
3. Run tests: `dev.bat test`

### Before Pushing
1. Run all tests: `dev.bat test all`
2. Generate coverage: `dev.bat coverage`
3. Verify build: `dev.bat build`

### Daily Development
1. Setup environment: `dev.bat setup`
2. Clean artifacts: `dev.bat clean`
3. Run quality checks: `dev.bat quality`

## 🚨 Troubleshooting

### Common Issues

#### Pre-commit Hook Fails
```bash
# Bypass hooks (not recommended)
git commit --no-verify -m "message"

# Fix formatting issues
dev.bat format
```

#### Build Failures
```bash
# Clean and rebuild
dev.bat clean
dev.bat build
```

#### Test Failures
```bash
# Run specific test category
dotnet test --filter "Category=Fast"
```

#### Coverage Issues
```bash
# Regenerate coverage
dev.bat coverage
```

## 📈 Continuous Improvement

### Metrics to Monitor
- Test coverage percentage
- Build success rate
- Code quality score
- Performance benchmarks
- Security vulnerabilities

### Regular Tasks
- Update dependencies
- Review coverage reports
- Analyze performance metrics
- Update quality tools
- Review CI/CD pipeline

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make changes with quality checks
4. Run full test suite
5. Submit pull request

The CI/CD pipeline will automatically:
- Run quality gates
- Execute tests
- Generate reports
- Check security
- Build application
