# Changelog

All notable changes to this project will be documented in this file.
The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Removed

- All functionality of the 0.x major: GraphQL API, events, e-mail sending and the subscription form. The `availability-notify` block now renders an empty `<div>` so stores still on 0.x don't break, and the HTTP routes respond `410 Gone` with a deprecation message. Migrate to `vtex.availability-notify@1.x`.

## [0.1.3] - 2022-01-19

### Fixed

- Block broke when trying to access properties from `undefined` sellers while product query was still running

## [0.1.2] - 2021-09-01

### Added

- Add icon, images, licenses, and descriptions for app store submission

## [0.1.1] - 2021-08-26

### Fixed

- Fixed search query for notifications

## [0.1.0] - 2021-08-25

### Added

- Add Locale and Seller

## [0.0.11] - 2021-07-14

### Added

- Added logging

## [0.0.10] - 2021-06-11

### Added

- View requests
- Delete request

## [0.0.9] - 2021-06-09

### Added

- Processing routes

## [0.0.8] - 2021-06-08

## [0.0.7] - 2021-06-07

## [0.0.6] - 2021-06-07

### Added

- Docs

### Changed

- Request obj

## [0.0.5] - 2021-06-04

## [0.0.4] - 2021-06-04

## [0.0.3] - 2021-06-04

### Added

- Install Event

## [0.0.2] - 2021-06-03

### Changed

- Name change

## [0.0.1] - 2021-05-28

### Added

- Initial version
