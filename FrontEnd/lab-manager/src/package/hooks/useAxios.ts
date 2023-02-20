import { useEffect } from "react";
import useAuth from "./useAuth";
import { AxiosHeaders, AxiosRequestConfig } from "axios";
import AxiosPrivate from "../apis/AxiosPrivate";

const useAxios = () => {
	const { auth, handlerLogOut } = useAuth();

	/* --------------------------------------------------------------------------------------------- */
	// Request Intercept
	useEffect(() => {
		const requestIntercept = AxiosPrivate.interceptors.request.use(
			(config: AxiosRequestConfig) => {
				if (!config) config = {} as AxiosRequestConfig;

				if (!config.headers) config.headers = {};

				(config.headers as AxiosHeaders).set(
					"Authorization",
					`Bearer ${auth?.Token}`,
				);
				return config;
			},
			error => Promise.reject(error),
		);

		/* --------------------------------------------------------------------------------------------- */
		// Response Intercept
		const responseIntercept =
			AxiosPrivate.interceptors.response.use(
				response => response,
				async error => {
					// Errore di Auth con il Back-End, Log out immediato dell'Utente
					// 401 : Unauthorized
					// 403 : Forbidden
					if (
						error.response.status === 401 ||
						error.response.status === 403
					)
						handlerLogOut();

					return Promise.reject(error);
				},
			);

		return () => {
			AxiosPrivate.interceptors.request.eject(requestIntercept);
			AxiosPrivate.interceptors.response.eject(responseIntercept);
		};
	}, [auth, handlerLogOut]);

	return AxiosPrivate;
};

export default useAxios;
