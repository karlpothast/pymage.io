import { Component } from '@angular/core';
import { GetBrowserInfoClient, GetOCRTextClient, GetOCRText } from '../web-api-client';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
   providers: [GetBrowserInfoClient]

})
export class HomeComponent {
  public ocrresult;
  public errMsg;
  public extracted_text;
  public extracted_text_formatted;
  public textCopiedNotification;
  public appMode;
  public userAgent;
  public browser;
  
  constructor(public clientAppInfo: GetBrowserInfoClient, public client: GetOCRTextClient) {  
    this.appMode = this.clientAppInfo.appMode;
    this.userAgent = this.clientAppInfo.userAgent;
    this.browser =  this.clientAppInfo.browser;
  }

  public copyText(modifiedText) {
    navigator.clipboard.writeText(modifiedText);
    this.textCopiedNotification = "Text copied successfully!";
  }

}