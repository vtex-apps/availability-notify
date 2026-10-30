# Availability Notify

> **DEPRECATED.** The `0.x` major of `vtex.availability-notify` no longer has any functionality.

The `availability-notify` block still exists so stores that declare it don't break, but it renders an empty `<div>`. The backend (GraphQL API, events and e-mail sending) was removed, and the HTTP routes respond `410 Gone`.

## Migrating to 1.x

The block name is the same. In your store theme `manifest.json`, change the dependency:

```diff
- "vtex.availability-notify": "0.x"
+ "vtex.availability-notify": "1.x"
```
