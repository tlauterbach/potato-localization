# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.3] - 2021-06-11
### Fixed
- Hash value of `LocalizationKey` now creates and serializes itself properly

## [0.2.2] - 2021-06-08
### Fixed
- `LocalizationMgr` now has a script execution order of -50 so that text objects will always initialize after it

## [0.2.1] - 2021-06-04
### Fixed
- Fixed `LocalizedTextUGUI` to require the correct component

## [0.2.0]
### Added
- Inspector `PropertyDrawer` for `LocalizationKey`

## [0.1.1] - 2021-05-24
### Added
- Missing .asmdef file

### Fixed
- incorrect dependencies for package.json file

## [0.1.0] - 2021-05-24
### Added
- Base scripts for creating localization assets and manager

### Notes
- Not battle-tested in a game, may need immediate patching 