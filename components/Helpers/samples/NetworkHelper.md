---
title: NetworkHelper
author: nmetulev
description: The NetworkHelper class is used to determine whether the app has Internet, and if it is on a metered Internet connection.
keywords: Helpers, NetworkHelper
dev_langs:
  - csharp
category: Helpers
subcategory: System
discussion-id: 0
issue-id: 0
icon: Assets/NetworkHelper.png
---

It exposes network information though a property called ConnectionInformation. The `ConnectionInformation` holds information about ConnectionType, ConnectivityLevel, ConnectionCost, SignalStrength, Internet Connectivity and more.

**_What is a metered connection?_**
A metered connection is an Internet connection that has a data limit or cost associated with it. Cellular data connections are set as metered by default. Wi-Fi network connections can be set to metered, but aren't by default. Application developers should take metered nature of connection into account and reduce data usage.

> [!Sample NetworkHelperSample]
