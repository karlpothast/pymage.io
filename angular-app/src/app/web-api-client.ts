import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

export interface IGetOCRTextClient {
    get(): Observable<GetOCRText>;
}

@Injectable({
    providedIn: 'root'
})
export class GetOCRTextClient implements IGetOCRTextClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    get(): Observable<GetOCRText> {
        let url_ = this.baseUrl + "/api/ocr/GetOCRText";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGet(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGet(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<GetOCRText>;
                }
            } else
                return _observableThrow(response_) as any as Observable<GetOCRText>;
        }));
    }

    protected processGet(response: HttpResponseBase): Observable<GetOCRText> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);

            return _observableOf(resultData200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }
}

export class GetOCRText implements IGetOCRText {
    extracted_text?: string | undefined;

    constructor(data?: IGetOCRText) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    //console.log("init function : ");
    init(_data?: any) {
        if (_data) {
            this.extracted_text = _data["extracted_text"];
        }

    }

    static fromJS(data: any): GetOCRText {
        data = typeof data === 'object' ? data : {};
        let result = new GetOCRText();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["extracted_text"] = this.extracted_text;
        return data;
    }
}

export interface IGetOCRText {
    extracted_text?: string | undefined;
}

export class SwaggerException extends Error {
    override message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isSwaggerException = true;

    static isSwaggerException(obj: any): obj is SwaggerException {
        return obj.isSwaggerException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new SwaggerException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = event => {
                observer.next((event.target as any).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}

export class GetBrowserInfoClient { 
    public appMode;
    public navigatorPlatform;
    public userAgent;
    public browser;
    public device;
    public errMsg;

    constructor() {
      this.userAgent = window.navigator.userAgent;
      this.browser = "";
      this.navigatorPlatform = window.navigator.platform;
      this.appMode = "button";

      switch (true) { 
        case (this.userAgent.indexOf("Chrome") > -1):
          this.browser = "Chrome";
          break;
        case (this.userAgent.indexOf("Safari") > -1):
          this.browser = "Safari";
          break;
        case (this.userAgent.indexOf("Edge") > -1):
          this.browser = "Edge";
          break;
        case (this.userAgent.indexOf("Firefox") > -1):
          this.browser = "Firefox";
          break;
        case (this.userAgent.indexOf("Opera") > -1):
          this.browser = "Opera";
          break;
        default:
          this.browser = "Other";
          break;
      }

      if (this.browser == "Firefox") {
        this.appMode = "paste";
       }
    }

}









