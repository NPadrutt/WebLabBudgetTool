export default class ApiService {

    static readonly baseUrl = 'http://localhost:49180/api/';

    async makeRequest(endpoint: string, payload: object, method: 'GET' | 'POST' | 'PUT' | 'DELETE') {
        let url = ApiService.baseUrl + endpoint;
        let formData = new FormData();
        for (let key in payload) {
            formData.append(key, payload[key]);
        }

        return await fetch(url, {
            method: method,
            body: formData,
            headers: {'Content-Type': 'application/json'},
            credentials: 'same-origin'
        });
    }
}