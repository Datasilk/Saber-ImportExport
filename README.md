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

### Zip File Structure
##### Accepted Folder Structure
Please make sure the files contained within the zip file you wish to import follows the folder structure below. None of the folders or files below are required to be included within your zip file.

> **NOTE:** Folder & file names that are labeled "**my-**" are an example of custom folders or files that can be named however you want. Also, you can create as many custom folders or files as you want within the given folder structure where ever the folder or file names are labeled with "**my-**".

* **/Content**
  * **/pages**
    * my-page.json
    * my-page.html
    * my-page.less
    * my-page.js
    * **/my-sub-page-folder**
      * my-page.json
      * my-page.html
      * my-page.less
      * my-page.js
  * **/partials**
    * my-partial.html
    * my-partial.less
    * **my-partials-folder**
      * my-partial.html
      * my-partial.less
* **/CSS**
  * **website.less**
* **/Scripts**
  * **website.js**
* **/wwwroot**
  * **/my-folder**
    * any file type (.jpg, .png, .gif, .ico, .js, .css, .svg, .avi, .mpg, etc)

##### Ignored Folder Structure
Do not include any of the folders defined below within your zip file. These are system folders reserved for the Saber platform only.
* **/wwwroot/content**
* **/wwwroot/css**
* **/wwwroot/editor**
* **/wwwroot/js**
* **/wwwroot/themes**

##### Zip File Processing
After importing your zip file, all acceptable files will be copied to their respective folders, then all **less* files will be compiled to **css** and copied to the **wwwroot** folder to their folders. Any **js** files outside of the **wwwroot** folder will be copied into the **wwwroot** folder as well.

> **NOTE**: The **wwwroot** folder is a public-facing folder that any user can download content from.