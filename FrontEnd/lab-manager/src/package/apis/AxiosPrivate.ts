import axios from "axios"
import config from "package/config.json"

export const AxiosPrivate = axios.create({
	baseURL: config.BASE_URL,
	headers: { "Content-Type": "application/json" }
})

export default AxiosPrivate
