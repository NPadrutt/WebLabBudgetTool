export default class ApiService {

    static readonly baseUrl = 'http://localhost:49180/api/';

    static async makeRequest(endpoint: string,
                      payload: object | undefined = undefined,
                      method: 'GET' | 'POST' | 'PUT' | 'DELETE' = 'GET') {
        let url = ApiService.baseUrl + endpoint;
        let request = <RequestInit>{
            method: method,
            headers: {'Content-Type': 'application/json'},
            credentials: 'same-origin'
        };

        if (localStorage.accessToken !== undefined) {
            if (request.headers === undefined) {
                request.headers = {};
            }
            request.headers['Authorization'] = 'Bearer ' + localStorage.accessToken;
        }

        if (payload !== undefined) {
            request['body'] = JSON.stringify(payload);
        }

        return await fetch(url, request);
    }
}