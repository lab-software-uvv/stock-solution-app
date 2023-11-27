import axios, { Axios } from "axios";
import api from "../environment/backend";

const Requests = axios.create ({
    baseURL: api,
    headers: {
        "Content-Type": "application/json",
    }
})

export default Requests;