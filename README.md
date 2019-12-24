# Saber-ImportExport
A vendor plugin for [Saber](https://github.com/Datasilk/Saber) that allows webmasters to backup & restore web content for their Saber website using a simple zip file. 

With this plugin, you'll be able to make changes to your website HTML, CSS, LESS, & Javascript within a development environment, then upload the changes to a production environment without having to republish the Saber .NET Core application again. Simply download the exported zip file from your development environment and import (upload) the zip file into your production environment from the **App Settings** section within Saber.

### Prerequisites
* Visual Studio 2017
* Clone [Saber](https://github.com/Datasilk/Saber) repository

### Installation
* Clone this repository inside your Saber project within `/App/Vendor/` and name the folder **ImportExport**
	> NOTE: use `git clone` instead of `git submodule add` since the contents of the Vendor folder is ignored by git
* Run `gulp default`