import axios from "axios";
import api from "../environment/backend";

const Requests = axios.create({
    baseURL: api,
    headers: {
        "Content-Type": "application/json",
    },
});

Requests.interceptors.request.use((request) => {
    // Buscando seu token salvo no localstorage ou qualquer outro local
    const token = localStorage.getItem("token");

    if (token) {
        // Authorization geralmente é o header padrão para envio de token, mas isso não é uma regra. O endpoint pode requisitar outro header.
        request.headers.Authorization = `Bearer ${token}`;
    }
    // Este return é necessário para continuar a requisição para o endpoint.
    return request;
});

export default Requests;
