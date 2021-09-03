📢 Use this project, [contribute](https://github.com/vtex-apps/reviews-and-ratings) to it or open issues to help evolve it using [Store Discussion](https://github.com/vtex-apps/store-discussion).

<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->

[![All Contributors](https://img.shields.io/badge/all_contributors-2-orange.svg?style=flat-square)](#contributors-)

<!-- ALL-CONTRIBUTORS-BADGE:END -->

# Availability Notify

The Availability Notify component is responsible for showing a subscription form when a product SKU isn't available. The form lets customers subscribe to get notified when that item gets restocked. 


![store-notifier](https://user-images.githubusercontent.com/67270558/132012045-06c65073-2692-4827-b08a-7be5730b6422.png)


The app records the notification request and monitors inventory updates. This way, once the requested SKU gets back in stock, the app will email the shoppers who asked to be notified.

## Configuration

1. [Install](https://developers.vtex.com/vtex-developer-docs/docs/vtex-io-documentation-installing-an-app) the Availability Notify app in the desired VTEX account by running `vtex install vtex.availability-notify` in your terminal.

2. Open your store’s Store Theme app directory in your code editor.

3. Open your app's `manifest.json file` and add the Availability Notify app under the `peerDependencies` field.

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

5. Once you have added the `availability-notify` component, access your store's Admin.

7. Go to **Customer** > **Message center** > **Templates**.

9. Search for the `availability-notify` component template, named **BACK IN STOCK** and click on it.

11. After, you will see the email template and its configuration. For example:


![template-back-in-stock](https://user-images.githubusercontent.com/67270558/131547198-a4eb3f0e-5a20-4e63-9f1f-d3bb312fa621.gif)
>ℹ️ The email template that will be used is `back-in-stock`


Now you set up the template according to your necessities. Check out the next section, [Setting up the Back in stock template](#setting-up-the-back-in-stock-template), to learn how to do it.

## Setting up the Back in stock template
🚧🚧


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
