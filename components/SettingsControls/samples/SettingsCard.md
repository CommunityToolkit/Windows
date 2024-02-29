---
title: SettingsCard
author: niels9001
description: A card control that can be used to create Windows 11 style settings experiences.
keywords: SettingsCard, Control, Layout, Settings
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 0
issue-id: 0
icon: Assets/SettingsCard.png
---

SettingsCard is a control that can be used to display settings in your experience. It uses the default styling found in Windows 11 and is easy to use, meets all accessibility standards and will make your settings page look great!
You can set the `Header`, `Description`, `HeaderIcon` and `Content` properties to create an easy to use experience, like so:

> [!SAMPLE SettingsCardSample]

SettingsCard can also be turned into a button, by setting the `IsClickEnabled` property. This can be useful whenever you want your settings component to navigate to a detail page or open an external link. You can set a custom icon by setting the `ActionIcon`, or hiding it completely by setting the `IsActionIconVisible` to `false`.

> [!SAMPLE ClickableSettingsCardSample]
