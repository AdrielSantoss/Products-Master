import { Observable } from 'rxjs';

import { map } from 'rxjs/operators';

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { JsonLocalStorage, JsonStorage } from './json.storage';

interface UserInfo {
    accessToken: string;
    refreshToken: string;
    refreshAt: number;
    id: string;
    email: string;
}

@Injectable()
export class AuthService {
    private userInfo: UserInfo | null = null;
    private readonly userInfoStorage: JsonStorage<UserInfo>;

    constructor(private http: HttpClient){
        this.userInfoStorage = new JsonLocalStorage('portal-diniz-user-info');
        this.userInfo = this.userInfoStorage.load();
    }
    
    postLogin(username: string, password: string): Observable<any> {
        const params = new HttpParams()
        .append('grant_type', 'password')
        .append('scope', 'openid offline_access')
        .append('username', username)
        .append('password', password);

    return this.postConnectToken(params);

    }

    private postConnectToken(params: HttpParams) {
        const options = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        };
        return this.http.post<any>('http://locahost:5000/connect/token', params.toString(), options)
            .pipe(map(response => {
                this.setToken(response);
                return true;
            }));
    }

    private setToken(response: any) {
        // Extraí informações do idToken
        const infoBase64 = response.id_token.split('.')[1];
        const info = JSON.parse(decodeURIComponent(atob(infoBase64)));

        // Atualiza refresh token (se houver)
        let refreshToken = response.refresh_token;
 

        // Calcula próxima data de refresh (em milissegundos) - 1/4 da data de expiração
        const now = new Date();
        const refreshAt = new Date(now.getTime() + (response.expires_in / 4) * 1000).getTime();

        // Salva tokens e informações do usuário
        this.userInfo = {
            accessToken: response.access_token,
            refreshAt,
            refreshToken: response.refresh_token,
            id: info.sub,
            email: info.email
        };
        this.userInfoStorage.save(this.userInfo);
    }


}