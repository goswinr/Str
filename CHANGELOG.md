# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.20.0] - 2025-10-11
### Changed
- BREAKING CHANGE: Change `Str.get` to have index as first parameter and string as second parameter.

## [0.19.0] - 2025-02-20
### Added
- Add replaceLast and replaceFirst

## [0.18.0] - 2024-10-28
### Changed
- Rename extension methods on StringBuilder from `.append` to `.Add`
### Added
-  better documentation
- .IsEmpty extension method
- .IsNotEmpty extension method
- .IsWhite extension method
- .IsNotWhite extension method

## [0.17.2] - 2024-10-26
### Added
- Documentation via [FSharp.Formatting](https://fsprojects.github.io/FSharp.Formatting/)

## [0.17.0] - 2024-09-20
### Changed
- Change `Str` to static class from module

## [0.16.0] - 2024-09-15
### Fixed
- Fix overloads for `if StringComparison`
- Check TS build in Fable

## [0.15.0] - 2024-02-25
### Added
- Implementation ported from [FsEx](https://github.com/goswinr/FsEx/blob/main/Src/StringModule.fs)
- Added more tests


[Unreleased]: https://github.com/goswinr/Str/compare/0.20.0...HEAD
[0.20.0]: https://github.com/goswinr/Str/compare/0.19.0...0.20.0
[0.19.0]: https://github.com/goswinr/Str/compare/0.18.0...0.19.0
[0.18.0]: https://github.com/goswinr/Str/compare/0.17.2...0.18.0
[0.17.2]: https://github.com/goswinr/Str/compare/0.17.0...0.17.2
[0.17.0]: https://github.com/goswinr/Str/compare/0.16.0...0.17.0
[0.16.0]: https://github.com/goswinr/Str/compare/0.15.0...0.16.0
[0.15.0]: https://github.com/goswinr/Str/releases/tag/0.15.0

<!--
use to get tag dates:
git log --tags --simplify-by-decoration --pretty="format:%ci %d"

-->