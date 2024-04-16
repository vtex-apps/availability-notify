# Changelog

All notable changes to this project will be documented in this file.
The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.13.0] - 2024-04-16

### Changed
- Updated GraphQL dependency to 3

## [1.12.2] - 2024-01-24

### Fixed
- Fixed broadcaster filter

## [1.12.1] - 2023-11-22

## [1.12.0] - 2023-11-21

### Changed
- Skip call to delete lockfile

## [1.11.4] - 2023-11-14

### Changed
- Delete lockfiles

## [1.11.3] - 2023-08-17

### Changed
- Updated docs

## [1.11.2] - 2023-08-08

### Fixed

- Discard catalog broadcaster events earlier if they are not inventory-related, avoiding unneccesary calls to vbase

## [1.11.1] - 2023-03-30

### Fixed

- German translation.

## [1.11.0] - 2023-01-30

### Added

- Added check for admin token to list requests

### Added

- [ENGINEERS-282] - Verify user gets email on product availability

## [1.10.0] - 2023-01-17

### Changed

- Consider unlimited inventory as in-stock

## [1.9.0] - 2022-12-23

### Fixed

- Arabic, English and Norwegian translations.

### Added

- Catalan, Czech, German, Greek, Finnish, Hungarian, Indonesian, Polish, Russian, Swedish and Ukrainian translations.

### Added

- GitHub dispatch workflow added

### Changed

- Cypress improvements

### Changed

- GitHub reusable workflow uptaded to v2

## [1.8.8] - 2022-10-18

### Fixed

- Reduce logging and add throttling for catalog events

## [1.8.7] - 2022-09-13

### Added

- Added error handling when reading orders broadcaster notification

## [1.8.6] - 2022-08-22

### Added

- Added policy to handle request for MD

## [1.8.5] - 2022-08-08

### Fixed

- (NOTIFIER-56) Fix processUnsentRequests mutation response.

## [1.8.4] - 2022-07-27

### Changed

- (NOTIFIER-40) Make private the processUnsentRequests service.
- (NOTIFIER-40) Create graphQL mutations for processUnsentRequests.
- (NOTIFIER-40) Admin access permission check for mutations processUnsentRequests and deleteRequest.

## [1.8.3] - 2022-07-15

### Changed

- (NOTIFIER-50) Added LMProductId prop to admin route.

## [1.8.2] - 2022-07-15

### Changed

- Changed lock file to be sku specific

## [1.8.1] - 2022-07-13

### Changed

- (NOTIFIER-49) Changed errors to warnings when forwarding a notification.

## [1.8.0] - 2022-07-11

### Added

- (NOTIFIER-19) Added admin page with download of request records and button to process unsent requests

## [1.7.9] - 2022-06-29

### Added

- (NOTIFIER-46) Added logging and error handling listing notify requests

## [1.7.8] - 2022-06-28

### Fixed

- Fixed deserialization errors

## [1.7.7] - 2022-06-24

### Fixed

- Fixed deserialization errors

## [1.7.6] - 2022-06-22

### Fixed

- (NOTIFIER-39) Fixed Cart Simulation logic when verifying that a sku can ship to a shopper

## [1.7.5] - 2022-06-21

- When forwarding a request, do not forward to self.

## [1.7.4] - 2022-06-21

### Fixed

- (NOTIFIER-39) Fixed Cart Simulation logic when verifying that a sku can ship to a shopper

### Changed

- (NOTIFIER-44) Updated manual processing links

## [1.7.3] - 2022-05-16

(NOTIFIER-31) Changed forwarding address

## [1.7.2] - 2022-05-13

### Changed

- (NOTIFIER-31) Changed access policy to public.

## [1.7.1] - 2022-05-12

### Changed

- (NOTIFIER-31) Updated app version in access policy.

## [1.7.0] - 2022-05-10

### Added

- Periodically process all unsent notifications

## [1.6.7] - 2022-05-06

### Changed

- Updated readme - added the configuration for sellers

## [1.6.6] - 2022-05-05

### Changed

- (NOTIFIER-31) Trim account name when forwarding notification.
- Added logging

## [1.6.5] - 2022-04-07

### Fix

- Added validation and avoid subscribing more than once for the same sku

## [1.6.4] - 2022-03-11

### Fix

- Added validation and request to get sku from marketplace

## [1.6.3] - 2022-03-09

### Changed

- (NOTIFIER-26) Changed lock error response type

## [1.6.2] - 2022-03-07

### Added

- Added error handling

## [1.6.1] - 2022-03-04

### Changed

- (NOTIFIER-24) Changed lock error to 429

## [1.6.0] - 2022-03-04

### Added

- (NOTIFIER-24) Added lock file to block notifications during processing

## [1.5.0] - 2022-03-04

### Added

- Arabic, Norwegian and Norwegian variant translation.

## [1.4.6] - 2022-03-03

## [1.4.5] - 2022-02-25

### Added

- Added info back to README (this app works on master workspace)

## [1.4.4] - 2022-02-07

### Fixed

- Change property type to string

## [1.4.3] - 2022-01-21

### Added

- Policy to the settings process notification service
- Add wildcard to allow request from any account

### Fixed

- Change type for PriceValidUntil
- Added outbound access to the availability-notify endpoint

## [1.4.2] - 2022-01-20

### Fixed

- Tooling and linting

## [1.4.1] - 2022-01-19

### Fixed

- fix property when receiving and undefined value

### Added

- Quality Engineering Actions (SonarCloud for TS and C# plus Lint for TS)

## [1.4.0] - 2022-01-06

## [1.3.7] - 2021-12-17

### Fixed

- Fix ForwardNotification deserialization
- Add log for notification email

## [1.3.2] - 2021-12-08

### Added

- Crowdin configuration file

## [1.3.1] - 2021-12-02

### Changed

- Check seller inventory when getting availability

## [1.3.0] - 2021-11-22

### Changed

- Updated default template and dependecy version
- Added option to forward notifications to another account

## [1.2.1] - 2021-10-22

### Added

- filter out inactive warehouses
- Add new model ListAllWarehouses
- Add new Request ListAllWarehouses

## [1.2.0] - 2021-10-15

### Added

- Updated README to let developers know that the broadcaster doesn't triger the email on dev workspaces

## [1.1.1] - 2021-10-04

## [1.1.0] - 2021-09-21

### Added

- Added option to run shipping simulation to verify that the item can be shipped.

## [1.0.1] - 2021-09-02

### Added

- Update billing options with type and availableCountries fields

## [1.0.0] - 2021-09-02

### Added

- Add billing options to manifest

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
