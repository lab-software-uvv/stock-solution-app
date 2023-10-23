import Axios from "axios";
import api from "../environment/backend";

class Requests {
    static auth = async (config) => {
        let content;
        await Axios.post(api + "/auth", config)
            .then((res) => {
                const response = res;
                content = response;
            })
            .catch((res, err) => {
                const response = res.response;
                content = response;
            });
        return content;
    };
}

export default Requests;