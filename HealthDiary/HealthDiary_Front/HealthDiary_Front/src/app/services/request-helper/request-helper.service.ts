import { Injectable } from '@angular/core';
import { RequestHelperBaseService } from '../request-helper-base/request-helper-base.service';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/app/environment';

@Injectable()
export abstract class RequestHelperService extends RequestHelperBaseService {

  constructor(protected override http: HttpClient) {
    super(http);
    this.basePath = environment.basePortalPath;
    this.apiPath = environment.portalPath;
    this.actionUrl = this.basePath + this.getApiRoute();
  }
}
