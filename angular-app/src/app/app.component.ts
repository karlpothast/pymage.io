import { Component } from '@angular/core';
import { GetBrowserInfoClient } from './web-api-client';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
   styleUrls: ['./app.component.scss'],
   providers: [GetBrowserInfoClient]
})
export class AppComponent {
  title = 'app';
  public navigatorPlatform;
  public userAgent;
  public browser;
  public device;
  public errMsg;
  public appMode;

  constructor(public clientAppInfo: GetBrowserInfoClient) {
    this.appMode = this.clientAppInfo.appMode;
    switch (this.navigatorPlatform) {
      case this.navigatorPlatform == "Linux x86_64":
        this.browser = "Linux x86_64";
        break;
      default:
       this.browser = "Other";
       break;
    }

    //iPhone, iPad, Win32
    //screenshot to text with a snip and a click
    //alert(this.navigatorPlatform)
  } 
}


  

