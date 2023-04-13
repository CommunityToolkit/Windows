---
title: Primitives
author: githubaccount
description: TODO: Your experiment's description here
keywords: Primitives, Control, Layout
dev_langs:
  - csharp
category: Controls
subcategory: Layout
discussion-id: 0
issue-id: 0
---

<!-- To know about all the available Markdown syntax, Check out https://docs.microsoft.com/contribute/markdown-reference -->
<!-- Ensure you remove all comments before submission, to ensure that there are no formatting issues when displaying this page.  -->
<!-- It is recommended to check how the Documentation will look in the sample app, before Merging a PR -->
<!-- **Note:** All links to other docs.microsoft.com pages should be relative without locale, i.e. for the one above would be /contribute/markdown-reference -->
<!-- Included images should be optimized for size and not include any Intellectual Property references. -->

<!-- Be sure to update the discussion/issue numbers above with your Labs discussion/issue id numbers in order for UI links to them from the sample app to work. -->

# Primitives

TODO: Fill in information about this experiment and how to get started here...

## Custom Control

You can inherit from an existing component as well, like `Panel`, this example shows a control without a
XAML Style that will be more light-weight to consume by an app developer:

> [!Sample PrimitivesCustomSample]

## Templated Controls

The Toolkit is built with templated controls. This provides developers a flexible way to restyle components
easily while still inheriting the general functionality a control provides. The examples below show
how a component can use a default style and then get overridden by the end developer.

TODO: Two types of templated control building methods are shown. Delete these if you're building a custom component.
Otherwise, pick one method for your component and delete the files related to the unchosen `_ClassicBinding` or `_xBind`
classes (and the custom non-suffixed one as well). Then, rename your component to just be your component name.

The `_ClassicBinding` class shows the traditional method used to develop components with best practices.

### Implict style

> [!SAMPLE PrimitivesTemplatedSample]

### Custom style

> [!SAMPLE PrimitivesTemplatedStyleCustomSample]

## Templated Controls with x:Bind

This is an _experimental_ new way to define components which allows for the use of x:Bind within the style.

### Implict style

> [!SAMPLE PrimitivesXbindBackedSample]

### Custom style

> [!SAMPLE PrimitivesXbindBackedStyleCustomSample]

