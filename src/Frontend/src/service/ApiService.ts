export default class ApiService {

    static readonly baseUrl = 'http://localhost:49180/api/';

    async makeRequest(endpoint: string, payload: object, method: 'GET' | 'POST' | 'PUT' | 'DELETE') {
        let url = ApiService.baseUrl + endpoint;

        return await fetch(url, {
            method: method,
            body: JSON.stringify(payload),
            headers: {'Content-Type': 'application/json'},
            credentials: 'same-origin'
        });
    }
}