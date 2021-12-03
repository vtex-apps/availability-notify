📢 Use this project, [contribute](https://github.com/vtex-apps/reviews-and-ratings) to it or open issues to help evolve it using [Store Discussion](https://github.com/vtex-apps/store-discussion).

<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->

[![All Contributors](https://img.shields.io/badge/all_contributors-2-orange.svg?style=flat-square)](#contributors-)

<!-- ALL-CONTRIBUTORS-BADGE:END -->

# Availability Notify

## Description

`AvailabilityNotfier` is a VTEX Component that shows the availability notify form that is shown when the product isn't available.
The app will record the request for notification and monitor inventory updates.  When the requested sku is back in stock, the app
will send an email to the shopper that requested to be notified.

## Usage

To use this app, you need to import it in your store theme peer dependencies on manifest.json.
```json
  "peerDependencies": {
    "vtex.availability-notify": "1.x"
  }
```
Then, you can add the `availability-notify` component block to your PDP in your store theme.

The email template that will be used is `back-in-stock`

*NOTE: notification email is only triggered when on the `master` workspace*

## Configuration

![image](https://user-images.githubusercontent.com/47258865/144638028-32f060ee-9b73-4588-aa00-731afb862b1e.png)


`Verify Availability` runs a shipping simulation to verify that the item can be shipped to the shopper before sending a notificaiton.
`Marketplace to Notify` allows a seller account to specify a comma separated list of marketplace account names to notify of inventory updates.

<!-- DOCS-IGNORE:start -->

## Contributors ✨

Thanks goes to these wonderful people:

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
  </tr>
</table>

<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!

<!-- DOCS-IGNORE:end -->
