import { HttpParams } from '@angular/common/http';

export class QueryParameters {
  pageNumber: number = 1;
  pageSize: number = 10;
  searchTerm: string = '';

  getQueryString() {
    let queryString = '';
    queryString += this.pageNumber + '-';

    queryString += this.pageSize;

    return queryString;
  }

  getHttpParamsObject() {
    let params = new HttpParams();
    params = params.append('pageNumber', this.pageNumber);

    params = params.append('pageSize', this.pageSize);

    if (this.searchTerm) {
      params = params.append('searchTerm', this.searchTerm);
    }

    return params;
  }
}
