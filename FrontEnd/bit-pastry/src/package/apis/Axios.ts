import axios from "axios"
import config from "package/config.json"

export const Axios = axios.create({
	baseURL: config.BASE_URL
})

export default Axios
