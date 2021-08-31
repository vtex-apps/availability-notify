📢 Use this project, [contribute](https://github.com/vtex-apps/reviews-and-ratings) to it or open issues to help evolve it using [Store Discussion](https://github.com/vtex-apps/store-discussion).

<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->

[![All Contributors](https://img.shields.io/badge/all_contributors-2-orange.svg?style=flat-square)](#contributors-)

<!-- ALL-CONTRIBUTORS-BADGE:END -->

# Availability Notify

`AvailabilityNotifier` is a VTEX Component that provides an availability notification form that is shown when the product being viewed isn't available.

The app records the request for notification and monitors inventory updates.  When the requested sku is back in stock, the app will send an email to the shopper that requested to be notified.

## Configuration

1. [Install](https://developers.vtex.com/vtex-developer-docs/docs/vtex-io-documentation-installing-an-app) the Availability Notify app in the desired VTEX account by running `vtex install vtex.availability-notify` in your terminal.

2. Open your store’s Store Theme app directory in your code editor.

3. Add the Availability Notify app to your theme’s in your store theme peer dependencies on `manifest.json`.
```json
  "peerDependencies": {
    "vtex.availability-notify": "0.x"
  }
```
4. Add the `availability-notify` component block to your PDP in your store theme (`store.product`). For example:

```json
{
 "store.product": {
    "children": [
      "availability-notify"
    ]
  },

```


>ℹ️ The email template that will be used is `back-in-stock`

## Customization

In order to apply CSS customizations to this and other blocks, follow the instructions given in the recipe on [Using CSS Handles for store customization](https://vtex.io/docs/recipes/style/using-css-handles-for-store-customization).

| CSS Handles             |
| ----------------------- |
| `notiferContainer`|
| `title`       |
| `notifyLabelt`|
| `form`      |
| `content`   |
| `input`     |
| `inputName` |
| `inputEmail`|
| `submit`  |
| `sucess`  |
| `error`   |


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
