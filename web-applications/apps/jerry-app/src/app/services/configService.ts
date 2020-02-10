import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root',
})
export class ConfigService {
    private _config: any = null;

    constructor(private http: HttpClient) {
    }

    public getConfig(key: any) {
        if (this._config === null) {
            this.loadAppConfig();
        }
        return this._config[key];
    }

    public loadAppConfig() {
        const env = environment.production ? 'production' : 'development';
        const envConfigFilePath = '../../assets/config/' + env + '.json';
        return this.http.get(envConfigFilePath)
            .toPromise()
            .then(data => {
                this._config = data;
            })
            .catch((error: any) => {
                console.error(error);
                throwError(error.json().error || 'Server error');
            });
    }
}
