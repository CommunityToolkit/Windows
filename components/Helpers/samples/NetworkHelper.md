---
title: NetworkHelper
author: nmetulev
description: The NetworkHelper class is used to determine whether the app has Internet, and if it is on a metered Internet connection.
keywords: Helpers, NetworkHelper
dev_langs:
  - csharp
category: Helpers
subcategory: Layout
discussion-id: 0
issue-id: 0
icon: assets/icon.png
---

# NetworkHelper

The [NetworkHelper](/dotnet/api/microsoft.toolkit.uwp.connectivity.networkhelper) class provides functionality to monitor changes in network connection and allows users to query for network information without additional lookups.

It exposes network information though a property called ConnectionInformation. The [ConnectionInformation](/dotnet/api/microsoft.toolkit.uwp.connectivity.connectioninformation) holds information about ConnectionType, ConnectivityLevel, ConnectionCost, SignalStrength, Internet Connectivity and more.

**_What is a metered connection?_**
A metered connection is an Internet connection that has a data limit or cost associated with it. Cellular data connections are set as metered by default. Wi-Fi network connections can be set to metered, but aren't by default. Application developers should take metered nature of connection into account and reduce data usage.

> [!Sample NetworkHelperSample]


## NetworkHelper Properties

| Property | Type | Description |
| -- | -- | -- |
| ConnectionInformation | [ConnectionInformation](/dotnet/api/microsoft.toolkit.uwp.connectivity.connectioninformation) | Gets instance of ConnectionInformation |
| Instance | NetworkHelper | Gets public singleton property |


## ConnectionInformation Properties

| Property | Type | Description |
| -- | -- | -- |
|        ConnectionCost         |           [ConnectionCost](/uwp/api/Windows.Networking.Connectivity.ConnectionCost)           |       Gets connection cost for the current Internet Connection Profile        |
|        ConnectionType         |       [ConnectionType](/dotnet/api/microsoft.toolkit.uwp.connectivity.connectiontype)        |       Gets connection type for the current Internet Connection Profile        |
|       ConnectivityLevel       | [NetworkConnectivityLevel](/uwp/api/Windows.Networking.Connectivity.NetworkConnectivityLevel) |      Gets connectivity level for the current Internet Connection Profile      |
|      IsInternetAvailable      |                                                          bool                                                           | Gets a value indicating whether internet is available across all connections  |
| IsInternetOnMeteredConnection |                                                          bool                                                           | Gets a value indicating whether if the current internet connection is metered |
|         NetworkNames          |                                                  IReadOnlyList\<string>                                                  |       Gets signal strength for the current Internet Connection Profile        |
|        SignalStrength         |                                                     Nullable\<Byte>                                                      |       Gets signal strength for the current Internet Connection Profile        |


## ConnectionInformation Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| UpdateConnectionInformation(ConnectionProfile) | void | Updates the current object based on profile passed |


## NetworkHelper Events

| Events | Description |
| -- | -- |
| NetworkChanged | Event raised when the network changes |
