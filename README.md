# WebFTP
ASP.NET web-based FTP-like service that can be interacted from unix command line

Typically you will need to publish this website to an IIS server

Change your `FTP_ROOT` folder in the `Web.config` file and set proper permissions for IIS user

# Example

interact using curl

```shell
curl http://example.com/WebFTP.aspx --silent --form "command=ls" --form "argpath="
curl http://example.com/WebFTP.aspx --silent --form "command=put" --form "argpath=TEST.zip" --form "upload=@TEST.zip"
curl http://example.com/WebFTP.aspx --silent --form "command=get" --form "argpath=TEST.zip" --fail --output TEST.zip
curl http://example.com/WebFTP.aspx --silent --form "command=delete" --form "argpath=TEST.zip"
curl http://example.com/WebFTP.aspx --silent --form "command=mkdir" --form "argpath=TEST/"
curl http://example.com/WebFTP.aspx --silent --form "command=rmdir" --form "argpath=TEST/"
```
