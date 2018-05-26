import ApiService from "./ApiService";

export default class AuthService {

    static async login(username, password) {
        if (localStorage.accessToken) {
            return true;
        }

        return await ApiService.makeRequest('user/sign-in', {email: username, password: password}, 'POST')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Login not successful');
                }
                return response.json();
            })
            .then(json => {
                this.setToken(json.access_token);
                return true;
            })
            .catch(error => {
                console.error('Login failed with error', error);
                return false;
            });
    }

    private static setToken(token: string) {
        localStorage.accessToken = token;
    }

    static getToken() {
        return localStorage.accessToken
    }

    static logout(callback?: Function) {
        delete localStorage.accessToken;
        if (callback) {
            callback();
        }
    }

    static loggedIn() {
        return !!localStorage.accessToken
    }

}