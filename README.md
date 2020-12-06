# Saber-ImportExport
A vendor plugin for [Saber](https://saber.datasilk.io) that allows webmasters to backup & restore web content for their Saber website using a simple zip file. 

With this plugin, you'll be able to make changes to your website HTML, CSS, LESS, & Javascript within a development environment, then upload the changes to a production environment without having to republish the Saber .NET Core application again. Simply download the exported zip file from your development environment and import (upload) the zip file into your production environment from the **Website Settings** section within Saber.

### Prerequisites
* [Saber](https://saber.datasilk.io) ([latest release](https://github.com/Datasilk/Saber/releases))

### Installation
#### For Visual Studio Users
* Clone this repository inside your Saber project within `/App/Vendors/` and name the folder **ImportExport**
	> NOTE: use `git clone` instead of `git submodule add` since the contents of the Vendor folder is ignored by git
* Run `gulp vendors` from the root of your Saber project folder

#### For DevOps Users
While using the latest release of Saber, do the following:
* Download latest release of [Saber.Vendors.ImportExport](https://github.com/Datasilk/Saber-ImportExport/releases)
* Extract all files & folders from either the `win-x64` or `linux-x64` zip folder to Saber's `/Vendors/` folder

### Publish
* run command `./publish.bat`
* publish `bin/Publish/ImportExport.7z` as latest release

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
    * **my-page-folder**
      * image-001.jpg
      * image-002.png
      * image-03.gif
      * doc-001.pdf
      * doc-002.docx
    * **/my-sub-folder**
      * my-page.json
      * my-page.html
      * my-page.less
      * my-page.js
      * **my-page-folder**
        * image-001.jpg
        * image-002.png
        * image-03.gif
        * doc-001.pdf
        * doc-002.docx
  * **/partials**
    * my-partial.html
    * my-partial.less
    * **my-partials-folder**
      * my-partial.html
      * my-partial.less
* **/CSS**
  * **website.less**
* **/wwwroot**
  * **/js**
    * **website.js**
    * my-js-file.js
  * **/my-folder**
    * any file type (.jpg, .png, .gif, .ico, .js, .css, .svg, .avi, .mpg, etc)
  * **/content**
    * **/pages**
      * **/my-page**
        * any files assoiated with a page  (.jpg, .png, .gif, .pdf, .zip, etc)

##### Ignored Folder Structure
Do not include any of the folders defined below within your zip file. These are system folders reserved for the Saber platform only.
* **/wwwroot/editor**

##### Zip File Processing
After importing your zip file, all acceptable files will be copied to their respective folders, then all **less* files will be compiled to **css** and copied to the **wwwroot** folder and nny **js** files outside of the **wwwroot** folder will be copied into the **wwwroot** folder as well.

> **NOTE**: The **wwwroot** folder is a public-facing folder that any user can download content from.